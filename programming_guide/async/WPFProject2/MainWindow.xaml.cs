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


namespace WPFProject2
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
            await CreateMultipleTasksAsync();
            resultsTextBox.Text += "\r\n Control returned to startButton_Click.\r\n";
            startButton.IsEnabled = true;
        }

        private async Task<int> ProcessUrlAsync(string url, HttpClient client)
        {
            var byteArray = await client.GetByteArrayAsync(url);
            DisplayResults(url, byteArray);
            return byteArray.Length;
        }

        private void DisplayResults(string url, byte[] content)
        {
            var bytes = content.Length;
            var displayUrl = url.Replace("https://", "");
            resultsTextBox.Text += $"\r\n{displayUrl, -58} {bytes, 8}";
        }

        private async Task CreateMultipleTasksAsync()
        {
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            client.DefaultRequestHeaders.Add(
                "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");

            Task<int> download1 = ProcessUrlAsync("https://msdn.microsoft.com", client);
            Task<int> download2 = 
                ProcessUrlAsync("https://msdn.microsoft.com/library/hh156528(VS.110).aspx", client);
            Task<int> download3 =
                ProcessUrlAsync("https://msdn.microsoft.com/library/67w7t67f.aspx", client);

            int length1 = await download1;
            int length2 = await download2;
            int lenght3 = await download3;
            int total = length1 + length2 + lenght3;

            resultsTextBox.Text += $"\r\n\nTotal bytes returned: {total}. \r\n";
        }
    }
}
