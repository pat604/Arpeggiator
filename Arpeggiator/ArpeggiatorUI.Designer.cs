using System.Windows.Forms;

namespace Arpeggiator
{
    partial class ArpeggiatorUI
    {

        // Required designer variable.
        private System.ComponentModel.IContainer components = null;

      
        // Clean up any resources being used.
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
            this.labelNotesOn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelNotesOn
            // 
            this.labelNotesOn.AutoSize = true;
            this.labelNotesOn.Location = new System.Drawing.Point(45, 37);
            this.labelNotesOn.Name = "labelNotesOn";
            this.labelNotesOn.Size = new System.Drawing.Size(35, 13);
            this.labelNotesOn.TabIndex = 0;
            this.labelNotesOn.Text = "label1";
            // 
            // ArpeggiatorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 262);
            this.Controls.Add(this.labelNotesOn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ArpeggiatorUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ArpeggiatorUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNotesOn;
    }
}