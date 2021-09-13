using MaxPower.NetPro.Setup.WPFView.ViewModels;
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

namespace DemoUse.WPFView.Views
{
    /// <summary>
    /// SetDataPage.xaml 的互動邏輯
    /// </summary>
    public partial class SetDataPage : Window
    {
        public SetDataPage(SetDataPageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
            viewModel.Close = () => Close();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
