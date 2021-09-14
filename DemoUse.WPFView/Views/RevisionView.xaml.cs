using DemoUse.WPFView.Models;
using DemoUse.WPFView.ViewModels;
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
    /// RevisionView.xaml 的互動邏輯
    /// </summary>
    public partial class RevisionView : Window
    {
        public RevisionView(BootstrapperApplicationModel bootstrapper)
        {
            InitializeComponent();
            var vm = new RevisionViewModel(bootstrapper);
            this.DataContext = vm;
            this.Closed += (sender, e) =>
            vm.CancelCommand.Execute(this);
        }
        private void window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var thisTextBox = (TextBox)sender;
            if (thisTextBox.LineCount != -1)
            {
                thisTextBox.ScrollToLine(thisTextBox.LineCount - 1);
            }
        }
    }
}
