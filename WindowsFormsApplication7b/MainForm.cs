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

namespace WindowsFormsApplication7b
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public MainForm()
        {
            InitializeComponent();
        }

        private class MyProgress : Progress<int>
        {
            public void Progress(int value)
            {
                OnReport(value);
            }
        }

        private async void getHtmlButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            getHtmlButton.Enabled = false;
            progressBar.Value = 0;
            progressBar.Visible = true;
            cancelButton.Enabled = true;
            cancelButton.Visible = true;

            HttpWebRequest webRequest = await StartRequestAsync();

            byte[] inputBuffer;
            int index = 0;
            using (WebResponse response = await webRequest.GetResponseAsync())
            {
                inputBuffer = new byte[response.ContentLength];

                Stream stream = response.GetResponseStream();

                MyProgress progress = new MyProgress();
                progress.ProgressChanged += progress_ProgressChanged;

                int bytesRead;
                do
                {
                    try
                    {
                        bytesRead = await stream.
                                              ReadAsync(inputBuffer, index,
                                                        inputBuffer.Length - index,
                                                        cancellationToken);
                        if (cancellationToken.IsCancellationRequested) break;
                        index += bytesRead;
                        progress.Progress((index*100)/inputBuffer.Length);
                    }
                    catch (TaskCanceledException)
                    {
                        break;
                    }
                } while (bytesRead != 0);
            }

            listBox.Items.Clear();
            if (!cancellationToken.IsCancellationRequested)
            {
                Trace.WriteLine(String.Format("Read {0} bytes", index));
                string html = Encoding.ASCII.GetString(inputBuffer, 0, index);
                if (!String.IsNullOrEmpty(html))
                {
                    foreach (var x in GetScriptBodies(html))
                        listBox.Items.Add(x);
                }
            }
            else
            {
                listBox.Items.Add("CANCELLED");
                progressBar.Visible = false;
            }
            getHtmlButton.Enabled = true;
            cancelButton.Enabled = false;
            cancelButton.Visible = false;
        }

        void progress_ProgressChanged(object sender, int e)
        {
            progressBar.Value = e;
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

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }
    }
}
