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
using WpfKuGouGet.ViewModel;

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
        }

        private async void ButtonBase_OnClickAsync()
        {
            try
            {
                var text = this.UrlTextBox.Text;
                if (string.IsNullOrEmpty(text))
                    return;

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
    }
}
