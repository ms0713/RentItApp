namespace RentIt.Domain.Apartments;


// Why value objects instead primitives:
// 1. Structural Equality
// 2. Uniquely identified by values
// 3. Record is immutable which suits value objects

public record Address(
string Country,
string State,
string ZipCode,
string City,
string Street);
