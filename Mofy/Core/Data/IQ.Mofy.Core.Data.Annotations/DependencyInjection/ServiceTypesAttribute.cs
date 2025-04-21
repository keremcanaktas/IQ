// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InvertIf
// ReSharper disable MemberCanBeProtected.Global
namespace IQ.Mofy.Core.Data.Annotations.DependencyInjection;

[Flags]
public enum ServiceSelectorType
{
    None = 0,
    Self = 1,
    DefaultInterface = 2,
    AllInterface = DefaultInterface << 1,
    All = Self | DefaultInterface | AllInterface
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class ServiceTypesAttribute(params Type[] types) : Attribute
{
    public ServiceTypesAttribute(object key, params Type[] types) : this(types) => Key = key;

    public Type[] Types { get; } = types;

    public object? Key { get; set; }
    
    public ServiceSelectorType ServiceSelectorType { get; set; } = ServiceSelectorType.Self | ServiceSelectorType.DefaultInterface;

    public virtual IEnumerable<Type> GetServiceTypes(Type type)
    {
        foreach (var serviceType in Types)
            yield return serviceType;

        if (Types.Length != 0)
            ServiceSelectorType = ServiceSelectorType.None;
        
        if (ServiceSelectorType.HasFlag(ServiceSelectorType.Self))
            yield return type;

        if (ServiceSelectorType.HasFlag(ServiceSelectorType.AllInterface))
        {
            foreach (var @interface in type.GetInterfaces())
                yield return @interface;
        }
        else if (ServiceSelectorType.HasFlag(ServiceSelectorType.DefaultInterface))
        {
            var defaultInterface = type.GetInterfaces().FirstOrDefault();
            if (defaultInterface != null)
                yield return defaultInterface;
        }

        if (ServiceSelectorType.HasFlag(ServiceSelectorType.All))
            if (type.BaseType is not null)
                yield return type.BaseType;
    }
}

public class ServiceTypesAttribute<T> : ServiceTypesAttribute
{
    public ServiceTypesAttribute(params Type[] types) : base(types) { }
    public ServiceTypesAttribute(object key, params Type[] types) : base(key, types.Concat([typeof(T)]).ToArray()) { }
}