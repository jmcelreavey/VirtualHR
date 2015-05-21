using System.Windows;
using System.Windows.Controls;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for PatientHomeWindow.xaml
    /// </summary>
    public partial class PatientHomeWindow : Page
    {
        private UserDetails userDetails;

        public PatientHomeWindow(UserDetails userDetails)
        {
            InitializeComponent();
            this.userDetails = userDetails;
        }

        private void btnExercise_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to exercise select screen with user details
            this.NavigationService.Navigate(new ExerciseSelectWindow(userDetails));
        }

        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            // Logout on Yes
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you wish to log out?", "Log Out Confirmation", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                // Navigate to Login Window
                this.NavigationService.Navigate(new LoginWindow());
            }
        }

        private void btnResults_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to patient results window with user details
            this.NavigationService.Navigate(new PatientResultWindow(userDetails, true));
        }
    }
}
