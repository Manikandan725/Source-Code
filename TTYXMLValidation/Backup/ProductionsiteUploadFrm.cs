// Decompiled with JetBrains decompiler
// Type: TTYValidate.ProductionsiteUploadFrm
// Assembly: TTYXMLValidation, Version=2.0.6828.36913, Culture=neutral, PublicKeyToken=null
// MVID: FEB2971C-CF92-47B4-A936-B4033AC2F55F
// Assembly location: C:\Users\maniamar\Downloads\TTYXMLValidation.exe

using Ethos.Core;
using Generic.Util;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace TTYValidate
{
  public class ProductionsiteUploadFrm : EthosProcessFormBase
  {
    private TTYXMLValidation _TTYXMLValidation;
    private IContainer components;

    public ProductionsiteUploadFrm() => this.InitializeComponent();

    private void ProductionsiteUploadFrm_Load(object sender, EventArgs e)
    {
      try
      {
        this._TTYXMLValidation = new TTYXMLValidation();
        this.EthosProcess = (IEthosProcess) this._TTYXMLValidation;
        this._TTYXMLValidation.StatusUpdate += new EventHandler<ProcessEventArgs<DataRow, DataRow, object>>(this.BatchStatusUpdate);
      }
      catch (Exception ex)
      {
        int num = (int) MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        this.Close();
      }
    }

    private void BatchStatusUpdate(object sender, ProcessEventArgs<DataRow, DataRow, object> e)
    {
      try
      {
        if (this.lvwList.InvokeRequired)
        {
          this.BeginInvoke((Delegate) new EventHandler<ProcessEventArgs<DataRow, DataRow, object>>(this.BatchStatusUpdate), sender, (object) e);
          Application.DoEvents();
        }
        else
        {
          DataRow level1Data = e.Level1Data;
          DataRow level2Data = e.Level2Data;
          string str1 = Convert.ToString(level1Data["CustName"]);
          string str2 = Convert.ToString(level1Data["ProjName"]);
          string text = Convert.ToString(level2Data["Dcn"]);
          ListViewItem listViewItem = (ListViewItem) null;
          if (this.lvwList.Items.Count == 100)
            this.lvwList.Items[0].Remove();
          if (this.lvwList.Items.Count > 0)
            listViewItem = this.lvwList.FindItemWithText(text, true, 0);
          if (listViewItem != null)
          {
            listViewItem.SubItems[2].Text = str2;
            listViewItem.SubItems[3].Text = DateTime.Now.ToString("MM/dd/yy HH:mm:ss");
            listViewItem.SubItems[5].Text = e.Status;
            listViewItem.SubItems[6].Text = e.ErrorDescription;
          }
          else
          {
            string[] items = new string[7];
            items[0] = Convert.ToString(++this.ListIndex);
            items[1] = str1;
            items[2] = str2;
            items[3] = DateTime.Now.ToString("MM/dd/yy HH:mm:ss");
            items[4] = text;
            items[5] = e.Status;
            items[6] = e.ErrorDescription;
            listViewItem = new ListViewItem(items)
            {
              UseItemStyleForSubItems = false
            };
            this.lvwList.Items.Add(listViewItem);
            listViewItem.EnsureVisible();
          }
          listViewItem.SubItems[5].ForeColor = !(e.Status.ToUpper() == "COMPLETED") ? (!(e.Status.ToUpper() == "WIP") ? Color.DarkRed : Color.Blue) : Color.DarkGreen;
        }
      }
      catch (Exception ex)
      {
        LogFile.WriteErrorLog("TTYXMLValidation.cs", nameof (BatchStatusUpdate), ex.Message);
      }
    }

    protected new virtual void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.tabControl1.SuspendLayout();
      this.tabMain.SuspendLayout();
      this.SuspendLayout();
      this.lvwList.Location = new Point(4, 5);
      this.lvwList.Size = new Size(1062, 492);
      this.tabControl1.Location = new Point(0, 74);
      this.tabControl1.Size = new Size(1078, 536);
      this.tabMain.Location = new Point(4, 30);
      this.tabMain.Margin = new Padding(4, 5, 4, 5);
      this.tabMain.Padding = new Padding(4, 5, 4, 5);
      this.tabMain.Size = new Size(1070, 502);
      this.AutoScaleDimensions = new SizeF(9f, 21f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(1078, 646);
      this.Margin = new Padding(4, 5, 4, 5);
      this.Name = nameof (ProductionsiteUploadFrm);
      this.Text = "TTYXMLValidation";
      this.Load += new EventHandler(this.ProductionsiteUploadFrm_Load);
      this.tabControl1.ResumeLayout(false);
      this.tabMain.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
