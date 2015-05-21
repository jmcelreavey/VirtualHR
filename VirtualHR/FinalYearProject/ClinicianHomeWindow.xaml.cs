using System.Windows;
using System.Windows.Controls;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for ClinicianHomeWindow.xaml
    /// </summary>
    public partial class ClinicianHomeWindow : Page
    {

        public ClinicianHomeWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Navigates to Patient Create Window
            this.NavigationService.Navigate(new CreatePatientWindow());
        }

        private void btnRetrieve_Click(object sender, RoutedEventArgs e)
        {
            // Navigates to Patient Search Window
            this.NavigationService.Navigate(new RetrievePatientWindow());
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // Logout on Yes
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you wish to log out?", "Log Out Confirmation", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                // Navigates to Login Window
                this.NavigationService.Navigate(new LoginWindow());
            }
        }
    }
}
