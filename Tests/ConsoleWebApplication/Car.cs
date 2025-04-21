using ConsoleWebApplication.Data;

namespace ConsoleWebApplication;

public class Car : IEntity<int>
{
    public int Id { get; set; }

    public string? Name { get; set; }
}