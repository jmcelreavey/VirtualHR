using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace FinalYearProject
{
    static class SQLFunctions
    {
        // Connection for local database
        static private SqlConnection connection = new SqlConnection("Data Source=EXCALIBUR\\SQLSERVER;Initial Catalog=VirtualHR;Integrated Security=True");

        // Retrieve User Details
        static public UserDetails RetrieveUserDetails(string fullName)
        {
            UserDetails userDetails = new UserDetails();

            connection.Open();

            // Select statement
            SqlCommand command = new SqlCommand("SELECT * FROM [tblUserDetails] WHERE [FullName] = @FullName", connection);
            command.Parameters.AddWithValue("@FullName", fullName);
            SqlDataReader reader = command.ExecuteReader();

            // Populate userDetails class
            while (reader.Read()) 
            {
                userDetails.UserId = (int)reader[0];
                userDetails.AccountType = (bool)reader[1];
                userDetails.FullName = (string)reader[2];
                userDetails.Address = (string)reader[3];
                userDetails.Email = (string)reader[4];
                userDetails.PostCode = (string)reader[5];
            }
            connection.Close();

            return userDetails;
        }

        // Retrieve Exercise Dates
        static public List<Tuple<int, DateTime>> RetrieveExerciseDates(int userId, string exercise)
        {
            connection.Open();
            // Select statement
            SqlCommand command = new SqlCommand("SELECT [ExerciseID], [TimeStamp] FROM [tblExercises] WHERE [UserId] = @UserId AND [Exercise] = @Exercise ORDER BY [TimeStamp] DESC", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Exercise", exercise);
            SqlDataReader reader = command.ExecuteReader();

            //Populate list with exercise times and IDs for each
            List<Tuple<int, DateTime>> exerciseDates = new List<Tuple<int,DateTime>>();
            while (reader.Read())
            {
                int exerciseId = (int)reader["ExerciseId"];
                DateTime timeStamp = (DateTime)reader["TimeStamp"];
                Tuple<int, DateTime> exerciseDate = new Tuple<int,DateTime>(exerciseId, timeStamp);
                exerciseDates.Add(exerciseDate);
            }
            connection.Close();

            return exerciseDates;
        }

        // Retrieve Exercise Results
        static public List<Tuple<string, double>> RetrieveExerciseResults(int exerciseId)
        {
            connection.Open();
            // Select statement
            SqlCommand command = new SqlCommand("SELECT [RepNumber], [RepTime] FROM [tblExerciseResults] WHERE [exerciseId] = @ExerciseId", connection);
            command.Parameters.AddWithValue("@ExerciseId", exerciseId);
            SqlDataReader reader = command.ExecuteReader();

            // Populate list with each repition time and repition number for the exercise ID passed
            List<Tuple<string, double>> exerciseResults = new List<Tuple<string, double>>();
            while (reader.Read())
            {
                string repNumber = "Rep: " + reader["RepNumber"].ToString();
                double repTime = (double)reader["RepTime"];
                Tuple<string, double> exerciseResult = new Tuple<string, double>(repNumber, repTime);
                exerciseResults.Add(exerciseResult);
            }
            connection.Close();

            return exerciseResults;
        }

        static public List<Tuple<string, double>> MultipleRetrieveExerciseResults(List<int> exerciseIds)
        {          
            List<Tuple<string, double>> exerciseResults = new List<Tuple<string, double>>();

            // Repeat for each exerciseId in the list
            IEnumerable<int> exerciseIdList = exerciseIds;
            foreach (int exerciseId in exerciseIdList.Reverse())
            {
                int counter = 0;
                string repName = "";
                double repTime = 0;
                connection.Open();
                // Select statement
                SqlCommand command = new SqlCommand("SELECT [RepNumber], [RepTime], tblExercises.timeStamp FROM [tblExerciseResults]  inner Join [tblExercises] on tblExerciseResults.exerciseID=tblExercises.exerciseID AND tblExerciseResults.exerciseID = @ExerciseID", connection);
                command.Parameters.AddWithValue("@ExerciseId", exerciseId);
                SqlDataReader reader = command.ExecuteReader();

                // Populate list with each repition time and repition number for the exercise ID passed
                while (reader.Read())
                {
                    counter++;
                    repName = reader["TimeStamp"].ToString();
                    repTime += (double)reader["RepTime"];  
                }
                repTime /= counter;
                repTime = Math.Round(repTime, 2);
                Tuple<string, double> exerciseResult = new Tuple<string, double>(repName, repTime);
                exerciseResults.Add(exerciseResult);
                connection.Close();
            }

            return exerciseResults;
        }

        static public List<int> RetrieveTop10ExerciseResults(int exerciseId, int userId, String exercise)
        {
            connection.Open();
            // Select statement
            SqlCommand command = new SqlCommand("SELECT TOP 10 [ExerciseID] FROM [tblExercises] WHERE [ExerciseId] <= @ExerciseId AND [UserId] = @UserId AND [Exercise] = @Exercise ORDER BY TimeStamp DESC", connection);
            command.Parameters.AddWithValue("@ExerciseId", exerciseId);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Exercise", exercise);
            SqlDataReader reader = command.ExecuteReader();

            // Populate list with each repition time and repition number for the exercise ID passed
            List<int> exerciseResults = new List<int>();
            while (reader.Read())
            {
                exerciseResults.Add((int)reader["ExerciseID"]);
            }
            connection.Close();

            return exerciseResults;
        }

        // Retrieve Best Time and Date
        static public Tuple<double, string> RetrieveBestTimeDate(int userId, string exerciseName)
        {
            connection.Open();
            // Select statement
            SqlCommand command = new SqlCommand("SELECT TOP 1 [TimeStamp], [BestTime] FROM [tblExercises] WHERE [UserId] = @UserId AND [BestTimeOccured] = 1 AND [Exercise] = @Exercise ORDER BY [TimeStamp] DESC", connection);
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Exercise", exerciseName);
            SqlDataReader reader = command.ExecuteReader();

            // Retrieve the Best Time and Date from the select statement
            double bestTime = 0;
            string bestDate = "";
            while (reader.Read())
            {
                bestDate = Convert.ToString(reader["TimeStamp"]);
                bestTime = Convert.ToDouble(reader["BestTime"]);
            }
            connection.Close();

            Tuple<double, string> bestDateTime = new Tuple<double,string>(bestTime, bestDate);
            return bestDateTime;
        }

        // Insert Exercise Results into Database
        static public void InsertExerciseResults(int userId, string exercise, bool bestTimeOccured, double bestTime, DateTime timeStamp, List<Tuple<int, double>> reps)
        {
            connection.Open();
            // Insert statement and values
            SqlCommand commmandInsertExercise = new SqlCommand("INSERT INTO [tblExercises] ([UserID], [Exercise], [BestTimeOccured], [BestTime], [TimeStamp]) VALUES(@UserID, @Exercise, @BestTimeOccured, @BestTime, @TimeStamp)", connection);
            commmandInsertExercise.Parameters.AddWithValue("@UserID", userId);
            commmandInsertExercise.Parameters.AddWithValue("@Exercise", exercise);
            commmandInsertExercise.Parameters.AddWithValue("@BestTimeOccured", bestTimeOccured);
            commmandInsertExercise.Parameters.AddWithValue("@BestTime", bestTime);
            commmandInsertExercise.Parameters.AddWithValue("@TimeStamp", timeStamp);
            commmandInsertExercise.ExecuteNonQuery();

            // Select the ID that was auto generated above
            SqlCommand command = new SqlCommand("SELECT @@Identity;", connection);
            SqlDataReader reader = command.ExecuteReader();
            int id = 0;
            while (reader.Read())
            {
                id = Convert.ToInt32(reader[0]);
            }
            connection.Close();

            // Insert a new row for each repition and time that was completed and store the UserID against each
            foreach (var rep in reps)
            {
                connection.Open();
                SqlCommand commmandInsertResults = new SqlCommand("INSERT INTO [tblExerciseResults] ([RepNumber], [RepTime], [ExerciseID]) VALUES(@RepNumber, @RepTime, @ID)", connection);
                commmandInsertResults.Parameters.AddWithValue("@RepNumber", rep.Item1);
                commmandInsertResults.Parameters.AddWithValue("@RepTime", rep.Item2);
                commmandInsertResults.Parameters.AddWithValue("@ID", id);
                commmandInsertResults.ExecuteNonQuery();
                connection.Close();
            }
        }

        // Insert User Details into Database
        static public void InsertUserDetails(UserDetails userDetails)
        {
            connection.Open();
            // Insert statement and values
            SqlCommand commmandInsert = new SqlCommand("INSERT INTO [tblUserDetails] ([AccountType], [FullName], [Address], [Email], [PostCode]) VALUES(@AccountType, @FullName, @Address, @Email, @PostCode)", connection);
            commmandInsert.Parameters.AddWithValue("@AccountType", userDetails.AccountType);
            commmandInsert.Parameters.AddWithValue("@FullName", userDetails.FullName);
            commmandInsert.Parameters.AddWithValue("@Address", userDetails.Address);
            commmandInsert.Parameters.AddWithValue("@Email", userDetails.Email);
            commmandInsert.Parameters.AddWithValue("@PostCode", userDetails.PostCode);
            commmandInsert.ExecuteNonQuery();
            connection.Close();
        }

        // Delete User Details from Database
        static public void DeleteUserDetails(int userId)
        {
            connection.Open();
            // Delete statement and values
            SqlCommand commandDelete = new SqlCommand("DELETE FROM [tblUserDetails] WHERE [UserId] = @UserId", connection);
            commandDelete.Parameters.AddWithValue("@UserId", userId);
            commandDelete.ExecuteNonQuery();
            connection.Close();
        }
    }
}

