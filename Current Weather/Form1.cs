﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Current_Weather
{
    public partial class Form1 : Form
    {
        //Global wariable with server's address
        readonly string BaseUrl = "https://weather-csharp.herokuapp.com/";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetWeather_Click(object sender, EventArgs e)
        {
            //Disable button to prevent user making another request before this one is done
            btnGetWeather.Enabled = false;

            //Read data from TextBoxes
            string city = txtCity.Text;
            string state = txtState.Text;

            if (LocationDataValid(city, state))
            {
                //Fetch current weather and display
                if (GetWeatherText(city, state, out string weather, out string textErrorMessage))
                {
                    lblWeather.Text = weather;
                }
                else
                {
                    MessageBox.Show(textErrorMessage, "Error");
                }

                if(picWeather.Image != null)
                {
                    picWeather.Image.Dispose(); // Clear previous image
                }

                if (GetWeatherImage(city, state, out Image image, out string imageErrorMessage)) 
                {
                    picWeather.Image = image;
                }
            }
            else
            {
                MessageBox.Show("Enter both city and state", "Error");
            }

            //Enable button so user can get weather for somewhere else
            btnGetWeather.Enabled = true;
        }

        private bool GetWeatherText(string city, string state, out string weatherText, out string errorMessage)
        {
            //Use the format method to make a string in the format
            //hppt://weather-csharp.herokuapp.com/text?city=dallas&state=tx

            string weatherTextUrl = String.Format("{0}text?city={1}&state={2}", BaseUrl, city, state);
            Debug.WriteLine(weatherTextUrl); // Message only seen by developers

            errorMessage = null;
            weatherText = null;

            try
            {
                using (WebClient client = new WebClient())
                {
                    weatherText = client.DownloadString(weatherTextUrl);
                }

                Debug.WriteLine(weatherText);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                errorMessage = e.Message; // You'll refine this in lab
                return false;
            }
        }

        private bool LocationDataValid(string city, string state)
        {
            //Make checks on data, return false if any fail
            if (String.IsNullOrWhiteSpace(city))
            {
                return false;
            }

            if (String.IsNullOrWhiteSpace(state))
            {
                return false;
            }

            // All checks passed? Data looks good, return true
            return true;
        }

        private bool GetWeatherImage(string city, string state, out Image weatherImage, out string errorMessage)
        {
            weatherImage = null; // Initialize the out parameters
            errorMessage = null; // Will only set one of these, depending on if things work or not

            try
            {
                using (WebClient client = new WebClient())
                {
                    // Use  the Format method to make a string in  the format
                    // hhtt://weather-scharp.herokuapp.com/photo?city=dallas&state=tx
                    string weatherPhotoUrl = String.Format("{0}photo?city={1}&state={2}", BaseUrl, city, state);
                    string tempFileDirectory = Path.GetTempPath().ToString(); // Directory to save image
                    String weatherFilePath = Path.Combine(tempFileDirectory, "weather_image.jpeg"); // Plus filename to make complete path
                    Debug.WriteLine(weatherFilePath);
                    client.DownloadFile(weatherPhotoUrl, weatherFilePath); //Download from URL
                    weatherImage = Image.FromFile(weatherFilePath);//setting the out parameter
                }
                return true; //Request was made, file was saved, no errors
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace); // To help troubleshoot
                errorMessage = e.Message; // setting the out parameter
                return false; //to inform the caller that ther was an error
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void lblWeather_Click(object sender, EventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();//end program with close method
        }
    }
    
}


