namespace Master.Domain.SeedWork;

public abstract class TypedIdValueBase : IEquatable<TypedIdValueBase>
{
    protected TypedIdValueBase(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public bool Equals(TypedIdValueBase other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        return obj is TypedIdValueBase other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(TypedIdValueBase obj1, TypedIdValueBase obj2)
    {
        if (Equals(obj1, null))
        {
            if (Equals(obj2, null)) return true;
            return false;
        }

        return obj1.Equals(obj2);
    }

    public static bool operator !=(TypedIdValueBase x, TypedIdValueBase y)
    {
        return !(x == y);
    }
}