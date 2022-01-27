using System.Reflection.Emit;

namespace GeneticOptimization.Configuration;

public class PropertyWrapper
{
    private IConfiguration _configuration;
    public string Name { get; }
    public Type Type { get; }
    public bool IsEnum { get; }
    public bool IsArray { get; }
    public Array? EnumValues { get; }

    public object Value
    {
        get
        {
            return _configuration.GetPropertyValue<object>(Name);
        }
        set
        {
            if (value == "") return;
            switch (Type.ToString())
            {
                case "System.Double": 
                    _configuration.SetPropertyValue(Name, double.Parse((string)value));
                    return;
                case "System.Int32": _configuration.SetPropertyValue(Name, int.Parse((string)value));
                    return;
            }
            if(IsEnum) _configuration.SetPropertyValue(Name, value);
        }
    }

    public PropertyWrapper(IConfiguration configuration, string name, Type type)
    {
        Name = name;
        _configuration = configuration;
        Type = type;
        if (typeof(Array).IsAssignableFrom(type)) IsArray = true;
        if(typeof(Enum).IsAssignableFrom(type)) IsEnum = true;
        if (IsEnum)
        {
            EnumValues = Enum.GetValues(type);
        }
    }
    
}