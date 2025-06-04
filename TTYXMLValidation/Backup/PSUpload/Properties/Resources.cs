// Decompiled with JetBrains decompiler
// Type: PSUpload.Properties.Resources
// Assembly: TTYXMLValidation, Version=2.0.6828.36913, Culture=neutral, PublicKeyToken=null
// MVID: FEB2971C-CF92-47B4-A936-B4033AC2F55F
// Assembly location: C:\Users\maniamar\Downloads\TTYXMLValidation.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace PSUpload.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (PSUpload.Properties.Resources.resourceMan == null)
          PSUpload.Properties.Resources.resourceMan = new ResourceManager("PSUpload.Properties.Resources", typeof (PSUpload.Properties.Resources).Assembly);
        return PSUpload.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => PSUpload.Properties.Resources.resourceCulture;
      set => PSUpload.Properties.Resources.resourceCulture = value;
    }
  }
}
