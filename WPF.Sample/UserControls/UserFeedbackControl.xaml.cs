using System;
using System.Windows;
using System.Windows.Controls;
using WPF.Sample.ViewModelLayer;

namespace WPF.Sample.UserControls
{
    /// <summary>
    /// Interaction logic for UserFeedbackControl.xaml
    /// </summary>
    public partial class UserFeedbackControl : UserControl
    {
        private UserFeedbackViewModel _viewModel = null;

        public UserFeedbackControl()
        {
            InitializeComponent();

            // Connect to instance of the view model
            _viewModel = (UserFeedbackViewModel) this.Resources["viewModel"];
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Close(true);
        }

        private void SendFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SendFeedback();
        }
    }
}
