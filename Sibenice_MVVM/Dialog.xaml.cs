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

namespace Sibenice_MVVM
{
    /// <summary>
    /// Interakční logika pro Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public Dialog()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public bool Confirmed { get; set; } = false;
       

        private void deleteContent(object sender, MouseEventArgs e)
        {
            letterInput.Width = letterInput.ActualWidth;
            letterInput.Text = "";
            letterInput.Foreground = Brushes.Black;
            letterInput.MouseEnter -= deleteContent;
        }

        private void Confirm(object sender, RoutedEventArgs e)
        {
            Confirmed = true;
            Visibility = Visibility.Hidden;
        }

        private void Cancel(object sender, RoutedEventArgs e)
        {
            Confirmed = false;
            Visibility = Visibility.Hidden;
        }
    }
}
