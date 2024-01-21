using System.Reflection;

namespace Domain.Enums;

public abstract record SmartEnum<TEnum,TValue>(string key,TValue value)
    where TEnum : SmartEnum<TEnum,TValue> 
    where TValue : IEquatable<TValue>,IComparable<TValue>
{
    public static Dictionary<string, TEnum> GetAll()
    {
        var enumType = typeof(TEnum);
        var fieldTypes = enumType.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

        return fieldTypes.ToDictionary(x => x.key);
    }
    public static TEnum GetByKey(string key)
    => GetAll()[key];

    public static IEnumerable<string> GetKeys()
        => GetAll().Select(x=>x.Key);
    public static string CommaSeperatedkeys()
    {
        return $"{string.Join(",",GetKeys())}";
    }

    public static implicit operator TValue (SmartEnum<TEnum, TValue> e)
        => e.value;

    public static implicit operator string(SmartEnum<TEnum,TValue> e)
        => e.key;

    public static implicit operator SmartEnum<TEnum, TValue>(string key)
        => GetAll()[key];

    public bool IsValid()
        => GetByKey(key) != null;
}

public abstract record SmartEnum<TEnum> : SmartEnum<TEnum, string>
    where TEnum : SmartEnum<TEnum>
{
    protected SmartEnum(string key, string value) : base(key, value)
    {
    }
}