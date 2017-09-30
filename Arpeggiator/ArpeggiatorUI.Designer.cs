namespace Arpeggiator
{
    partial class ArpeggiatorUI
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
            this.NotesOn = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NotesOn
            // 
            this.NotesOn.AutoSize = true;
            this.NotesOn.Location = new System.Drawing.Point(45, 37);
            this.NotesOn.Name = "NotesOn";
            this.NotesOn.Size = new System.Drawing.Size(38, 13);
            this.NotesOn.TabIndex = 0;
            this.NotesOn.Text = "label1";
            // 
            // ArpeggiatorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 262);
            this.Controls.Add(this.NotesOn);
            this.Font = new System.Drawing.Font("Rockwell", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ArpeggiatorUI";
            this.Text = "ArpeggiatorUI";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NotesOn;
    }
}