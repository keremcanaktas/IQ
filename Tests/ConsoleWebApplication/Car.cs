using IQ.Mofy.Data.Abstractions.Entities;

namespace ConsoleWebApplication;

public class Car : IEntity<int>
{
    public int Id { get; set; }

    public string? Name { get; set; }
}