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
using System.Threading;
using System.Runtime.CompilerServices;

namespace CancellingTasks
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
            resultsBox.Text += "Clicked Start Button";
            startButton.IsEnabled = false;
            await AccessTheWebAsync(cts);
            startButton.IsEnabled = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (cts != null)
            {
                resultsBox.Text = "Cancelled tasks";
                cts.Cancel();
            }
            resultsBox.Text = "Clicked Cancellation Button";
        }

        private async Task AccessTheWebAsync(CancellationTokenSource cts)
        {
            List<string> urlList = SetUpURLList();
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");

            IEnumerable<Task<int>> downloadTasksQuery = 
                from url in urlList select ProcessUrlAsync(url, client, cts.Token);
            Task<int>[] downloadTasks = downloadTasksQuery.ToArray();
            
            // Await for any first task that is complete
            Task<int> finishedTask = await Task.WhenAny(downloadTasks);

            // Cancel all remaining tasks..
            cts.Cancel();
            var length = await finishedTask;
            resultsBox.Text += $"\r\nLength of the downloaded website is {length} \r\n";

        }


        private async Task<int> ProcessUrlAsync(string url, HttpClient client, CancellationToken ct)
        {
            try
            {
                // GetAsync returns a HttpResponseMessage
                HttpResponseMessage response = await client.GetAsync(url, ct);

                // Retrieve the website contents from the response
                byte[] urlContents = await response.Content.ReadAsByteArrayAsync();

                return urlContents.Length;
            }
            catch (HttpRequestException e)
            {
                resultsBox.Text += $"\r\n{url, -20} Error: {e.Message, 5}";
            }
            catch (Exception ex)
            {
                resultsBox.Text += $"\r\nError: {ex.Message}";
            }
            return 0;
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
