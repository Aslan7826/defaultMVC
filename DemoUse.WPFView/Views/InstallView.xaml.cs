using DemoUse.WPFView.ViewModels;
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
using System.Windows.Shapes;

namespace DemoUse.WPFView.Views
{
    /// <summary>
    /// InstallView.xaml 的互動邏輯
    /// </summary>
    public partial class InstallView : Window
    {
        SetDataPageViewModel setDataViewModel;
        public InstallView(InstallViewModel viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
            this.Closed += (sender, e) =>
            viewModel.CancelCommand.Execute(this);
            setDataViewModel = new SetDataPageViewModel(viewModel.model);

        }
        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
        private void StartInstall_Click(object sender, RoutedEventArgs e)
        {
            SetDataPage setPage = new SetDataPage(setDataViewModel);
            setPage.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            setPage.Owner = this;
            setPage.ShowDialog();
           
        }
    }
}
