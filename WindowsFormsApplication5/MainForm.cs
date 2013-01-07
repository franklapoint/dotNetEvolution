using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void getHtmlButton_Click(object sender, EventArgs e)
		{
		    ThreadPool.QueueUserWorkItem(_ => StartRequest());
		    getHtmlButton.Enabled = false;
		}

		private void StartRequest()
		{
			byte[] inputBuffer = new byte[1024000];

			var webRequest =
				(HttpWebRequest) WebRequest.Create("http://google.ca");
			webRequest.
				BeginGetResponse(
					asyncResult =>
						{
							WebResponse response =
								webRequest.EndGetResponse(asyncResult);
							Stream stream = response.GetResponseStream();
							if (stream == null) return;
							stream.
								BeginReadToEnd(inputBuffer,
								               0,
								               inputBuffer.Length,
								               readAsyncResult =>
									               {
										               int bytesRead = stream.
											               EndReadToEnd(readAsyncResult);
										               Trace.WriteLine(
											               string.Format("Read {0} bytes",
											                             bytesRead));
										               string text = Encoding.ASCII.
											               GetString(inputBuffer, 0,
											                         bytesRead);
										               SetData(text);
										               EnableButton();
													   ((IDisposable)response).
														   Dispose();
									               }
								               , stream);
						}, webRequest);
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
				BeginInvoke((MethodInvoker)EnableButton);
				return;
			}
			getHtmlButton.Enabled = true;
		}

		private static IEnumerable<string> GetScriptBodies(string html)
		{
			const string pattern = @"<script.*?>\s*(?'scriptBody'.+?)\s*</script>";
			MatchCollection matches = Regex.Matches(html, pattern);
			if (matches.Count == 0) yield break;
			foreach (string scriptBody in 
				from Match match in matches select match.Groups["scriptBody"].Value)
			{
				yield return scriptBody;
			}
		}
	}
}
