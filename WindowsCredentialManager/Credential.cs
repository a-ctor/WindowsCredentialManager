namespace WindowsCredentialManager
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.Diagnostics;
  using System.Runtime.InteropServices;
  using Win32;
  using Win32.SafeHandles;
  using Win32.Types;
  using static Win32.UnsafeNativeApi;

  public abstract class Credential
  {
    public string TargetName { get; }

    internal CredentialType Type { get; }

    public string? Comment { get; set; }

    public DateTimeOffset LastModified { get; protected set; }

    public CredentialPersistence Persistence { get; set; } = CredentialPersistence.Session;

    public List<CredentialAttribute> Attributes { get; protected set; } = new List<CredentialAttribute>();

    public string? TargetAlias { get; set; }

    internal Credential (string targetName, CredentialType type)
    {
      TargetName = targetName ?? throw new ArgumentNullException (targetName);
      Type = type;
      LastModified = DateTimeOffset.UtcNow;
    }

    internal abstract unsafe void Deserialize (CREDENTIALW_RAW* credentialW);

    internal abstract void Serialize (ref CREDENTIALW credentialW);

    public unsafe void Load()
    {
      CredentialSafeHandle? handle = null;
      try
      {
        if (!CredReadW (TargetName, Type.ConvertToApiEnum(), 0, out handle))
          throw new Win32Exception();

        var credentialW = handle.AsCredentialW();

        Comment = Marshal.PtrToStringUni (credentialW->Comment);
        LastModified = DateTimeOffset.FromFileTime (credentialW->LastWritten);
        Persistence = credentialW->Persist.ConvertToConsumerEnum();
        TargetAlias = Marshal.PtrToStringUni (credentialW->TargetAlias);
        Attributes = DeserializeAttributes (credentialW->Attributes, credentialW->AttributeCount);

        Deserialize (credentialW);
      }
      finally
      {
        handle?.Dispose();
      }
    }

    private unsafe List<CredentialAttribute> DeserializeAttributes (IntPtr attributes, int count)
    {
      var result = new List<CredentialAttribute>();

      var ptr = (CREDENTIAL_ATTRIBUTEW*) attributes;
      for (var i = 0; i < count; i++)
      {
        var keyword = Marshal.PtrToStringUni (ptr[i].Keyword);
        var value = Win32Utility.UniStringToString (ptr[i].Value, ptr[i].ValueSize) ?? string.Empty;

        result.Add (new CredentialAttribute (keyword, value));
      }

      return result;
    }

    public void Save()
    {
      var freeObjects = new List<IntPtr>();
      SecureBlob? blob = null;
      try
      {
        CREDENTIALW credentialW = default;
        credentialW.Flags = CREDENTIAL_FLAGS.None;
        credentialW.Type = Type.ConvertToApiEnum();
        credentialW.TargetName = TargetName;
        credentialW.Comment = Comment;
        credentialW.LastWritten = default; // LastWritten is ignored on write
        credentialW.BlobSize = 0;
        credentialW.Blob = null;
        credentialW.Persist = Persistence.ConvertToApiEnum();
        credentialW.Attributes = SerializeAttributes (freeObjects, out var attributeCount);
        credentialW.AttributeCount = attributeCount;
        credentialW.TargetAlias = TargetAlias;
        credentialW.UserName = null;

        Serialize (ref credentialW);

        blob = credentialW.Blob;
        credentialW.BlobSize = blob?.Size ?? 0;

        Debug.Assert (credentialW.Type != default, "credentialW.Type != default");
        Debug.Assert (credentialW.TargetName != null, "credentialW.TargetName != null");

        if (!CredWriteW (ref credentialW, 0))
          throw new Win32Exception();
      }
      finally
      {
        blob?.Dispose();
        foreach (var freeObject in freeObjects)
          Marshal.FreeHGlobal (freeObject);
      }
    }

    private unsafe IntPtr SerializeAttributes (List<IntPtr> freeObjects, out int count)
    {
      count = Attributes.Count;
      if (count == 0)
        return IntPtr.Zero;

      var attributeIntPtr = Marshal.AllocHGlobal (sizeof(CREDENTIAL_ATTRIBUTEW) * count);
      freeObjects.Add (attributeIntPtr);

      var ptr = (CREDENTIAL_ATTRIBUTEW*) attributeIntPtr;
      for (var i = 0; i < count; i++)
      {
        var attribute = Attributes[i];

        var keyword = Marshal.StringToHGlobalUni (attribute.Keyword);
        if (keyword != IntPtr.Zero)
          freeObjects.Add (keyword);

        var value = Win32Utility.StringToHGlobalUniWithoutTerminator (attribute.Value, out var valueSize);
        if (value != IntPtr.Zero)
          freeObjects.Add (value);

        ptr[i].Keyword = keyword;
        ptr[i].ValueSize = valueSize;
        ptr[i].Value = value;
      }

      return attributeIntPtr;
    }
  }
}
