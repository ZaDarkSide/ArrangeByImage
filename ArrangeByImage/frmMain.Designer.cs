namespace ArrangeByImage
{
	partial class frmMain
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
            this.saveButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.arrangeButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.pbDisplay = new System.Windows.Forms.PictureBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.txtNumberOfIcons = new System.Windows.Forms.TextBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.chkEnableDrawMode = new System.Windows.Forms.CheckBox();
            this.cmdClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(7, 8);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(155, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "&Save current Icon Layout";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(181, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(155, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "&Restore saved icon layout";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // arrangeButton
            // 
            this.arrangeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.arrangeButton.Location = new System.Drawing.Point(10, 352);
            this.arrangeButton.Name = "arrangeButton";
            this.arrangeButton.Size = new System.Drawing.Size(155, 23);
            this.arrangeButton.TabIndex = 2;
            this.arrangeButton.Text = "Arrange by &Image";
            this.arrangeButton.UseVisualStyleBackColor = true;
            this.arrangeButton.Click += new System.EventHandler(this.arrangeButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(264, 352);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // pbDisplay
            // 
            this.pbDisplay.BackColor = System.Drawing.Color.White;
            this.pbDisplay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbDisplay.Location = new System.Drawing.Point(12, 107);
            this.pbDisplay.Name = "pbDisplay";
            this.pbDisplay.Size = new System.Drawing.Size(329, 228);
            this.pbDisplay.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbDisplay.TabIndex = 13;
            this.pbDisplay.TabStop = false;
            this.pbDisplay.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbDisplay_MouseMove);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(9, 81);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(88, 13);
            this.Label2.TabIndex = 12;
            this.Label2.Text = "Number of Icons:";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(43, 58);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(54, 13);
            this.lblFileName.TabIndex = 11;
            this.lblFileName.Text = "FileName:";
            // 
            // txtNumberOfIcons
            // 
            this.txtNumberOfIcons.Location = new System.Drawing.Point(103, 81);
            this.txtNumberOfIcons.Name = "txtNumberOfIcons";
            this.txtNumberOfIcons.Size = new System.Drawing.Size(108, 20);
            this.txtNumberOfIcons.TabIndex = 10;
            this.txtNumberOfIcons.Text = "40";
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(103, 55);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(135, 20);
            this.txtFileName.TabIndex = 8;
            this.txtFileName.Text = "smiley.bmp";
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(244, 52);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowse.TabIndex = 7;
            this.cmdBrowse.Text = "Browse";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.FileName = "OpenFileDialog1";
            this.OpenFileDialog1.Filter = "Bitmap Files | *.bmp";
            // 
            // chkEnableDrawMode
            // 
            this.chkEnableDrawMode.AutoSize = true;
            this.chkEnableDrawMode.Location = new System.Drawing.Point(224, 84);
            this.chkEnableDrawMode.Name = "chkEnableDrawMode";
            this.chkEnableDrawMode.Size = new System.Drawing.Size(117, 17);
            this.chkEnableDrawMode.TabIndex = 14;
            this.chkEnableDrawMode.Text = "Enable Draw Mode";
            this.chkEnableDrawMode.UseVisualStyleBackColor = true;
            this.chkEnableDrawMode.CheckedChanged += new System.EventHandler(this.chkEnableDrawMode_CheckedChanged);
            // 
            // cmdClear
            // 
            this.cmdClear.Location = new System.Drawing.Point(244, 53);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(75, 23);
            this.cmdClear.TabIndex = 15;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Visible = false;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(353, 391);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.chkEnableDrawMode);
            this.Controls.Add(this.pbDisplay);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.txtNumberOfIcons);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.arrangeButton);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.saveButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.Text = "Arrange";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button arrangeButton;
		private System.Windows.Forms.Button cancelButton;
        internal System.Windows.Forms.PictureBox pbDisplay;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label lblFileName;
        internal System.Windows.Forms.TextBox txtNumberOfIcons;
        internal System.Windows.Forms.TextBox txtFileName;
        internal System.Windows.Forms.Button cmdBrowse;
        internal System.Windows.Forms.OpenFileDialog OpenFileDialog1;
        private System.Windows.Forms.CheckBox chkEnableDrawMode;
        internal System.Windows.Forms.Button cmdClear;

	}
}

