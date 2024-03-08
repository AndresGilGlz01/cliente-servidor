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

namespace tcp_proyecto_cliente.Views.Controls
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModels.MainViewModel;

            viewModel!.OnSelectedPicture += ViewModel_OnSelectedPicture;
            viewModel!.OnSendPicture += HomeView_OnSendPicture; ;
            viewModel!.OnCancel += HomeView_OnCancel; ;
        }

        private void HomeView_OnCancel(object? sender, EventArgs e)
        {
            var viewModel = DataContext as ViewModels.MainViewModel;

            btnOk.Content = "Select photo";
            btnOk.Command = viewModel!.ChoosePhotoCommand;

            btnCancel.Content = "Disconnect";
            btnCancel.Command = viewModel!.DisconnectCommand;
        }

        private void ViewModel_OnSelectedPicture(object? sender, EventArgs e)
        {
            var viewModel = DataContext as ViewModels.MainViewModel;

            btnOk.Content = "Send";
            btnOk.Command = viewModel!.SendPhotoCommand;

            btnCancel.Content = "Cancel";
            btnCancel.Command = viewModel!.CancelCommand;
        }

        private void HomeView_OnSendPicture(object? sender, EventArgs e)
        {
            var viewModel = DataContext as ViewModels.MainViewModel;

            btnOk.Content = "Select photo";
            btnOk.Command = viewModel!.ChoosePhotoCommand;

            btnCancel.Content = "Disconnect";
            btnCancel.Command = viewModel!.DisconnectCommand;
        }
    }
}
