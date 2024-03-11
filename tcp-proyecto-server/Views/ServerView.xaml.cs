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

namespace tcp_proyecto_server.Views
{
    /// <summary>
    /// Interaction logic for ServerView.xaml
    /// </summary>
    public partial class ServerView : Window
    {
        public ServerView()
        {
            InitializeComponent();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
        }

        
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void lista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            indice = lista.SelectedIndex;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
        }
        double offset = 0;
        int indice = 0;
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            ScrollViewer sv = (ScrollViewer)btn.Tag;
            offset -= 240;
            if (offset < 0)
            {
                offset = 0;
            }

            sv.ScrollToHorizontalOffset(offset);
            if (indice <= 0)
            {
                indice = 0;
            }
            else
            {
                indice--;
            }

            lista.SelectedItem = lista.Items.GetItemAt(indice);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            ScrollViewer? sv = (ScrollViewer)btn.Tag;
            offset += 240;
            if (offset > sv.ScrollableWidth)
            {
                offset = sv.ScrollableWidth;
            }
            sv.ScrollToHorizontalOffset(offset);
            if (indice >= lista.Items.Count - 1)
            {
                indice = lista.Items.Count - 1;
            }
            else
            {
                indice++;
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
