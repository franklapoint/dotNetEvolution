namespace dotNetEvolution
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
			this.getHtmlButton = new System.Windows.Forms.Button();
			this.listBox = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// getHtmlButton
			// 
			this.getHtmlButton.Location = new System.Drawing.Point(13, 13);
			this.getHtmlButton.Name = "getHtmlButton";
			this.getHtmlButton.Size = new System.Drawing.Size(75, 23);
			this.getHtmlButton.TabIndex = 0;
			this.getHtmlButton.Text = "Get HTML...";
			this.getHtmlButton.UseVisualStyleBackColor = true;
			this.getHtmlButton.Click += new System.EventHandler(this.getHtmlButton_Click);
			// 
			// listBox
			// 
			this.listBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.listBox.FormattingEnabled = true;
			this.listBox.Location = new System.Drawing.Point(13, 43);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(259, 199);
			this.listBox.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.getHtmlButton);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button getHtmlButton;
		private System.Windows.Forms.ListBox listBox;
	}
}

