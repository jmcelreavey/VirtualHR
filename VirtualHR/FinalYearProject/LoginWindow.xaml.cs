using System.Windows;
using System.Windows.Controls;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class LoginWindow : Page
    {

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            // Retrieve the users details
            UserDetails userDetails = SQLFunctions.RetrieveUserDetails(txtName.Text);

            // Check if patient or clinician
            if (userDetails.AccountType == true) // Clinician
            {
                this.NavigationService.Navigate(new ClinicianHomeWindow());
            }
            else if (userDetails.AccountType == false) // Patient
            {
                // Pass in user details if patient
                this.NavigationService.Navigate(new PatientHomeWindow(userDetails));
            }
            else
            {
                // User not found
                MessageBox.Show("User not found, please check your spelling and try again", "User Not Found");
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Close NavigationWindow upon exit (shutdown application)
            (this.Parent as NavigationWindow).Close();
        }
    }
}
