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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArpeggiatorUI));
            this.labelNotesOn = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxDirection = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxOctaves = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.comboBoxRythm1 = new System.Windows.Forms.ComboBox();
            this.comboBoxRythm2 = new System.Windows.Forms.ComboBox();
            this.comboBoxRythm3 = new System.Windows.Forms.ComboBox();
            this.comboBoxRythm4 = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox5 = new System.Windows.Forms.ComboBox();
            this.comboBox6 = new System.Windows.Forms.ComboBox();
            this.comboBox7 = new System.Windows.Forms.ComboBox();
            this.comboBox8 = new System.Windows.Forms.ComboBox();
            this.checkBoxSwing = new System.Windows.Forms.CheckBox();
            this.comboBoxProbability = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // labelNotesOn
            // 
            this.labelNotesOn.AutoSize = true;
            this.labelNotesOn.BackColor = System.Drawing.Color.Transparent;
            this.labelNotesOn.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNotesOn.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.labelNotesOn.Location = new System.Drawing.Point(117, 217);
            this.labelNotesOn.Name = "labelNotesOn";
            this.labelNotesOn.Size = new System.Drawing.Size(34, 14);
            this.labelNotesOn.TabIndex = 0;
            this.labelNotesOn.Text = "none";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label1.Location = new System.Drawing.Point(48, 217);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "Notes on:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label2.Location = new System.Drawing.Point(48, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Direction ";
            // 
            // comboBoxDirection
            // 
            this.comboBoxDirection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDirection.FormattingEnabled = true;
            this.comboBoxDirection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.comboBoxDirection.Location = new System.Drawing.Point(120, 63);
            this.comboBoxDirection.Name = "comboBoxDirection";
            this.comboBoxDirection.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDirection.TabIndex = 2;
            this.comboBoxDirection.SelectionChangeCommitted += new System.EventHandler(this.comboBoxDirection_SelectionChangeCommitted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label3.Location = new System.Drawing.Point(48, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "Octaves";
            // 
            // comboBoxOctaves
            // 
            this.comboBoxOctaves.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxOctaves.FormattingEnabled = true;
            this.comboBoxOctaves.Location = new System.Drawing.Point(120, 95);
            this.comboBoxOctaves.Name = "comboBoxOctaves";
            this.comboBoxOctaves.Size = new System.Drawing.Size(55, 21);
            this.comboBoxOctaves.TabIndex = 5;
            this.comboBoxOctaves.SelectionChangeCommitted += new System.EventHandler(this.comboBoxOctaves_SelectionChangeCommitted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label4.Location = new System.Drawing.Point(48, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "Rythm";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "quarter.png");
            this.imageList1.Images.SetKeyName(1, "eight_eight.png");
            this.imageList1.Images.SetKeyName(2, "rest.png");
            this.imageList1.Images.SetKeyName(3, "eight_rest.png");
            this.imageList1.Images.SetKeyName(4, "rest_eight.png");
            this.imageList1.Images.SetKeyName(5, "triplet.png");
            // 
            // comboBoxRythm1
            // 
            this.comboBoxRythm1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRythm1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRythm1.FormattingEnabled = true;
            this.comboBoxRythm1.ItemHeight = 25;
            this.comboBoxRythm1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.comboBoxRythm1.Location = new System.Drawing.Point(120, 128);
            this.comboBoxRythm1.Name = "comboBoxRythm1";
            this.comboBoxRythm1.Size = new System.Drawing.Size(55, 31);
            this.comboBoxRythm1.TabIndex = 8;
            this.comboBoxRythm1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox1_DrawItem_1);
            this.comboBoxRythm1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // comboBoxRythm2
            // 
            this.comboBoxRythm2.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRythm2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRythm2.FormattingEnabled = true;
            this.comboBoxRythm2.ItemHeight = 25;
            this.comboBoxRythm2.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.comboBoxRythm2.Location = new System.Drawing.Point(181, 128);
            this.comboBoxRythm2.Name = "comboBoxRythm2";
            this.comboBoxRythm2.Size = new System.Drawing.Size(55, 31);
            this.comboBoxRythm2.TabIndex = 9;
            this.comboBoxRythm2.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox2_DrawItem);
            this.comboBoxRythm2.SelectionChangeCommitted += new System.EventHandler(this.comboBox2_SelectionChangeCommitted);
            // 
            // comboBoxRythm3
            // 
            this.comboBoxRythm3.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRythm3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRythm3.FormattingEnabled = true;
            this.comboBoxRythm3.ItemHeight = 25;
            this.comboBoxRythm3.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.comboBoxRythm3.Location = new System.Drawing.Point(242, 128);
            this.comboBoxRythm3.Name = "comboBoxRythm3";
            this.comboBoxRythm3.Size = new System.Drawing.Size(55, 31);
            this.comboBoxRythm3.TabIndex = 10;
            this.comboBoxRythm3.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox3_DrawItem);
            this.comboBoxRythm3.SelectionChangeCommitted += new System.EventHandler(this.comboBox3_SelectionChangeCommitted);
            // 
            // comboBoxRythm4
            // 
            this.comboBoxRythm4.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.comboBoxRythm4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRythm4.FormattingEnabled = true;
            this.comboBoxRythm4.ItemHeight = 25;
            this.comboBoxRythm4.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.comboBoxRythm4.Location = new System.Drawing.Point(303, 128);
            this.comboBoxRythm4.Name = "comboBoxRythm4";
            this.comboBoxRythm4.Size = new System.Drawing.Size(55, 31);
            this.comboBoxRythm4.TabIndex = 11;
            this.comboBoxRythm4.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.comboBox4_DrawItem);
            this.comboBoxRythm4.SelectionChangeCommitted += new System.EventHandler(this.comboBox4_SelectionChangeCommitted);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::Arpeggiator.Properties.Resources.coollogo_com_28574929;
            this.pictureBox1.Location = new System.Drawing.Point(25, -3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(233, 60);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::Arpeggiator.Properties.Resources.coollogo_com_66841752;
            this.pictureBox2.Location = new System.Drawing.Point(253, 28);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(149, 29);
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.Location = new System.Drawing.Point(0, 0);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // comboBox2
            // 
            this.comboBox2.Location = new System.Drawing.Point(0, 0);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 0;
            // 
            // comboBox3
            // 
            this.comboBox3.Location = new System.Drawing.Point(0, 0);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(121, 21);
            this.comboBox3.TabIndex = 0;
            // 
            // comboBox4
            // 
            this.comboBox4.Location = new System.Drawing.Point(0, 0);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(121, 21);
            this.comboBox4.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label5.Location = new System.Drawing.Point(48, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 14);
            this.label5.TabIndex = 14;
            this.label5.Text = "Accent";
            // 
            // comboBox5
            // 
            this.comboBox5.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox5.FormattingEnabled = true;
            this.comboBox5.Items.AddRange(new object[] {
            "none",
            "downbeat",
            "upbeat"});
            this.comboBox5.Location = new System.Drawing.Point(120, 175);
            this.comboBox5.Name = "comboBox5";
            this.comboBox5.Size = new System.Drawing.Size(55, 21);
            this.comboBox5.TabIndex = 15;
            this.comboBox5.SelectionChangeCommitted += new System.EventHandler(this.comboBox5_SelectionChangeCommitted);
            // 
            // comboBox6
            // 
            this.comboBox6.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox6.FormattingEnabled = true;
            this.comboBox6.Items.AddRange(new object[] {
            "none",
            "downbeat",
            "upbeat"});
            this.comboBox6.Location = new System.Drawing.Point(181, 175);
            this.comboBox6.Name = "comboBox6";
            this.comboBox6.Size = new System.Drawing.Size(55, 21);
            this.comboBox6.TabIndex = 16;
            this.comboBox6.SelectionChangeCommitted += new System.EventHandler(this.comboBox6_SelectionChangeCommitted);
            // 
            // comboBox7
            // 
            this.comboBox7.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox7.FormattingEnabled = true;
            this.comboBox7.Items.AddRange(new object[] {
            "none",
            "downbeat",
            "upbeat"});
            this.comboBox7.Location = new System.Drawing.Point(242, 175);
            this.comboBox7.Name = "comboBox7";
            this.comboBox7.Size = new System.Drawing.Size(55, 21);
            this.comboBox7.TabIndex = 17;
            this.comboBox7.SelectionChangeCommitted += new System.EventHandler(this.comboBox7_SelectionChangeCommitted);
            // 
            // comboBox8
            // 
            this.comboBox8.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox8.FormattingEnabled = true;
            this.comboBox8.Items.AddRange(new object[] {
            "none",
            "downbeat",
            "upbeat"});
            this.comboBox8.Location = new System.Drawing.Point(303, 175);
            this.comboBox8.Name = "comboBox8";
            this.comboBox8.Size = new System.Drawing.Size(55, 21);
            this.comboBox8.TabIndex = 18;
            this.comboBox8.SelectionChangeCommitted += new System.EventHandler(this.comboBox8_SelectionChangeCommitted);
            // 
            // checkBoxSwing
            // 
            this.checkBoxSwing.AutoSize = true;
            this.checkBoxSwing.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxSwing.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxSwing.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxSwing.ForeColor = System.Drawing.Color.White;
            this.checkBoxSwing.Location = new System.Drawing.Point(300, 66);
            this.checkBoxSwing.Name = "checkBoxSwing";
            this.checkBoxSwing.Size = new System.Drawing.Size(58, 18);
            this.checkBoxSwing.TabIndex = 19;
            this.checkBoxSwing.Text = "Swing";
            this.checkBoxSwing.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.checkBoxSwing.UseVisualStyleBackColor = false;
            this.checkBoxSwing.CheckedChanged += new System.EventHandler(this.checkBoxSwing_CheckedChanged);
            // 
            // comboBoxProbability
            // 
            this.comboBoxProbability.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProbability.FormattingEnabled = true;
            this.comboBoxProbability.Items.AddRange(new object[] {
            "100",
            "90",
            "80",
            "70",
            "60",
            "50"});
            this.comboBoxProbability.Location = new System.Drawing.Point(310, 96);
            this.comboBoxProbability.Name = "comboBoxProbability";
            this.comboBoxProbability.Size = new System.Drawing.Size(48, 21);
            this.comboBoxProbability.TabIndex = 20;
            this.comboBoxProbability.SelectionChangeCommitted += new System.EventHandler(this.comboBoxProbability_SelectionChangeCommitted);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label6.Location = new System.Drawing.Point(204, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 14);
            this.label6.TabIndex = 21;
            this.label6.Text = "Note probability";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft JhengHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.label7.Location = new System.Drawing.Point(364, 99);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 14);
            this.label7.TabIndex = 22;
            this.label7.Text = "%";
            // 
            // ArpeggiatorUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Arpeggiator.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(417, 245);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxProbability);
            this.Controls.Add(this.checkBoxSwing);
            this.Controls.Add(this.comboBox8);
            this.Controls.Add(this.comboBox7);
            this.Controls.Add(this.comboBox6);
            this.Controls.Add(this.comboBox5);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.comboBoxRythm4);
            this.Controls.Add(this.comboBoxRythm3);
            this.Controls.Add(this.comboBoxRythm2);
            this.Controls.Add(this.comboBoxRythm1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxOctaves);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.comboBoxDirection);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelNotesOn);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ArpeggiatorUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "ArpeggiatorUI";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNotesOn;
        private Label label1;
        private Label label2;
        private ComboBox comboBoxDirection;
        private ComboBox comboBoxOctaves;
        private Label label3;
        private Label label4;
        private ImageList imageList1;
        private ComboBox comboBoxRythm1;
        private ComboBox comboBoxRythm2;
        private ComboBox comboBoxRythm3;
        private ComboBox comboBoxRythm4;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private Label label5;
        private ComboBox comboBox5;
        private ComboBox comboBox6;
        private ComboBox comboBox7;
        private ComboBox comboBox8;
        private CheckBox checkBoxSwing;
        private ComboBox comboBoxProbability;
        private Label label6;
        private Label label7;
    }
}