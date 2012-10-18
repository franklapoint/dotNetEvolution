using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
	public class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void getHtmlButton_Click(object sender, EventArgs e)
		{
			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.
				Create("http://google.ca");
			webRequest.BeginGetResponse(GetResponseCallback, webRequest);
			getHtmlButton.Enabled = false;
		}

		private ListBox listBox;

		private readonly byte[] finalBuffer = new byte[1024000];

		private void GetResponseCallback(IAsyncResult asyncResult)
		{
			HttpWebRequest webRequest = (HttpWebRequest) asyncResult.AsyncState;
			WebResponse response = webRequest.EndGetResponse(asyncResult);
			Stream stream = response.GetResponseStream();
			if (stream == null) return;
			StreamHelper.BeginReadStreamToEnd(stream, finalBuffer, 0, 
				finalBuffer.Length, ReadToEndCallback, stream);
		}

		private void ReadToEndCallback(IAsyncResult asyncResult)
		{
			int bytesRead = StreamHelper.EndReadStreamToEnd(asyncResult);
			Trace.WriteLine(string.Format("Read {0} bytes", bytesRead));
			string text = Encoding.ASCII.GetString(finalBuffer, 0, bytesRead);
			SetData(text);
			EnableButton();
		}

		private delegate void SetHtmlDelegate(string html);

		private void SetData(string html)
		{
			if (String.IsNullOrEmpty(html)) return;

			if (InvokeRequired)
			{
				BeginInvoke(new SetHtmlDelegate(SetData), html);
				return;
			}

			listBox.Items.Clear();
			foreach (string x in GetScriptBodies(html))
				listBox.Items.Add(x);
		}

		private static IEnumerable GetScriptBodies(string html)
		{
			const string pattern = @"<script.*?>\s*(?'scriptBody'.+?)\s*</script>";
			MatchCollection matches = Regex.Matches(html, pattern);
			string[] result = new string[matches.Count];
			if (matches.Count == 0) return result;
			int i = 0;
			foreach (Match match in matches)
			{
				string scriptBody = match.Groups["scriptBody"].Value;
				result[i++] = scriptBody;
			}
			return result;
		}

		private void EnableButton()
		{
			if (InvokeRequired)
			{
				BeginInvoke(new MethodInvoker(EnableButton));
				return;
			}
			getHtmlButton.Enabled = true;
		}

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
			this.getHtmlButton.Location = new System.Drawing.Point(12, 12);
			this.getHtmlButton.Name = "getHtmlButton";
			this.getHtmlButton.Size = new System.Drawing.Size(106, 23);
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
			this.listBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.listBox.FormattingEnabled = true;
			this.listBox.ItemHeight = 20;
			this.listBox.Location = new System.Drawing.Point(12, 42);
			this.listBox.Name = "listBox";
			this.listBox.Size = new System.Drawing.Size(258, 184);
			this.listBox.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(282, 255);
			this.Controls.Add(this.listBox);
			this.Controls.Add(this.getHtmlButton);
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private Button getHtmlButton;

		private void MainForm_Load(object sender, EventArgs e)
		{

		}
	}
}
