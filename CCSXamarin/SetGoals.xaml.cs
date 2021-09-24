using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace CCSXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SetGoals : ContentPage
    {

        int calorieMin;
        int calorieMax;

        int fatMin;
        int fatMax;

        int carbsMin;
        int carbsMax;

        int proteinMin;
        int proteinMax;
        
        public SetGoals()
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

        void btnSave_Clicked(object sender, EventArgs e)
        {
            bool inputError = false;

            try
            {
                calorieMin = Convert.ToInt32(txtMinCalories.Text);
                calorieMax = Convert.ToInt32(txtMaxCalories.Text);

                fatMin = Convert.ToInt32(txtMinFat.Text);
                fatMax = Convert.ToInt32(txtMaxFat.Text);

                carbsMin = Convert.ToInt32(txtMinCarbs.Text);
                carbsMax = Convert.ToInt32(txtMaxCarbs.Text);

                proteinMin = Convert.ToInt32(txtMaxProtein.Text);
                proteinMax = Convert.ToInt32(txtMaxProtein.Text);
            }
            catch
            {
                inputError = true;
            }

            if (inputError || calorieMin < 0 || calorieMax < 0 || fatMin < 0 || fatMax < 0 || carbsMin < 0 || carbsMax < 0 || proteinMin < 0 || proteinMax < 0)
            {
                lblError.IsVisible = true;
                lblError.Text = "Please only enter whole numbers greater than 0.";
            }
            else if (calorieMin > calorieMax)
            {
                lblError.IsVisible = true;
                lblError.Text = "Please make sure the minimum amount of calories is less than the maximum.";
            }
            else if (fatMin > fatMax || carbsMin > carbsMax || proteinMin > proteinMax)
            {
                lblError.IsVisible = true;
                lblError.Text = "Please make sure the minimum number of grams for any macronutrient is less than the maximum.";
            }
            else if (!inputError)
            {
                Application.Current.Properties["calorieMin"] = calorieMin;
                Application.Current.Properties["calorieMax"] = calorieMax;
                Application.Current.Properties["fatMin"] = fatMin;
                Application.Current.Properties["fatMax"] = fatMax;
                Application.Current.Properties["carbsMin"] = carbsMin;
                Application.Current.Properties["carbsMax"] = carbsMax;
                Application.Current.Properties["proteinMin"] = proteinMin;
                Application.Current.Properties["proteinMax"] = proteinMax;
                successPopUp.IsVisible = true;
                App.goalsSet = true;
            }
        }

        void btnSave_Pressed(object sender, EventArgs e)
        {
            saveFrame.BackgroundColor = Color.FromHex("#2460bf");
        }

        void btnSave_Released(object sender, EventArgs e)
        {
            saveFrame.BackgroundColor = Color.FromHex("#FF1083E8");
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

        async void btnGoToLogFood_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new LogFood(), this);
            await Navigation.PopAsync();
        }

        void btnGoToLogFood_Pressed(object sender, EventArgs e)
        {
            logFoodPopUpGrid.BackgroundColor = Color.FromHex("#09156b");
        }

        void btnGoToLogFood_Released(object sender, EventArgs e)
        {
            logFoodPopUpGrid.BackgroundColor = Color.FromHex("#FF1083E8");
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
                txtMinFat.Focus();
            }
            else if (entry.Id == txtMinFat.Id)
            {
                txtMaxFat.Focus();
            }
            else if (entry.Id == txtMaxFat.Id)
            {
                txtMinCarbs.Focus();
            }
            else if (entry.Id == txtMinCarbs.Id)
            {
                txtMaxCarbs.Focus();
            }
            else if (entry.Id == txtMaxCarbs.Id)
            {
                txtMinProtein.Focus();
            }
            else if (entry.Id == txtMinProtein.Id)
            {
                txtMaxProtein.Focus();
            }
            else if (entry.Id == txtMaxProtein.Id)
            {
                await mainScrollViewer.ScrollToAsync(buttonStackLayout.X, buttonStackLayout.Y, true);
            }
        }
    }
}