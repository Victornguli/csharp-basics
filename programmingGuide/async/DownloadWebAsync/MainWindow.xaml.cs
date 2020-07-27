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


namespace DownloadWebAsync
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

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            startButton.IsEnabled = false;
            resultsTextBox.Clear();
            errorsTextBox.Clear();
            errorsTextBox.Text += $"\r\nErrors:\r\n";
            resultsTextBox.Text += $"\r\n{"Url",-58} {"Bytes Returned",8}\r\n";
            await SumPageSizesAsync();
            resultsTextBox.Text += "\r\nRequests Complete.";
            startButton.IsEnabled = true;
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

        /// <summary>
        /// Returns byte array fetched from a url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>url contents in byte array</returns>
        private async Task<byte[]> getUrlContentsAsync(string url)
        {
            var content = new MemoryStream();

            // Initialize a WebRequest connection.
            var webReq = (HttpWebRequest)WebRequest.Create(url);
            // Add User-Agent header to avoid 403 and 400 errors
            webReq.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/51.0.2704.103 Safari/537.36";
            try
            {
                Task<WebResponse> getResponseTask = webReq.GetResponseAsync();
                using (WebResponse response = await getResponseTask)
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        Task copyTask = responseStream.CopyToAsync(content);
                        await copyTask;
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
                            Task copyTask = responseStream.CopyToAsync(content);
                            await copyTask;
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

        private async Task<int> getUrlContentsUsingHttpClientAsync(string url)
        {
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000, Timeout = TimeSpan.FromSeconds(10) };
            // Add User-Agent header to avoid 403 and 400 errors
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/51.0.2704.103 Safari/537.36");
            byte[] urlContents = new byte[] { };
            try
            {
                urlContents = await client.GetByteArrayAsync(url);
            }
            catch (HttpRequestException e)
            {
                errorsTextBox.Text += $"\nRequest to {url} failed. {e.Message}";
            }
            catch (Exception e)
            {
                errorsTextBox.Text += $"\nError when requesting {url}. {e.Message}";
            }
            displayResults(url, urlContents);
            return urlContents.Length;
        }

        private async Task SumPageSizesAsync()
        {
            List<string> urlList = SetUpURLList();

            IEnumerable<Task<int>> downloadTasksQuery =
                from url in urlList select getUrlContentsUsingHttpClientAsync(url);

            Task<int>[] downloadTasks = downloadTasksQuery.ToArray();

            // Alternative to using the Query method above:
            // Initialize the downloadTasks as a List of tasks and use foreach loop
            // to add the processUrlTasks
            //List<Task<int>> downloadTasks = new List<Task<int>>();
            //foreach (var url in urlList)
            //{
            //    Task<int> processUrlTask = ProcessUrlAsync(url);
            //    downloadTasks.Add(processUrlTask);
            //}

            int[] lengths = await Task.WhenAll(downloadTasks);
            var total = lengths.Sum();

            resultsTextBox.Text += $"\r\nTotal bytes returned: {total} \r\n";
        }

        private async Task<int> ProcessUrlAsync(string url)
        {
            var byteArray = await getUrlContentsAsync(url);
            displayResults(url, byteArray);
            return byteArray.Length;
        }

        private void displayResults(string url, byte[] content)
        {
            var bytes = content.Length;
            var displayUrl = url.Replace("https://", "");
            resultsTextBox.Text += $"\n{displayUrl,-58} {bytes,8}";
        }

    }
}
