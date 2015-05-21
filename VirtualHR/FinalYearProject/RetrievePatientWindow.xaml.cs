using System.Windows;
using System.Windows.Controls;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for RetrievePatientWindow.xaml
    /// </summary>
    public partial class RetrievePatientWindow : Page
    {
        UserDetails userDetails;
        public RetrievePatientWindow()
        {
            InitializeComponent();
        }

        public RetrievePatientWindow(UserDetails userDetails)
        {
            this.userDetails = userDetails;
            InitializeComponent();
            setUserDetails(userDetails);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // If something has been entered search for user details
            userDetails = SQLFunctions.RetrieveUserDetails(txtName.Text);
            if (userDetails.UserId != 0)
            {            
                setUserDetails(userDetails);
            }
            else
            {
                MessageBox.Show("Account not found, please check the entered name", "Account Not Found");
            }
        }

        private void setUserDetails(UserDetails userDetails)
        {           
            // If user exists populate labels
             if (userDetails.UserId != 0 && userDetails.AccountType != true)
             {
                 txtName.Text = userDetails.FullName;
                 lblName.Content = userDetails.FullName;
                 lblEmail.Content = userDetails.Email;
                 lblAddress.Content = userDetails.Address;
                 lblPostCode.Content = userDetails.PostCode;
                 btnDelete.IsEnabled = true;
                 btnViewProgress.IsEnabled = true;
             }
        }    

        // Delete Account
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you wish to delete this account?", "Delete Confirmation", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                // Delete selected userID
                SQLFunctions.DeleteUserDetails(userDetails.UserId);
                MessageBox.Show("Account successfully deleted", "Account Deleted");
                this.NavigationService.Navigate(new ClinicianHomeWindow());
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Clincians Home Window
            this.NavigationService.Navigate(new ClinicianHomeWindow());
        }

        private void btnViewProgress_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Patient Result Window with the User Details
            this.NavigationService.Navigate(new PatientResultWindow(userDetails, false));
        }
    }
}
