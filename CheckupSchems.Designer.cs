
namespace PNTN_prov
{
    partial class CheckupSchems
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
            this.schemesBox = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // schemesBox
            // 
            this.schemesBox.FormattingEnabled = true;
            this.schemesBox.Location = new System.Drawing.Point(12, 12);
            this.schemesBox.Name = "schemesBox";
            this.schemesBox.Size = new System.Drawing.Size(121, 21);
            this.schemesBox.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.InitialImage = global::PNTN_prov.Properties.Resources.общая_схема;
            this.pictureBox1.Location = new System.Drawing.Point(139, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(695, 539);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // CheckupSchems
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 565);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.schemesBox);
            this.MaximumSize = new System.Drawing.Size(863, 603);
            this.Name = "CheckupSchems";
            this.Text = "Схемы проверок";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CheckupSchems_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox schemesBox;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}