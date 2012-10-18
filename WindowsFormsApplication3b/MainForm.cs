using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WindowsFormsApplication3b
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private WebClient webClient;

		private void getHtmlButton_Click(object sender, EventArgs e)
		{
			listBox.Items.Clear();
			webClient = new WebClient();
			webClient.DownloadProgressChanged += (o, args) =>
			                                  {
			                                  	progressBar.Value = args.ProgressPercentage;
			                                  };
			webClient.
				DownloadStringCompleted +=
				(o, args) =>
				{
					getHtmlButton.Enabled = true;
					cancelButton.Visible = false;
					webClient.Dispose();
					webClient = null;
					if (!args.Cancelled)
					{
						foreach (string x in GetScriptBodies(args.Result))
							listBox.Items.Add(x);
					}
					else
					{
						MessageBox.Show("Cancelled");
						progressBar.Visible = false;
					}
				};
			progressBar.Visible = true;
			cancelButton.Visible = true;
			progressBar.Value = 0;
			webClient.DownloadStringAsync(new Uri("http://google.ca"));

			getHtmlButton.Enabled = false;
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			if(webClient != null) webClient.CancelAsync();
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
			yield break;
		}
	}
}
