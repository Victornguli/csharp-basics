using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

namespace ReEntrancy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Task pending = null;
        char grp = (char)('A' - 1);

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            grp = (char)(grp + 1);
            resultsBox.Text += $"\nPreparing Tasks for group {grp}";
            try
            {
                char finishedGroup = await AccessTheWebAsync(grp);
                resultsBox.Text += $"\r\n\r\nGroup {finishedGroup} is complete\r\n";
            }
            catch (Exception)
            {
                resultsBox.Text += "\r\nDownloads failed.";
            }
        }

        private async Task<char> AccessTheWebAsync(char group)
        {
            List<string> urlList = setupUrlList();
            HttpClient client = new HttpClient() { MaxResponseContentBufferSize = 1000000 };
            client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");

            /**IEnumerable<Task<byte[]>> donwloadQuery =
                from url in urlList select client.GetByteArrayAsync(url);
            Task<byte[]>[] downloadTasks = donwloadQuery.ToArray();
            **/
            // Equivalent to commented out block above..
            Task<byte[]>[] downloadTasksArray = urlList.Select(url => client.GetByteArrayAsync(url)).ToArray();

            Task handleGroup = HandleGroup(group, urlList, downloadTasksArray);
            pending = handleGroup;

            resultsBox.Text += $"\r\n#Task assigned for group {group}. Donwload tasks are active.\r\n";

            await pending;

            return group;
        }

        private async Task HandleGroup(char group, List<string> urlList, Task<byte[]>[] tasksArray)
        {
            if (pending != null) await pending;

            int total = 0;
            for (int i = 0; i < tasksArray.Length; i++)
            {
                byte[] contents = await tasksArray[i];
                DisplayResults(group, i, urlList[i], contents.Length);
                total += contents.Length;
            }
            resultsBox.Text += $"\r\nTOTAL bytes returned {total}\r\n";
        }

        private List<string> setupUrlList()
        {
            List<string> urlList = new List<string> {
                "https://msdn.microsoft.com/library/hh191443.aspx",
                "https://msdn.microsoft.com/library/aa578028.aspx",
                "https://msdn.microsoft.com/library/jj155761.aspx",
                "https://msdn.microsoft.com/library/hh290140.aspx",
                "https://msdn.microsoft.com/library/hh524395.aspx",
                "https://msdn.microsoft.com/library/ms404677.aspx",
                "https://msdn.microsoft.com",
                "https://msdn.microsoft.com/library/ff730837.aspx"
            };
            return urlList;
        }

        private void DisplayResults(char group, int pos, string url, int length)
        {
            string shortUrl = url.Replace("https://", "");
            string dispGrp = $"{group}-{pos}. {shortUrl}";
            resultsBox.Text += $"\r\n{dispGrp, -58} {length, 8}";
        }
    }
}
