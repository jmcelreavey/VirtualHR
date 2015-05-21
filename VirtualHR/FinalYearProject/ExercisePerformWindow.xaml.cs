using Leap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace FinalYearProject
{
    /// <summary>
    /// Interaction logic for ExercisePerformWindow.xaml
    /// </summary>
    public partial class ExercisePerformWindow : Page
    {
        // Handler used to detect interaction with the Leap Motion
        private CustomHandler handler = new CustomHandler();

        // information needed for tracking/updating user results
        private UserDetails userDetails;
        private Stopwatch timer = new Stopwatch();
        private bool exerciseStarted = false;
        private bool exerciseComplete = false;
        private bool instructionGiven = false;
        private bool bestTimeOccured = false;
        private string exerciseChoice;
        private int repsComplete;
        private double bestTime = 0;
        private Tuple<double, string> bestTimeDate;
        private List<Tuple<int, double>> repsTimes = new List<Tuple<int, double>>();
        private List<int> fingers = new List<int>();


        // Initialises the class with the logged in users details and exercise choice
        public ExercisePerformWindow(string choice, UserDetails userDetails)
        {
            //Initialise objects and set user details/choice
            InitializeComponent();
            this.exerciseChoice = choice;
            this.userDetails = userDetails;

            //Retrieve the users best repitition time
            this.bestTimeDate = SQLFunctions.RetrieveBestTimeDate(userDetails.UserId, exerciseChoice);

            // Call Listener_OnHandRegistered when Leap Motion detection occurs
            this.handler.Listener.OnHandRegistered += Listener_OnHandRegistered;

            //Update labels with retrieved/passed in results
            updateExerciseName(choice);
            updateBestTime(bestTimeDate.Item1.ToString(), bestTimeDate.Item2.ToString());

        }

        //Processes and checks if a hand exercise is in progress or completed
        private void Listener_OnHandRegistered(Hand hand)
        {
            // If exercise is not yet complete
            if (repsComplete < 3)
            {
                // If timer hasn't been started (user hand has been detected for the first time)
                if (!timer.IsRunning)
                {
                    timer.Start();
                    instructionGiven = false;
                    if (repsComplete > 0)
                    {
                        updateInstructions("Next exercise will begin in 3 seconds", true);
                    }
                    else
                    {
                        updateInstructions("Exercise will begin in 3 seconds", false);
                    }
                }

                // Wait for 3 second countdown to finish
                if (timer.ElapsedMilliseconds > 3000 || exerciseStarted == true)
                {
                    // If exercise hasn't been started, start it               
                    if (!exerciseStarted)
                    {
                        timer.Restart();
                        exerciseStarted = true;
                    }
                    // Stop checking once exercise is complete
                    if (!exerciseComplete)
                    {
                        switch (exerciseChoice)
                        {
                            case "Fist Clench":
                                // check if instruction has been given
                                if (!instructionGiven)
                                {
                                    updateInstructions("Clench your hand and make a fist", false);
                                    instructionGiven = true;
                                }
                                // Once exercise is complete and hand is flat, set true
                                exerciseComplete = checkFistClench(hand);
                                break;
                            case "Thumb Touch":
                                if (!instructionGiven)
                                {
                                    updateInstructions("Pinch each finger with your thumb tightly", false);
                                    instructionGiven = true;
                                }
                                // Once exercise is complete and hand is flat, set true
                                exerciseComplete = checkThumbTouch(hand);
                                break;
                            case "Claw Stretch":
                                if (!instructionGiven)
                                {
                                    updateInstructions("Bend your fingertips down to touch the base of each finger joint. \nYour hand should look a little like a claw.", false);
                                    instructionGiven = true;
                                }
                                // Once exercise is complete and hand is flat, set true
                                exerciseComplete = checkClawStretch(hand);
                                break;
                        }
                    }
                    //If repition is complete and no longer performing an exercise
                    if (exerciseComplete && hand.GrabStrength < 0.2 && hand.PinchStrength == 0)
                    {
                        //convert to seconds and round to 2 decimal places
                        double elapsedSeconds = ((double)timer.ElapsedMilliseconds / 1000);
                        elapsedSeconds = Math.Round(elapsedSeconds, 2);

                        // Update best times if they're better or not set yet
                        if (elapsedSeconds < bestTimeDate.Item1 || bestTimeDate.Item1 == 0)
                        {
                            bestTime = elapsedSeconds;
                            bestTimeOccured = true;
                            updateBestTime(bestTime.ToString(), DateTime.Now.ToString());
                        }

                        // Reset values for the next repition and increment counter
                        timer.Reset();
                        fingers.Clear();
                        exerciseComplete = false;
                        exerciseStarted = false;
                        repsComplete++;

                        // Store repition time
                        Tuple<int, double> repTime = new Tuple<int, double>(repsComplete, elapsedSeconds);
                        repsTimes.Add(repTime);

                        //Update label on screen
                        updateRepInfo(repsComplete.ToString(), elapsedSeconds.ToString());
                    }
                }
            }
            else
            {              
                updateInstructions("Exercise complete, well done!", true);

                // Update database with results from exercise
                SQLFunctions.InsertExerciseResults(userDetails.UserId, exerciseChoice, bestTimeOccured, bestTime, DateTime.Now, repsTimes);

                // Enable next button and disable cancel as results are already stored
                this.Dispatcher.Invoke((Action)(() =>
                {
                    btnCancel.IsEnabled = false;
                    btnNext.IsEnabled = true;
                }));      
          
                // Store Leap Motion Listener
                handler.Dispose();               
            }
        }

        // Check if Fist Clench is being made
        private Boolean checkFistClench(Hand hand)
        {
            float sum = 0;
            // For each finger get the average bone direction
            for (var i = 0; i < hand.Fingers.Count; i++)
            {
                Finger finger = hand.Fingers[i];
                Leap.Vector meta = finger.Bone(Bone.BoneType.TYPE_METACARPAL).Direction;
                Leap.Vector proxi = finger.Bone(Bone.BoneType.TYPE_PROXIMAL).Direction;
                Leap.Vector inter = finger.Bone(Bone.BoneType.TYPE_INTERMEDIATE).Direction;
                float dMetaProxi = meta.Dot(proxi);
                float dProxiInter = proxi.Dot(inter);
                sum += dMetaProxi;
                sum += dProxiInter;
            }
            sum = sum / 10;

            // If it's less than half and grab strength is full, hand is clenched 
            if (sum <= 0.5 && hand.GrabStrength == 1)
            {
                updateInstructions("Well done, unclench and return to start position", false);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if Claw Stretch has been complete
        private Boolean checkClawStretch(Hand hand)
        {
            // If no fingers extended hand is in claw position 
            if (getExtendedFingers(hand) == 0)
            {
                updateInstructions("Well done, unclench and return to start position", false);
                return true;
            }
            else
            {
                return false;
            }
        }

        // Check if Thumb Touch has been complete
        private Boolean checkThumbTouch(Hand hand)
        {
            // Check each finger
            for (int i = 0; i < hand.Fingers.Count; i++)
            {
                // If finger is not extended and pinch is occuring
                if (!hand.Fingers[i].IsExtended && i != (int)Finger.FingerType.TYPE_THUMB && hand.PinchStrength == 1)
                {
                    // Check if this finger has already performed the exercise, if not then add it
                    if (!fingers.Contains(i))
                    {
                        if (i == 1 || fingers.Contains(i - 1))
                        {
                            fingers.Add(i);
                            // Ensure all fingers haven't completed the exercise
                            if (fingers.Count < 4)
                            {
                                updateInstructions("Now pinch the next finger against the thumb", false);
                            }
                        }
                    }
                }
            }

            // Check the list contains true for each finger, if it does the exercise is complete
            if (fingers.Contains(1) && fingers.Contains(2) && fingers.Contains(3) && fingers.Contains(4))
            {
                updateInstructions("Great, now return your hand to starting position", false);
                return true;
            } 
            else
            {
                return false;
            }            
        }

        // Counts extended fingers on the hand
        private int getExtendedFingers(Hand hand) {
            int f = 0;
            for (int i=0; i < hand.Fingers.Count; i++){
                if (hand.Fingers[i].IsExtended) {
                f++;
                }
            }
            return f;
        }

        // Update Best Time TextBlock
        private void updateBestTime(string time, string date)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                txtBestTime.Text = time + " Seconds" + System.Environment.NewLine + date;
            }));
        }

        // Update Exercise Name TextBlock
        private void updateExerciseName(string exercise)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                txtExerciseTitle.Text = exercise;
            }));
        }

        // Update Repition Info TextBlock
        private void updateRepInfo(string rep, string time)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                txtReps.Text = rep + "/3 Complete:" + System.Environment.NewLine + time + " Seconds";
            }));
        }

        // Update Instructions TextBlock
        private void updateInstructions(string instruction, bool repComplete)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (repComplete)
                {
                    txtOldInformation.Text = "";
                }
                else
                {
                    txtOldInformation.Text += txtInformation.Text + System.Environment.NewLine;
                }
                txtInformation.Text = instruction;
            }));
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Return to Patients home window if cancel click is true
            MessageBoxResult dialogResult = MessageBox.Show("Are you sure you wish to cancel, all progress will be lost.", "Cancel Confirmation", MessageBoxButton.YesNo);
            if (dialogResult == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new PatientHomeWindow(userDetails));
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to patients results window
            this.NavigationService.Navigate(new PatientResultWindow(userDetails, exerciseChoice, true));
        }
    }
}
