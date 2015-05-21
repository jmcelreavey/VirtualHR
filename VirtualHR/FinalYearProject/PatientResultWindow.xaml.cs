using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for PatientResultWindow.xaml
    /// </summary>
    public partial class PatientResultWindow : Page
    {
        private string exercise = "";
        private UserDetails userDetails;
        private List<Tuple<int, DateTime>> exerciseDates = new List<Tuple<int, DateTime>>();
        private List<Tuple<string, double>> exerciseResults = new List<Tuple<string, double>>();
        private bool isPatient;
        int exerciseId = 0;

        // Pass in user details and whether the user is patient or not
        public PatientResultWindow(UserDetails userDetails, bool isPatient)
        {
            InitializeComponent();
            this.userDetails = userDetails;
            this.isPatient = isPatient;
            // Populate dropdowns
            setDatesDropdown();
        }

        // If exercise has just been completed pass in exercise choice as well
        public PatientResultWindow(UserDetails userDetails, string exercise, bool isPatient)
        {
            InitializeComponent();
            this.userDetails = userDetails;
            this.exercise = exercise;
            this.isPatient = isPatient;

            // Set select exercise to what was just completed
            cbxExercises.Text = this.exercise;

            // Populate dropdowns
            setDatesDropdown();
        }

        public void setDatesDropdown()
        {
            // Retrieve all exercising dates for currently select dropdown for selected user(if any exist)
            exerciseDates = SQLFunctions.RetrieveExerciseDates(userDetails.UserId, cbxExercises.Text.ToString());

            // Clear any existing data
            if (cbxDates != null)
            {
                cbxDates.Items.Clear();
            }

            // Populate for each exercise date that exists
            foreach (var exerciseDate in exerciseDates)
            {
                cbxDates.Items.Add(exerciseDate.Item2);
            }

            // If dates existed set dropdown to the most recent
            if (exerciseDates.Count > 0)
            {
                cbxDates.SelectedItem = exerciseDates[0].Item2;
            }

            //Populate the charts with the select data
            populateCharts();
        }

        public void populateCharts()
        {
            foreach (var exerciseDate in exerciseDates)
            {
                // Find the selected date in the queried list
                if (exerciseDate.Item2.Equals(cbxDates.SelectedItem))
                {
                    // Set the exercise ID for that date (unique)
                    exerciseId = exerciseDate.Item1;
                }
            }
            // Retrieve the repitions and times for selected ID
            exerciseResults = SQLFunctions.RetrieveExerciseResults(exerciseId);

            // Set Line Graph's Title
            LineCategoryAxis.Title = "Repetition #";
            LineLinearAxis.Title = "Time (seconds)";

            // Set Column Graphs Title
            ColumnCategoryAxis.Title = "Repetition #";
            ColumnLinearAxis.Title = "Time (seconds)";

            // If data existed populate else set them to null
            if (exerciseDates.Count > 0)
            {
                //Setting data for column chart
                ctkColumnChart.DataContext = exerciseResults;

                // Setting data for line graph
                ctkLineGraph.DataContext = exerciseResults;
            }
            else
            {
                ctkColumnChart.DataContext = null;
                ctkLineGraph.DataContext = null;
            }
        }

        // Upon selecting a date populate the charts
        private void cbxDates_DropDownClosed(object sender, EventArgs e)
        {
            populateCharts();
        }

        // Upon selecting a date populate dropdowns (also charts)
        private void cbxExercises_DropDownClosed(object sender, EventArgs e)
        {
            setDatesDropdown();
        }

        private void btnDone_Click(object sender, RoutedEventArgs e)
        {
            // Return to the Patients or Clinicians Home window
            if (isPatient)
            {
                this.NavigationService.Navigate(new PatientHomeWindow(userDetails));
            }
            else
            {
                this.NavigationService.Navigate(new RetrievePatientWindow(userDetails));
            }
        }

        // Display selected chart type if data exists
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (rdoColumnChart != null)
            {
                if (rdoColumnChart.IsChecked == true)
                {
                    ctkColumnChart.Visibility = System.Windows.Visibility.Visible;
                    ctkLineGraph.Visibility = System.Windows.Visibility.Hidden;
                }
                else
                {
                    ctkColumnChart.Visibility = System.Windows.Visibility.Hidden;
                    ctkLineGraph.Visibility = System.Windows.Visibility.Visible;
                }
            }
        }
        
        private void btnHistorical_Click(object sender, RoutedEventArgs e)
        {
            List<int> ExerciseIds = SQLFunctions.RetrieveTop10ExerciseResults(exerciseId, userDetails.UserId, cbxExercises.Text);
            //Clear existing data
            exerciseResults.Clear();

             //Add the exerciseIDs that were retrieved from our list
             exerciseResults = SQLFunctions.MultipleRetrieveExerciseResults(ExerciseIds);

            // If data existed populate else set them to null

             // Set Column Graph's Title
             ColumnCategoryAxis.Title = "Exercise Date";
             ColumnLinearAxis.Title = "Average Time (seconds)";

             // Set Line Graph's Title
             LineCategoryAxis.Title = "Exercise Date";
             LineLinearAxis.Title = "Average Time (seconds)";

            if (exerciseDates.Count > 0)
            {
                // Setting data for column chart
                ctkColumnChart.DataContext = null;
                ctkColumnChart.DataContext = exerciseResults;

                // Setting data for line graph
                ctkLineGraph.DataContext = null;
                ctkLineGraph.DataContext = exerciseResults;
            }
            else
            {
                ctkColumnChart.DataContext = null;
                ctkLineGraph.DataContext = null;
            }
        }
    }
}
