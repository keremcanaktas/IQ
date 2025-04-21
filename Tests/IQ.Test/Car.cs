using IQ.Test.Data;

namespace IQ.Test;

public class Car : IEntity<int>
{
    public int Id { get; set; }
    public string? Name { get; set; }
}