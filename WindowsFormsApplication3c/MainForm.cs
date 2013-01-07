using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication3c
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private HttpWebRequest webRequest;

		private void getHtmlButton_Click(object sender, EventArgs e)
		{
			listBox.Items.Clear();

			backgroundWorker.RunWorkerAsync();

			getHtmlButton.Enabled = false;
			progressBar.Value = 0;
			progressBar.Visible = true;
			cancelButton.Visible = true;
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			int progress = 1;
			var backgroundWorker = (BackgroundWorker) sender;
			backgroundWorker.ReportProgress(progress);
			webRequest =
				(HttpWebRequest)WebRequest.Create("http://www.gutenberg.org/files/1497/1497.txt");
			try
			{
				var response = webRequest.GetResponse();
				var stream = response.GetResponseStream();
				byte[] inputBuffer = new byte[1024000];
				var asyncResult = StreamHelper.
					BeginReadStreamToEnd(stream, inputBuffer, 0, inputBuffer.Length,
										 ar =>
											 {
												 int bytesRead = StreamHelper.EndReadStreamToEnd(ar);
												 Array.Resize(ref inputBuffer, bytesRead);
												 ((IDisposable) response).Dispose();
											 }, stream);

				while (false == asyncResult.AsyncWaitHandle.WaitOne(200))
				{
					backgroundWorker.ReportProgress(progress);
					progress += (100 - progress)/2;
					if (!backgroundWorker.CancellationPending) continue;
					e.Cancel = true;
					return;
				}
				backgroundWorker.ReportProgress(100);
				e.Result = Encoding.ASCII.GetString(inputBuffer, 0, inputBuffer.Length);
			}
			catch (WebException ex)
			{
				if (ex.Status != WebExceptionStatus.RequestCanceled) throw;
				e.Cancel = true;
			}
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			progressBar.Value = e.ProgressPercentage;
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			getHtmlButton.Enabled = true;
			cancelButton.Visible = false;
			webRequest = null;
			if (e.Cancelled)
			{
				MessageBox.Show("Cancelled");
				progressBar.Visible = false;
			}
			else
			{
				foreach (string x in GetScriptBodies((string) e.Result))
					listBox.Items.Add(x);
			}
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			var request = webRequest;
			if (request == null) return;
			backgroundWorker.CancelAsync();
			request.Abort();
		}

		private static IEnumerable<string> GetScriptBodies(string html)
		{
			const string pattern = @"<script.*?>\s*(?'scriptBody'.+?)\s*</script>";
			if (html != null)
			{
				MatchCollection matches = Regex.Matches(html, pattern);
				if (matches.Count == 0) yield break;
				foreach (Match match in matches)
				{
					string scriptBody = match.Groups["scriptBody"].Value;
					yield return scriptBody;
				}
			}
			yield break;
		}
	}
}
