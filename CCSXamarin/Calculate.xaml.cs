using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CCSXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Calculate : ContentPage
    {
        int calorieMin;
        int calorieMax;

        int percentFat;
        int percentCarbs;
        int percentProtein;

        int fatMin;
        int fatMax;

        int carbsMin;
        int carbsMax;

        int proteinMin;
        int proteinMax;

        public Calculate()
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (Application.Current.Properties.ContainsKey("adsEnabled"))
                {
                    App.adsEnabled = Convert.ToBoolean(Application.Current.Properties["adsEnabled"]);
                }
                else
                {
                    App.adsEnabled = true;
                }

                bottomAd.IsVisible = App.adsEnabled;
            }
            else
            {
                App.adsEnabled = false;
            }
            bottomAd.IsVisible = App.adsEnabled;
        }

        

        protected override void OnAppearing()
        {
            base.OnAppearing();
            bottomAd.IsVisible = App.adsEnabled;
        }

        void btnCalculate_Clicked(object sender, EventArgs e)
        {
            bool inputError = false; 

            try
            {
                calorieMin = Convert.ToInt32(txtMinCalories.Text);
                calorieMax = Convert.ToInt32(txtMaxCalories.Text);

                percentFat = Convert.ToInt32(txtPercentFat.Text);
                percentCarbs = Convert.ToInt32(txtPercentCarbs.Text);
                percentProtein = Convert.ToInt32(txtPercentProtein.Text);
            }
            catch
            {
                inputError = true;
            }

            if (inputError || calorieMin < 0 || calorieMax < 0 || percentCarbs < 0 || percentFat < 0 || percentProtein < 0)
            {
                lblError.IsVisible = true;
                lblError.Text = "Please only enter whole numbers greater than 0.";
            }
            else if (percentFat + percentCarbs + percentProtein != 100)
            {
                lblError.IsVisible = true;
                lblError.Text = "Please make sure that your percentages add up to 100.";
            }
            else if (calorieMin > calorieMax)
            {
                lblError.IsVisible = true;
                lblError.Text = "Please make sure that the minimum amount of calories is less than the maximum.";
            }
            else if (!inputError)
            {
                CalculateMacros();

                successPopUp.IsVisible = true;
                lblSuccess.Text = $"Everyday you will eat {calorieMin}-{calorieMax} calories, {fatMin}-{fatMax}g of fat, {carbsMin}-{carbsMax}g of carbs, and {proteinMin}-{proteinMax}g of protein.  Would you like to save these nutrition goals now?";
            }
        }

        void btnSave_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["calorieMin"] = calorieMin;
            Application.Current.Properties["calorieMax"] = calorieMax;
            Application.Current.Properties["fatMin"] = fatMin;
            Application.Current.Properties["fatMax"] = fatMax;
            Application.Current.Properties["carbsMin"] = carbsMin;
            Application.Current.Properties["carbsMax"] = carbsMax;
            Application.Current.Properties["proteinMin"] = proteinMin;
            Application.Current.Properties["proteinMax"] = proteinMax;
            lblGoalsSaved.IsVisible = true;
            App.goalsSet = true;
        }

        void CalculateMacros()
        {
            int minFatCalories = calorieMin / 100 * percentFat;
            int maxFatCalories = calorieMax / 100 * percentFat;
            fatMin = minFatCalories / 9;
            fatMax = maxFatCalories / 9;

            int minCarbCalories = calorieMin / 100 * percentCarbs;
            int maxCarbCalories = calorieMax / 100 * percentCarbs;
            carbsMin = minCarbCalories / 4;
            carbsMax = maxCarbCalories / 4;

            int minProteinCalories = calorieMin / 100 * percentProtein;
            int maxProteinCalories = calorieMax / 100 * percentProtein;
            proteinMin = minProteinCalories / 4;
            proteinMax = maxProteinCalories / 4;

            int fatMinRemainder = minFatCalories % 9;
            int fatMaxRemainder = maxFatCalories % 9;
            int carbsMinRemainder = minCarbCalories % 4;
            int carbsMaxRemainder = maxCarbCalories % 4;
            int proteinMinRemainder = minProteinCalories % 4;
            int proteinMaxRemainder = maxProteinCalories % 4;

            if (fatMinRemainder >= 5)
            {
                fatMin++;
            }

            if (fatMaxRemainder >= 5)
            {
                fatMax++;
            }

            if (carbsMinRemainder >= 5)
            {
                carbsMin++;
            }

            if (carbsMaxRemainder >= 5)
            {
                carbsMax++;
            }

            if (proteinMinRemainder >= 5)
            {
                proteinMin++;
            }

            if (proteinMaxRemainder >= 5)
            {
                proteinMax++;
            }
        }

        void btnCalculate_Pressed(object sender, EventArgs e)
        {
            calculateFrame.BackgroundColor = Color.FromHex("#2460bf");
        }

        void btnCalculate_Released(object sender, EventArgs e)
        {
            calculateFrame.BackgroundColor = Color.FromHex("#FF1083E8");
        }

        void btnHelp_Clicked(object sender, EventArgs e)
        {
            helpPopUp.IsVisible = true;
        }

        void btnHelp_Pressed(object sender, EventArgs e)
        {
            helpFrame.BackgroundColor = Color.FromHex("#2460bf");
        }

        private void btnHelp_Released(object sender, EventArgs e)
        {
            helpFrame.BackgroundColor = Color.FromHex("#FF1083E8");
        }

        void btnClosePopUp_Clicked(object sender, EventArgs e)
        {
            helpPopUp.IsVisible = false;
            successPopUp.IsVisible = false;
        }

        void btnClosePopUp_Pressed(object sender, EventArgs e)
        {
            closeHelpPopUpGrid.BackgroundColor = Color.LightGray;
            closeSuccessPopUpGrid.BackgroundColor = Color.LightGray;
            lblGoalsSaved.IsVisible = false;
        }

        void btnClosePopUp_Released(object sender, EventArgs e)
        {
            closeHelpPopUpGrid.BackgroundColor = Color.Transparent;
            closeSuccessPopUpGrid.BackgroundColor = Color.Transparent;
        }

        async void txt_TextCompleted(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;

            if (entry.Id == txtMinCalories.Id)
            {
                txtMaxCalories.Focus();
            }
            else if (entry.Id == txtMaxCalories.Id)
            {
                txtPercentFat.Focus();
            }
            else if (entry.Id == txtPercentFat.Id)
            {
                txtPercentCarbs.Focus();
            }
            else if (entry.Id == txtPercentCarbs.Id)
            {
                txtPercentProtein.Focus();
            }
            else if (entry.Id == txtPercentProtein.Id)
            {
                await mainScrollViewer.ScrollToAsync(buttonStackPanel.X, buttonStackPanel.Y, true);
            }
        }
    }
}