using System.Windows;
using System.Windows.Controls;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for ExerciseSelectWindow.xaml
    /// </summary>
    public partial class ExerciseSelectWindow : Page
    {
        private UserDetails userDetails;

        //User passes in details upon entering page
        public ExerciseSelectWindow(UserDetails userDetails)
        {
            InitializeComponent();
            this.userDetails = userDetails;
        }

        private void btnPerformExercise_Click(object sender, RoutedEventArgs e)
        {
            // Pass in exercise name and user details and navigate to the exercise 
            var ExerciseName = ((Button)sender).Tag.ToString();
            NavigationService.Navigate(new ExercisePerformWindow(ExerciseName, userDetails));
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            // navigate back to patient home window with the user details
            NavigationService.Navigate(new PatientHomeWindow(userDetails));
        }
    }
}
