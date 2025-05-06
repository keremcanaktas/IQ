namespace IQ.Mofy.Regify.Generators.DependencyInjection.Data;

[Flags]
internal enum ServiceSelectorType
{
    None = 0,
    Self = 1,
    DefaultInterface = 2,
    Interfaces = DefaultInterface << 1,
    AllInterfaces = Interfaces << 1,
    All = Self | DefaultInterface | Interfaces | AllInterfaces
}