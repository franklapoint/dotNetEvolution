using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication4
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void getHtmlButton_Click(object sender, EventArgs e)
		{
			byte[] inputBuffer = new byte[1024000];

			var webRequest =
				(HttpWebRequest) WebRequest.Create("http://google.ca");
			webRequest.
				BeginGetResponse(asyncResult =>
				                 {
				                 	WebResponse response = webRequest.EndGetResponse(asyncResult);
				                 	Stream stream = response.GetResponseStream();
				                 	if (stream == null) return;
				                 	stream.BeginReadToEnd(inputBuffer,
				                 	                      0,
				                 	                      inputBuffer.Length,
				                 	                      readAsyncResult =>
				                 	                      {
				                 	                      	int bytesRead = stream.EndReadToEnd(readAsyncResult);
				                 	                      	Trace.WriteLine(string.Format("Read {0} bytes", bytesRead));
				                 	                      	string text = Encoding.ASCII.GetString(inputBuffer, 0,
				                 	                      	                                       bytesRead);
				                 	                      	SetData(text);
				                 	                      	EnableButton();
				                 	                      }, stream);
				                 }, webRequest);
			getHtmlButton.Enabled = false;
		}

		private void SetData(string html)
		{
			if (String.IsNullOrEmpty(html)) return;

			if (InvokeRequired)
			{
				BeginInvoke((MethodInvoker) (() => SetData(html)));
				return;
			}

			listBox.Items.Clear();
			foreach (var x in GetScriptBodies(html))
				listBox.Items.Add(x);
		}

		private void EnableButton()
		{
			if (InvokeRequired)
			{
				BeginInvoke((MethodInvoker)EnableButton); // method group instead of lambda
				return;
			}
			getHtmlButton.Enabled = true;
		}

		private static IEnumerable<string> GetScriptBodies(string html)
		{
			const string pattern = @"<script.*?>\s*(?'scriptBody'.+?)\s*</script>";
			MatchCollection matches = Regex.Matches(html, pattern);
			if (matches.Count == 0) yield break;
			foreach (Match match in matches)
			{
				string scriptBody = match.Groups["scriptBody"].Value;
				yield return scriptBody;
			}
			//uint unsignedNumber = 1;
			//int signedNumber = 2;
			//var x = unsignedNumber*signedNumber;
			//var someType = new {X = 10, Y = 12};
		}
	}
}
