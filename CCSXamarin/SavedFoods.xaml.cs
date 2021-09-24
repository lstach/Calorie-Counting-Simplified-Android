using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using System.Diagnostics.Tracing;
using Xamarin.Essentials;
using Plugin.Connectivity;

namespace CCSXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SavedFoods : ContentPage
    {
        bool blnFastLog;
        
        Color defaultGridColor = Color.FromHex("#FFFFFF");
        Color selectedGridColor = Color.FromHex("#a8ddff");

        public SavedFoods(bool bln)
        {
            InitializeComponent();
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            blnFastLog = bln;
            Title = "Saved Foods";
            logFoodMealPicker.ItemsSource = LogFood.mealStrings;

            lstSavedFood.ItemsSource = App.savedFoodsList;
            ResetUI();
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

        void ResetUI()
        {
            txtServings.Text = "";
            txtNewFoodName.Text = txtNewFoodCalories.Text = txtNewFoodFat.Text = txtNewFoodCarbs.Text = txtNewFoodProtein.Text = txtServingSize.Text = "";
            lblAddNewFoodError.IsVisible = lblLogSavedFoodError.IsVisible = false;
            lblLogFoodSelectMeal.Text = "Select Meal";
            logSavedFoodPopUp.IsVisible = addNewFoodPopUp.IsVisible = false;
            logFoodMealPicker.SelectedItem = null;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            int position = App.savedFoodsList.IndexOf(lastSavedFoodSelected);
            SavedFood replacement = lastSavedFoodSelected.copy();
            replacement.MacrosVisible = false;
            replacement.GridColor = defaultGridColor;

            if (position >= 0)
            {
                App.savedFoodsList[position] = replacement;
                lastSavedFoodSelected = null;
            }

        }

        void txtTextCompleted(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;

            if (entry.Id == txtNewFoodName.Id)
            {
                txtNewFoodCalories.Focus();
            }
            else if (entry.Id == txtNewFoodCalories.Id)
            {
                txtNewFoodFat.Focus();
            }
            else if (entry.Id == txtNewFoodFat.Id)
            {
                txtNewFoodCarbs.Focus();
            }
            else if (entry.Id == txtNewFoodCarbs.Id)
            {
                txtNewFoodProtein.Focus();
            }
            else if (entry.Id == txtNewFoodProtein.Id)
            {
                txtServingSize.Focus();
            }

        }

        SavedFood lastSavedFoodSelected = new SavedFood();
        void lstSavedFood_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            SavedFood food = (SavedFood)lstSavedFood.SelectedItem;

            //determines what to do with last tapped item
            int position = App.savedFoodsList.IndexOf(lastSavedFoodSelected);
            SavedFood replacement = lastSavedFoodSelected.copy();
            replacement.MacrosVisible = false;
            replacement.GridColor = defaultGridColor;

            if (position != App.savedFoodsList.IndexOf(food) && position >= 0)
            {
                App.savedFoodsList[position] = replacement;
            }

            //determines what to do with current tapped item
            int index = App.savedFoodsList.IndexOf(food);

            if (food.GridColor == defaultGridColor)
            {
                food.MacrosVisible = true;
                food.GridColor = selectedGridColor;
            }
            else
            {
                food.MacrosVisible = false;
                food.GridColor = defaultGridColor;
                lstSavedFood.SelectedItem = null;
            }

            App.savedFoodsList[index] = food;
            lastSavedFoodSelected = food;


            if (lstSavedFood.SelectedItem != null && blnFastLog)
            {
                logSavedFoodPopUp.IsVisible = true;
            }
        }        

        void btnLogSavedFood_Clicked(object sender, EventArgs e)
        {
            if (lstSavedFood.SelectedItem != null)
            {
                logSavedFoodPopUp.IsVisible = true;
            }
        }

        async void btnLogSavedFoodPopUp_Clicked(object sender, EventArgs e)
        {
            if (logFoodMealPicker.SelectedItem != null)
            {
                double servings;
                try
                {
                    servings = Convert.ToDouble(txtServings.Text);
                }
                catch
                {
                    servings = 1;
                }

                //if the user inputs a negative number
                if (servings <= 0)
                {
                    servings = 1;
                }

                SavedFood food = (SavedFood)lstSavedFood.SelectedItem;
                SavedFood foodToLog = food.copy();
                foodToLog.Calories = Convert.ToInt32(food.Calories * servings);
                foodToLog.Fat = Math.Round(food.Fat * servings, 1);
                foodToLog.Carbs = Math.Round(food.Carbs * servings, 1);
                foodToLog.Protein = Math.Round(food.Protein * servings, 1);

                foodToLog.Group = (string)logFoodMealPicker.SelectedItem;
                foodToLog.GridColor = defaultGridColor;
                foodToLog.DisplayServingsOrMinutes = "Servings:";
                foodToLog.ServingsOrMinutes = servings;
                foodToLog.MacrosVisible = false;

                App.savedFood = foodToLog;
                ResetUI();
                await Navigation.PopAsync();
            }
            else
            {
                lblLogSavedFoodError.Text = "Please select a meal.";
                lblLogSavedFoodError.IsVisible = true;
            }
        }

        void btnAddNewFood_Clicked(object sender, EventArgs e)
        {
            addNewFoodPopUp.IsVisible = true; 
        }

        void btnAddNewFood_Pressed(object sender, EventArgs e)
        {
            if (addNewFoodPopUp.IsVisible == false)
            {
                addNewFoodGrid.BackgroundColor = Color.FromHex("#053b17");
            }
            addNewFoodPopUpGrid.BackgroundColor = Color.FromHex("#053b17");
        }

        void btnAddNewFood_Released(object sender, EventArgs e)
        {
            addNewFoodGrid.BackgroundColor = Color.Green;
            addNewFoodPopUpGrid.BackgroundColor = Color.Green;
        }

        void btnLogFood_Pressed(object sender, EventArgs e)
        {
            if (logSavedFoodPopUp.IsVisible == false)
            {
                logSavedFoodGrid.BackgroundColor = Color.FromHex("#09156b");
            }
            logFoodPopUpGrid.BackgroundColor = Color.FromHex("#09156b");
        }

        void btnLogFood_Released(object sender, EventArgs e)
        {
            logSavedFoodGrid.BackgroundColor = Color.FromHex("#1463c4");
            logFoodPopUpGrid.BackgroundColor = Color.FromHex("#1463c4");
        }

        void btnDeleteSavedFood_Clicked(object sender, EventArgs e)
        {
            if (lstSavedFood.SelectedItem != null)
            {
                SavedFood food = (SavedFood)lstSavedFood.SelectedItem;
                App.savedFoodsList.Remove(food);
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.Delete<SavedFood>(food.ID);
                }
            }
        }

        void btnDelete_Pressed(object sender, EventArgs e)
        {
            deleteSavedFoodGrid.BackgroundColor = Color.FromHex("#ab0e0e");
        }

        void btnDelete_Released(object sender, EventArgs e)
        {
            deleteSavedFoodGrid.BackgroundColor = Color.Red;
        }

        void btnAddNewFoodPopUp_Clicked(object sender, EventArgs e)
        {
            bool savedFoodCalorieError = false;
            bool savedFoodMacroError = false;

            int savedFoodCalories = 0;
            double savedFoodFat = 0;
            double savedFoodCarbs = 0;
            double savedFoodProtein = 0;

            try
            {
                savedFoodCalories = Convert.ToInt32(txtNewFoodCalories.Text);
            }
            catch
            {
                savedFoodCalorieError = true;
            }

            try
            {
                savedFoodFat = Convert.ToDouble(txtNewFoodFat.Text);
                savedFoodCarbs = Convert.ToDouble(txtNewFoodCarbs.Text);
                savedFoodProtein = Convert.ToDouble(txtNewFoodProtein.Text);
            }
            catch
            {
                savedFoodMacroError = true;
            }

            if (txtNewFoodName.Text == "")
            {
                lblAddNewFoodError.IsVisible = true;
                lblAddNewFoodError.Text = "Please enter a name.";
            }
            else if (savedFoodCalorieError == true)
            {
                lblAddNewFoodError.Text = "Please only enter whole numbers greater than 0 in the calories field.";
                lblAddNewFoodError.IsVisible = true;
            }
            else if (savedFoodMacroError == true)
            {
                lblAddNewFoodError.Text = "Please only enter whole numbers in the macronutrient fields.";
                lblAddNewFoodError.IsVisible = true;
            }
            else if (savedFoodCalories < 0 || savedFoodFat < 0 || savedFoodCarbs < 0 || savedFoodProtein < 0)
            {
                lblAddNewFoodError.Text = "Please only enter numbers greater than or equal to 0.";
                lblAddNewFoodError.IsVisible = true;
            }
            else
            {
                if (App.savedFoodsList.IndexOf(lastSavedFoodSelected) >= 0)
                {
                    int position = App.savedFoodsList.IndexOf(lastSavedFoodSelected);
                    SavedFood replacement = lastSavedFoodSelected.copy();
                    replacement.MacrosVisible = false;
                    replacement.GridColor = defaultGridColor;

                    App.savedFoodsList[position] = replacement;

                }

                SavedFood food = new SavedFood { Name = txtNewFoodName.Text, Calories = savedFoodCalories, Fat = savedFoodFat, Carbs = savedFoodCarbs, Protein = savedFoodProtein, ServingSize = txtServingSize.Text};
                
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<SavedFood>();
                    conn.Insert(food);
                }
                food.GridColor = selectedGridColor;
                food.MacrosVisible = true;
                lastSavedFoodSelected = food;
                lstSavedFood.SelectedItem = food;
                App.savedFoodsList.Add(food);

                txtNewFoodName.Text = txtNewFoodCalories.Text = txtNewFoodFat.Text = txtNewFoodCarbs.Text = txtNewFoodProtein.Text = txtServingSize.Text = "";
                lblAddNewFoodError.Text = "Select Meal";
                addNewFoodPopUp.IsVisible = false;
            }
        }

        void mealPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblLogFoodSelectMeal.Text = "";
        }

        void btnClosePopUp_Clicked(object sender, EventArgs e)
        {
            ResetUI();
        }

        void btnClosePopUp_Pressed(object sender, EventArgs e)
        {
            closeLogFoodPopUpGrid.BackgroundColor = Color.LightGray;
            closeAddNewFoodPopUpGrid.BackgroundColor = Color.LightGray;
        }

        void btnClosePopUp_Released(object sender, EventArgs e)
        {
            closeLogFoodPopUpGrid.BackgroundColor = Color.Transparent;
            closeAddNewFoodPopUpGrid.BackgroundColor = Color.Transparent;
        }
    }
}