using MediatR;
using Microsoft.EntityFrameworkCore;
using RentIt.Application.Exceptions;
using RentIt.Domain.Abstractions;

namespace RentIt.Infrastructure;
internal sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    private readonly IPublisher m_Publisher;

    public ApplicationDbContext(
        DbContextOptions options,
        IPublisher publisher)
        : base(options)
    {
        m_Publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Save changes to Db atomically
        // After publishing events, event processor can fail
        // If we call publish event before SaveChanges,
        // then if SaveChanges can fail, it will create inconsistent behavior
        // To resolve this,
        // Optimistic Locking will be used with OutBox Pattern in DB
        // DB Table creates Version column while updating the column
        // Now while 


        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEventsAsync();

            return result;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception occured.",ex);
        }
    }

    private async Task PublishDomainEventsAsync()
    {
        var domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(entity => entity.Entity)
            .SelectMany(entity =>
            {
                //Extract domain events to avoid another context updating that
                var domainEvents = entity.GetDomainEvents();
                //Clear so that it wont be processed again in same context
                entity.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await m_Publisher.Publish(domainEvent);
        }
    }
}
