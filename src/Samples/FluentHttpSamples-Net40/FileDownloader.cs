using System;
using System.Windows.Forms;
using FluentHttp;

namespace FluentHttpSamples
{
    public partial class FileDownloader : Form
    {
        public FileDownloader()
        {
            InitializeComponent();
        }

        private FluentHttpRequest _fluentHttpRequest;

        private void btnDownload_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var stream = sfd.OpenFile();

                _fluentHttpRequest = new FluentHttpRequest()
                    .BaseUrl(txtDownloadUrl.Text)
                    .FlushRequestReadStream(true)
                    .FlushRequestStream(true)
                    .FlushResponseStream(true)
                    .FlushResponseSaveStream(true)
                    .OnResponseHeadersReceived(
                        (o, args) =>
                        {
                            var notifier = new StreamNotifier(stream, args.Response.HttpWebResponse.ContentLength, args.AsyncState);
                            args.SaveResponseIn(notifier);
                            notifier.WriteProgressChanged += notifier_WriteProgressChanged;
                        });

                progressBar1.Value = 0;
                _fluentHttpRequest.ExecuteAsync(
                    (ar =>
                         {
                             stream.Dispose();

                             if (ar.Exception != null)
                             {
                                 MessageBox.Show(ar.Exception.Message);
                             }
                             else if (ar.IsCancelled)
                             {
                                 MessageBox.Show("Download cancelled");
                             }
                             else if (ar.IsCompleted)
                             {
                                 MessageBox.Show("Download completed.");
                             }

                             btnCancel.Enabled = false;
                             btnDownload.Enabled = true;
                         }));

                btnCancel.Enabled = true;
                btnDownload.Enabled = false;
            }
        }

        void notifier_WriteProgressChanged(object sender, WriteProgressChangedEventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(() => progressBar1.Value = e.ProgressPercentage));
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (_fluentHttpRequest != null)
            {
                _fluentHttpRequest.Cancel();
                btnCancel.Enabled = false;
                btnDownload.Enabled = true;
            }
        }
    }
}
