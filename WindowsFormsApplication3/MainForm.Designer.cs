namespace WindowsFormsApplication3
{
	partial class MainForm
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
			this.listBox = new System.Windows.Forms.ListBox();
			this.getHtmlButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// listBox
			// 
			this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox.FormattingEnabled = true;
			this.listBox.ItemHeight = 17;
			this.listBox.Location = new System.Drawing.Point(9, 41);
			this.listBox.Margin = new System.Windows.Forms.Padding(2);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(194, 140);
			this.listBox.TabIndex = 5;
			// 
			// getHtmlButton
			// 
			this.getHtmlButton.Location = new System.Drawing.Point(9, 16);
			this.getHtmlButton.Margin = new System.Windows.Forms.Padding(2);
			this.getHtmlButton.Name = "getHtmlButton";
			this.getHtmlButton.Size = new System.Drawing.Size(56, 19);
			this.getHtmlButton.TabIndex = 4;
			this.getHtmlButton.Text = "Get HTML...";
			this.getHtmlButton.UseVisualStyleBackColor = true;
			this.getHtmlButton.Click += new System.EventHandler(this.getHtmlButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(212, 207);
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.getHtmlButton);
			this.Margin = new System.Windows.Forms.Padding(2);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox listBox;
		private System.Windows.Forms.Button getHtmlButton;
	}
}

