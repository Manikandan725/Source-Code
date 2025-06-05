namespace TTYWebServiceConversion
{
    partial class TTYWebServiceConvForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TTYWebServiceConvForm));
            this.SuspendLayout();
            // 
            // lvwList
            // 
            this.lvwList.Size = new System.Drawing.Size(767, 239);
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "CustomerDCN";
            // 
            // tabControl1
            // 
            this.tabControl1.Size = new System.Drawing.Size(781, 271);
            // 
            // TTYWebServiceConvForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 344);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TTYWebService";
            this.Text = "TTYWebService";
            this.Load += new System.EventHandler(this.FrmCignaFileConvLoad);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}