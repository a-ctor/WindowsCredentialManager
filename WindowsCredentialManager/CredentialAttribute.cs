namespace WindowsCredentialManager
{
  using System;

  public readonly struct CredentialAttribute : IEquatable<CredentialAttribute>
  {
    public string Keyword { get; }

    public string Value { get; }

    public CredentialAttribute (string keyword, string value)
    {
      Keyword = keyword ?? throw new ArgumentNullException (nameof(keyword));
      Value = value ?? throw new ArgumentNullException (nameof(value));
    }

    /// <inheritdoc />
    public bool Equals (CredentialAttribute other)
    {
      return Keyword == other.Keyword && Value == other.Value;
    }

    /// <inheritdoc />
    public override bool Equals (object? obj)
    {
      return obj is CredentialAttribute other && Equals (other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
      unchecked
      {
        return (Keyword.GetHashCode() * 397) ^ Value.GetHashCode();
      }
    }

    public static bool operator == (CredentialAttribute left, CredentialAttribute right)
    {
      return left.Equals (right);
    }

    public static bool operator != (CredentialAttribute left, CredentialAttribute right)
    {
      return !left.Equals (right);
    }

    /// <inheritdoc />
    public override string ToString()
    {
      return Value != null
        ? $"'{Keyword}' = '{Value}'"
        : $"'{Keyword}' = null";
    }
  }
}
