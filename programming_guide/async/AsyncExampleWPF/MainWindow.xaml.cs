using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Security.Policy;

namespace AsyncExampleWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            resultsTextBox.Clear();
            SumPageSizes();
            resultsTextBox.Text += "\r\nControl returned to startbutton_Click.";
        }

        private List<string> SetUpURLList()
        {
            var urls = new List<string>
            {
                "https://msdn.microsoft.com/library/windows/apps/br211380.aspx",
                "https://msdn.microsoft.com",
                "https://msdn.microsoft.com/library/hh290136.aspx",
                "https://msdn.microsoft.com/library/ee256749.aspx",
                "https://msdn.microsoft.com/library/hh290138.aspx",
                "https://msdn.microsoft.com/library/hh290140.aspx",
                "https://msdn.microsoft.com/library/dd470362.aspx",
                "https://msdn.microsoft.com/library/aa578028.aspx",
                "https://msdn.microsoft.com/library/ms404677.aspx",
                "https://msdn.microsoft.com/library/ff730837.aspx"
            };
            return urls;
        }

        private byte[] getUrlContents(string url)
        {
            var content = new MemoryStream();

            // Initialize a WebRequest connection.
            var webReq =(HttpWebRequest)WebRequest.Create(url);

            // Add User-Agent header to avoid 403 forbidden errors
            webReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";
            try
            {
                using (WebResponse response = webReq.GetResponse())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        responseStream.CopyTo(content);
                    }
                }
            }
            catch (WebException e)
            {
                using (WebResponse response = e.Response)
                {
                    HttpWebResponse httpResponse = (HttpWebResponse)response;
                    if (httpResponse != null)
                    {
                        resultsTextBox.Text += $"\r\nError code {httpResponse.StatusCode}\r\n";
                        using (Stream responseStream = httpResponse.GetResponseStream())
                        {
                            responseStream.CopyTo(content);
                        }
                    }
                    else
                    {
                        resultsTextBox.Text += $"\r\nError: {e.Message}\r\n";
                    }

                }
            }


            return content.ToArray();
        }

        private void SumPageSizes()
        {
            List<string> urlList = SetUpURLList();

            var total = 0;
            foreach (var url in urlList)
            {
                byte[] urlContents = getUrlContents(url);
                displayResults(url, urlContents);
                total += urlContents.Length;
            }

            resultsTextBox.Text += $"\r\n\r\nTotal bytes returned: {total} \r\n";
        }

        private void displayResults(string url, byte[] content)
        {
            var bytes = content.Length;
            var displayUrl = url.Replace("https://", "");
            resultsTextBox.Text += $"\n{displayUrl,-58} {bytes,8}";
        }
    }
}
