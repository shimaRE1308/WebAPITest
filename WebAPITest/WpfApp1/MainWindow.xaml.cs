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

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Send(object sender, RoutedEventArgs e)
        {
            
            Hoge();

            MessageBox.Show("通信終了。" + Id++);
        }

        private static int Id = 0;

        private static HttpClient client = new HttpClient();

        private void Hoge()
        {
            AsyncHelper.RunSync(() => this.Send());
        }


        private async Task Send()
        {
            MessageBox.Show("通信をします。" + Id++);

            HttpResponseMessage mes = await client.GetAsync("http://localhost:9000/api/Test1");
            string result = await mes.Content.ReadAsStringAsync();
            
            MessageBox.Show(result + "     " + Id++);

        }

        internal static class AsyncHelper
        {
            private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

            public static TResult RunSync<TResult>(Func<Task<TResult>> func)
            {
                return AsyncHelper._myTaskFactory.StartNew<Task<TResult>>(func).Unwrap<TResult>().GetAwaiter().GetResult();
            }

            public static void RunSync(Func<Task> func)
            {
                AsyncHelper._myTaskFactory.StartNew<Task>(func).Unwrap().GetAwaiter().GetResult();
            }
        }

    }
}
