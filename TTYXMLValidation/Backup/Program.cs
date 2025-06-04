// Decompiled with JetBrains decompiler
// Type: TTYValidate.Program
// Assembly: TTYXMLValidation, Version=2.0.6828.36913, Culture=neutral, PublicKeyToken=null
// MVID: FEB2971C-CF92-47B4-A936-B4033AC2F55F
// Assembly location: C:\Users\maniamar\Downloads\TTYXMLValidation.exe

using System;
using System.Windows.Forms;

namespace TTYValidate
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      Application.Run((Form) new ProductionsiteUploadFrm());
    }
  }
}
