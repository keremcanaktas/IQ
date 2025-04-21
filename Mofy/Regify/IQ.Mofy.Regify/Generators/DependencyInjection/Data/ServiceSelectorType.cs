namespace IQ.Mofy.Regify.Generators.DependencyInjection.Data;

[Flags]
internal enum ServiceSelectorType
{
    None = 0,
    Self = 1,
    DefaultInterface = 2,
    AllInterface = DefaultInterface << 1,
    All = Self | DefaultInterface | AllInterface
}