namespace WindowsCredentialManager.Win32
{
  using System;
  using System.Runtime.InteropServices;
  using System.Security;

  internal static class Win32Utility
  {
    public static unsafe int GetUniStringLengthWithoutTerminator (IntPtr str)
    {
      var ptr = (char*) str;
      if (ptr == null)
        return 0;

      var start = ptr;
      while (*ptr != 0)
        ptr++;

      return checked((int) (ptr - start)) * sizeof(char);
    }

    public static IntPtr StringToHGlobalUniWithoutTerminator (string? value, out int length)
    {
      var data = Marshal.StringToHGlobalUni (value);
      length = GetUniStringLengthWithoutTerminator (data);

      return data;
    }

    public static unsafe SecureString? UniStringToSecureString (IntPtr data, int dataLength)
    {
      if (dataLength < 0)
        throw new ArgumentOutOfRangeException (nameof(dataLength), dataLength, "The size cannot be less than zero.");
      if (dataLength % 2 != 0)
        throw new ArgumentException ("The specified blob has an invalid size.");

      if (data == IntPtr.Zero)
        return null;

      if (dataLength == 0)
        return new SecureString();

      var characterCount = dataLength / 2;
      return new SecureString ((char*) data, characterCount);
    }

    public static unsafe string? UniStringToString (IntPtr data, int dataLength)
    {
      if (dataLength < 0)
        throw new ArgumentOutOfRangeException (nameof(dataLength), dataLength, "The size cannot be less than zero.");
      if (dataLength % 2 != 0)
        throw new ArgumentException ("The specified blob has an invalid size.");

      if (data == IntPtr.Zero)
        return null;

      if (dataLength == 0)
        return string.Empty;

      var characterCount = dataLength / 2;
      return new string ((char*) data, 0, characterCount);
    }
  }
}
