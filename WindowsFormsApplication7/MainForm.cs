using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dotNetEvolution
{
	public partial class MainForm : Form
	{
		private async void getHtmlButton_Click(object sender, EventArgs e)
		{
			byte[] inputBuffer = new byte[1024000];

			getHtmlButton.Enabled = false;
			var webRequest = await StartRequestAsync();
			int index = 0;
			using (WebResponse response = await webRequest.GetResponseAsync())
			{
				Stream stream = response.GetResponseStream();

				int bytesRead;
				do
				{
					bytesRead = await stream.
						ReadAsync(inputBuffer, index, inputBuffer.Length - index);
					index += bytesRead;
				} while (bytesRead != 0);
			}

			Trace.WriteLine(String.Format("Read {0} bytes", index));
			string html = Encoding.ASCII.GetString(inputBuffer, 0, index);
			listBox.Items.Clear();
			if (!String.IsNullOrEmpty(html))
			{
				foreach (var x in GetScriptBodies(html))
					listBox.Items.Add(x);
			}
			getHtmlButton.Enabled = true;
		}

		private static Task<HttpWebRequest> StartRequestAsync()
		{
			return
				Task.Factory.
					StartNew(
						() =>
						(HttpWebRequest)
						WebRequest.Create("http://en.wikipedia.org/wiki/Line_of_succession_to_the_British_throne"));
		}

		private static IEnumerable<string> GetScriptBodies(string html)
		{
			const string pattern = @"<script.*?>\s*(?'scriptBody'.+?)\s*</script>";
			MatchCollection matches = Regex.Matches(html, pattern);
			foreach (Match match in matches)
			{
				string scriptBody = match.Groups["scriptBody"].Value;
				yield return scriptBody;
			}
			yield break;
		}

		public MainForm()
		{
			InitializeComponent();
		}
	}
}
