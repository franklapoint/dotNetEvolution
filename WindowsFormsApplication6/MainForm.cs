using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}
		byte[] inputBuffer = new byte[1024000];

		private void getHtmlButton_Click(object sender, EventArgs e)
		{
			var ui = TaskScheduler.FromCurrentSynchronizationContext();
			ThreadPool.QueueUserWorkItem(_ => StartRequest(ui));
			getHtmlButton.Enabled = false;
		}

		private void StartRequest(TaskScheduler ui)
		{
			HttpWebRequest webRequest =
				(HttpWebRequest) WebRequest.Create("http://google.ca");
			webRequest.
				BeginGetResponse(
					asyncResult =>
						{
							WebResponse response =
								webRequest.EndGetResponse(asyncResult);
							Stream stream = response.GetResponseStream();
							if (stream == null) return;
							Task<int> bytesRead =
								Task<int>.Factory.
									FromAsync(stream.BeginReadToEnd,
											  stream.EndReadToEnd,
											  inputBuffer, 0, inputBuffer.Length,
											  stream);

							bytesRead.ContinueWith(
								ar =>
									{
										Trace.WriteLine(
											string.Format("Read {0} bytes",
														  ar.Result));
										string text = Encoding.ASCII.
											GetString(inputBuffer, 0,
													  ar.Result);
										SetData(text);
										getHtmlButton.Enabled = true;
										((IDisposable)response).Dispose();
									}, ui);
						}, webRequest);
		}

		private void SetData(string html)
		{
			listBox.Items.Clear();
			if (String.IsNullOrEmpty(html)) return;

			foreach (var x in GetScriptBodies(html))
				listBox.Items.Add(x);
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
		}
	}
}
