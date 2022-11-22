using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfKuGouGet.ViewModel;
using Button = System.Windows.Controls.Button;

namespace WpfKuGouGet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = KuGouHelper.Instance;

            this.NumTextBox.Text = KuGouHelper.Instance.SearchNum.ToString();
        }

        private async void ButtonBase_OnClickAsync()
        {
            try
            {
                var text = this.UrlTextBox.Text;
                if (string.IsNullOrEmpty(text))
                    return;

                var numText = this.NumTextBox.Text;
                int num = 50;
                if(!string.IsNullOrEmpty(numText))
                    int.TryParse(numText, out num);
                KuGouHelper.Instance.SearchNum = num;

                var ok = await KuGouHelper.Instance.Search(text, 1);
                if(!ok)
                    Console.WriteLine("搜索失败！");
            }
            catch /*(Exception e)*/
            {
                //Console.WriteLine(e);
                //throw;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            ButtonBase_OnClickAsync();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }

        private void PathCacheButton_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.Description = "请选择文件夹";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (string.IsNullOrEmpty(dialog.SelectedPath))
                    {
                        System.Windows.MessageBox.Show(this, "文件夹路径不能为空", "提示");
                        return;
                    }
                    KuGouHelper.Instance.PathCache = dialog.SelectedPath;
                }
            }
            catch /*(Exception exception)*/
            {
                //Console.WriteLine(exception);
                //throw;
            }
        }

        private void DownLoadButton_clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                Button? dowButton = sender as Button;

                if(dowButton == null)
                    return;

                SongInfo? item = dowButton.Tag as SongInfo;

                if(item == null)
                    return;

                KuGouHelper.Instance.Download(item.SongFileHash, item.SongAlbumId);

            }
            catch /*(Exception exception)*/
            {
                //Console.WriteLine(exception);
                //throw;
            }
        }

        private void DownLoadAllButton_OnClick(object sender, RoutedEventArgs e)
        {
            KuGouHelper.Instance.DownloadAll();
        }
    }
}
