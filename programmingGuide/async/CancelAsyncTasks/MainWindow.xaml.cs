using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Security.Policy;

namespace CancelAsyncTasks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CancellationTokenSource cts;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            cts = new CancellationTokenSource();
            resultsBox.Clear();
            errorsBox.Clear();
            errorsBox.Text = "\r\nErrors:\r\n";
            resultsBox.Text = $"\r\n{"Url", -58} {"Bytes Returned", 8}\r\n";
            startButton.IsEnabled = false;
            cancelButton.IsEnabled = true;

            // *** If a download process is already underway, cancel it.
            if (cts != null)
            {
                cts.Cancel();
            }

            CancellationTokenSource newCTS = new CancellationTokenSource();
            cts = newCTS;

            try
            {
                // Cancel all tasks after 10 seconds..
                cts.CancelAfter(10500);
                await AccessTheWebAsync(cts);
            }
            catch (OperationCanceledException)
            {
                resultsBox.Text += $"\r\nAll Async tasks cancelled.";
            }
            catch (Exception ex)
            {
                errorsBox.Text += $"\r\nError: {ex.Message}\n";
            }
            finally
            {
                startButton.IsEnabled = true;
                resultsBox.Text += $"\r\n\nAll Async Tasks Processing Completed..\r\n";
            }
            // *** When the process is complete, signal that another process can proceed.
            if (cts == newCTS)
            {
                cts = null;
                cancelButton.IsEnabled = false;
            }

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (cts != null)
            {
                resultsBox.Text += "\r\n\nCancelling tasks....";
                cts.Cancel();
            }
        }

        private async Task AccessTheWebAsync(CancellationTokenSource cts)
        {
            List<string> urlList = SetUpURLList();
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");

            IEnumerable<Task<int>> downloadTasksQuery =
                from url in urlList select ProcessUrlAsync(url, client, cts.Token);
            List<Task<int>> downloadTasks = downloadTasksQuery.ToList();

            // Alternatively process the query as a lambda:
            // List<Task<int>> downloadTasks = 
            //   urlList.Select(url => ProcessUrlAsync(url, client, cts.Token)).ToList();

            while (downloadTasks.Count > 0)
            {
                // Await for any first task that is complete
                Task<int> finishedTask = await Task.WhenAny(downloadTasks);

                downloadTasks.Remove(finishedTask);
                await finishedTask;
                
                /**
                //To cancel after a single request is completed. 
                cts.Cancel();
                **/
            }
        }


        private async Task<int> ProcessUrlAsync(string url, HttpClient client, CancellationToken ct)
        {
            byte[] urlContents = new byte[] { };
            try
            {
                // GetAsync returns a HttpResponseMessage
                HttpResponseMessage response = await client.GetAsync(url, ct);

                // Retrieve the website contents from the response
                urlContents = await response.Content.ReadAsByteArrayAsync();
            }
            catch (HttpRequestException e)
            {
                errorsBox.Text += $"\nFailed to process {url}: {e.Message}\n";
            }
            DisplayResults(url, urlContents);
            return urlContents.Length;
        }

        private void DisplayResults(string url, byte[] contents)
        {
            int length = contents.Length;
            string shortUrl = url.Replace("https://", "");
            resultsBox.Text += $"\n{shortUrl, -58} {length, 8}";
        }

        private List<string> SetUpURLList()
        {
            List<string> urls = new List<string>
            {
                "https://msdn.microsoft.com",
                "https://msdn.microsoft.com/library/hh290138.aspx",
                "https://msdn.microsoft.com/library/hh290140.aspx",
                "https://msdn.microsoft.com/library/dd470362.aspx",
                "https://msdn.microsoft.com/library/aa578028.aspx",
                "https://msdn.microsoft.com/library/ms404677.aspx",
                "https://msdn.microsoft.com/library/ff730837.aspx"
            };
            return urls;
        }

    }
}
