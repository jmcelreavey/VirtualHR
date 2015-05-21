using System.Windows;
using System.Windows.Controls;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for CreatePatientWindow.xaml
    /// </summary>
    public partial class CreatePatientWindow : Page
    {
        public CreatePatientWindow()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            // Ensure all fields have been complete
            if (txtFirstName.Text != "" && txtLastName.Text != "" && txtAddress.Text != "" && txtPostCode.Text != "" && txtEmail.Text != "")
            {
                // Create an object of user details
                UserDetails userDetails = new UserDetails();
                userDetails.FullName = txtFirstName.Text + " " + txtLastName.Text;
                userDetails.Address = txtAddress.Text;
                userDetails.PostCode = txtPostCode.Text;
                userDetails.Email = txtEmail.Text;
                userDetails.AccountType = false;

                // Insert user details into the database
                SQLFunctions.InsertUserDetails(userDetails);

                // Success Message and redirect to Clinician Window
                MessageBox.Show("Account has been successfully created", "Account Created");
                this.NavigationService.Navigate(new ClinicianHomeWindow());
            }
            else
            {
                // Error Message
                MessageBox.Show("Please complete each field shown", "Mandatory Field Blank");
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //redirects to home window on back button click
            this.NavigationService.Navigate(new ClinicianHomeWindow());
        }
    }
}
