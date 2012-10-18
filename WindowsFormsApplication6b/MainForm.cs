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

namespace WindowsFormsApplication6b
{
    public partial class MainForm : Form
    {
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public MainForm()
        {
            InitializeComponent();
        }

        private void getHtmlButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();
            cancellationToken = cancellationTokenSource.Token;

            var ui = TaskScheduler.FromCurrentSynchronizationContext();
            ThreadPool.QueueUserWorkItem(_ => StartRequest(ui));

            getHtmlButton.Enabled = false;
            progressBar.Enabled = true;
            progressBar.Visible = true;
            cancelButton.Enabled = true;
            cancelButton.Visible = true;
        }

        private void StartRequest(TaskScheduler ui)
        {
            byte[] inputBuffer = new byte[1024000];

            HttpWebRequest webRequest =
                (HttpWebRequest)
                WebRequest.Create("http://en.wikipedia.org/wiki/Line_of_succession_to_the_British_throne");
            webRequest.
                BeginGetResponse(asyncResult =>
                                     {
                                         WebResponse response = webRequest.EndGetResponse(asyncResult);
                                         Stream stream = response.GetResponseStream();
                                         if (stream == null) return;
                                         Task<int> bytesRead =
                                             Task<int>.Factory.
                                                 FromAsync(stream.BeginReadToEnd,
                                                           stream.EndReadToEnd,
                                                           inputBuffer, 0, inputBuffer.Length,
                                                           stream);
                                         if (this.InvokeRequired) BeginInvoke((MethodInvoker) (() => progressBar.Value = 50));
                                         bytesRead.
                                             ContinueWith(ar =>
                                                              {
                                                                  progressBar.Value = 100;
                                                                  if (!cancellationToken.IsCancellationRequested)
                                                                  {
                                                                      Trace.WriteLine(
                                                                          string.Format("Read {0} bytes",
                                                                                        ar.Result));
                                                                      string text =
                                                                          Encoding.ASCII.GetString(
                                                                              inputBuffer, 0, ar.Result);
                                                                      SetData(text);
                                                                  }
                                                                  getHtmlButton.Enabled = true;
                                                                  progressBar.Enabled = true;
                                                                  progressBar.Visible = true;
                                                                  cancelButton.Enabled = false;
                                                                  cancelButton.Visible = false;
                                                              }, ui);
                                     }, webRequest);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
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
