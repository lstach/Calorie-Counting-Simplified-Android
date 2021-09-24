using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Internals;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Extensions;
using CCSXamarin.Controls;

using Xamarin.Essentials;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace CCSXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LogFood : ContentPage
    {
        //TODO: test open date and time settings on iOS

        int calorieGoalMin;
        int calorieGoalMax;
        public static int caloriesEaten;
        
        public static int caloriesRemainingMin;
        public static int caloriesRemainingMax;

        int fatGoalMin;
        int fatGoalMax;
        public static double fatEaten;
        public static double fatRemainingMin;
        public static double fatRemainingMax;

        int carbGoalMin;
        int carbGoalMax;
        public static double carbsEaten;
        public static double carbsRemainingMin;
        public static double carbsRemainingMax;

        int proteinGoalMin;
        int proteinGoalMax;
        public static double proteinEaten;
        public static double proteinRemainingMin;
        public static double proteinRemainingMax;

        double marginPerHeader;

        Color defaultGridColor = Color.FromHex("#FFFFFF");
        Color selectedGridColor = Color.FromHex("#a8ddff");

        public static List<string> mealStrings = new List<string> { "Breakfast", "Lunch", "Dinner", "Snack"};

        public bool GetGoalData()
        {
            if (Application.Current.Properties.ContainsKey("calorieMin"))
            {
                caloriesRemainingMin = calorieGoalMin = Convert.ToInt32(Application.Current.Properties["calorieMin"]);
                caloriesRemainingMax = calorieGoalMax = Convert.ToInt32(Application.Current.Properties["calorieMax"]);
                fatRemainingMin = fatGoalMin = Convert.ToInt32(Application.Current.Properties["fatMin"]);
                fatRemainingMax = fatGoalMax = Convert.ToInt32(Application.Current.Properties["fatMax"]);
                carbsRemainingMin = carbGoalMin = Convert.ToInt32(Application.Current.Properties["carbsMin"]);
                carbsRemainingMax = carbGoalMax = Convert.ToInt32(Application.Current.Properties["carbsMax"]);
                proteinRemainingMin = proteinGoalMin = Convert.ToInt32(Application.Current.Properties["proteinMin"]);
                proteinRemainingMax = proteinGoalMax = Convert.ToInt32(Application.Current.Properties["proteinMax"]);

                lblBigCalories.Text = lblCaloriesRemaining.Text = lblCalorieGoal.Text = $"{calorieGoalMin}-{calorieGoalMax}";
                lblFatRemaining.Text = lblFatGoal.Text = $"{fatGoalMin}-{fatGoalMax}";
                lblCarbsRemaining.Text = lblCarbGoal.Text = $"{carbGoalMin}-{carbGoalMax}";
                lblProteinRemaining.Text = lblProteinGoal.Text = $"{proteinGoalMin}-{proteinGoalMax}";

                lblCaloriesEaten.Text = $"{caloriesEaten}";
                lblFatEaten.Text = $"{fatEaten}";
                lblCarbsEaten.Text = $"{carbsEaten}";
                lblProteinEaten.Text = $"{proteinEaten}";

                return true;
            }
            else
            {
                return false;
            }
        }

        public int DaySize()
        {
            int size = 0;

            if (size <= 0)
            {
                size = 8;
            }
            return size;
        }

        void UpdateTotals(SavedFood food, int multiplyer)
        {
            caloriesEaten += food.Calories * multiplyer;
            fatEaten += food.Fat * multiplyer;
            carbsEaten += food.Carbs * multiplyer;
            proteinEaten += food.Protein * multiplyer;

            caloriesRemainingMin -= food.Calories * multiplyer;
            caloriesRemainingMax -= food.Calories * multiplyer;
            fatRemainingMin -= food.Fat * multiplyer;
            fatRemainingMax -= food.Fat * multiplyer;
            carbsRemainingMin -= food.Carbs * multiplyer;
            carbsRemainingMax -= food.Carbs * multiplyer;
            proteinRemainingMin -= food.Protein * multiplyer;
            proteinRemainingMax -= food.Protein * multiplyer;

            lblCaloriesEaten.Text = caloriesEaten.ToString();
            lblFatEaten.Text = fatEaten.ToString();
            lblCarbsEaten.Text = carbsEaten.ToString();
            lblProteinEaten.Text = proteinEaten.ToString();

            string displayCaloriesRemaining = $"{caloriesRemainingMin}-{caloriesRemainingMax}";
            string displayFatRemaining = $"{fatRemainingMin}-{fatRemainingMax}";
            string displayCarbsRemaining = $"{carbsRemainingMin}-{carbsRemainingMax}";
            string displayProteinRemaining = $"{proteinRemainingMin}-{proteinRemainingMax}";

            //makes the labels more appealing if the values are negative
            //calories
            if (caloriesRemainingMin <= 0 && caloriesRemainingMax > 0)
            {
                displayCaloriesRemaining = $"0-{caloriesRemainingMax}";
            }
            else if (caloriesRemainingMin < 0 && caloriesRemainingMax < 0)
            {
                displayCaloriesRemaining = caloriesRemainingMax.ToString();
            }
            else if (caloriesRemainingMax == 0)
            {
                displayCaloriesRemaining = "0";
            }

            //fat
            if (fatRemainingMin <= 0 && fatRemainingMax > 0)
            {
                displayFatRemaining = $"0-{fatRemainingMax}";
            }
            else if (fatRemainingMin < 0 && fatRemainingMax < 0)
            {
                displayFatRemaining = fatRemainingMax.ToString();
            }
            else if (fatRemainingMax == 0)
            {
                displayFatRemaining = "0";
            }

            //carbs
            if (carbsRemainingMin <= 0 && carbsRemainingMax > 0)
            {
                displayCarbsRemaining = $"0-{carbsRemainingMax}";
            }
            else if (carbsRemainingMin < 0 && carbsRemainingMax < 0)
            {
                displayCarbsRemaining = carbsRemainingMax.ToString();
            }
            else if (carbsRemainingMax == 0)
            {
                displayCarbsRemaining = "0";
            }

            if (proteinRemainingMin <= 0 && proteinRemainingMax > 0)
            {
                displayProteinRemaining = $"0-{proteinRemainingMax}";
            }
            else if (proteinRemainingMin < 0 && proteinRemainingMax < 0)
            {
                displayProteinRemaining = proteinRemainingMax.ToString();
            }
            else if (proteinRemainingMax == 0)
            {
                displayProteinRemaining = "0";
            }

            lblBigCalories.Text = displayCaloriesRemaining;
            lblCaloriesRemaining.Text = displayCaloriesRemaining;
            lblFatRemaining.Text = displayFatRemaining;
            lblCarbsRemaining.Text = displayCarbsRemaining;
            lblProteinRemaining.Text = displayProteinRemaining;
        }

        void CalculatePercentMacros(double fat, double carbs, double protein)
        {
            //calculates how many calories of each macronutrient have been eaten
            //9 calories in a gram of fat, 4 calories in a gram of carbs, 4 calories in a gram of protein

            double fatCalories = fat * 9;
            double carbCalories = carbs * 4;
            double proteinCalories = protein * 4;
            double caloriesFromMacros = fatCalories + carbCalories + proteinCalories;

            double fatPercent;
            double carbPercent;
            double proteinPercent;

            //if nothing has been eaten, this block does not execute to avoid a divide by 0 error.
            if (caloriesFromMacros > 0)
            {
                fatPercent = Math.Round(fatCalories / caloriesFromMacros * 100, 2);
                carbPercent = Math.Round(carbCalories / caloriesFromMacros * 100, 2);
                proteinPercent = Math.Round(proteinCalories / caloriesFromMacros * 100, 2);
            }
            else
            {
                fatPercent = 0;
                carbPercent = 0;
                proteinPercent = 0;
            }

            lblPercentFat.Text = fatPercent.ToString();
            lblPercentCarbs.Text = carbPercent.ToString();
            lblPercentProtein.Text = proteinPercent.ToString();
        }

        public LogFood()
        {
            InitializeComponent();

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            quickAddMealPicker.ItemsSource = mealStrings;            
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

        async void ShowAd()
        {
            //TODO: add boolean to determine if ads are active
            if (App.adsEnabled)
            {
                App.adCounter++;

                if (App.adCounter >= 4)
                {
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        await DependencyService.Get<IAdmobInterstitialAds>().Display(App.androidInterstitialAdId);
                    }
                    else
                    {
                        //show IOS ad
                    }
                    App.adCounter = 0;
                    App.adJustShown = true;
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //TODO: implement ads and check for them
            bottomAd.IsVisible = App.adsEnabled;

            if (DeviceDisplay.MainDisplayInfo.Density == 1)
            {
                Thickness fatThickness = new Thickness(0, 0, 1, 0);
                fatColumn.Margin = fatThickness;
                fatGoalGrid.Margin = fatThickness;
                fatEatenGrid.Margin = fatThickness;
                fatRemainingGrid.Margin = fatThickness;
            }

            if (!App.goalsSet)
            {
                nullDataPopUp.IsVisible = true;
                lblDate.Text = App.dt.ToString("MMMM d, yyyy");
                
                ObservableCollection<LoggedFood> dummyList = new ObservableCollection<LoggedFood>();

                dummyList.Add(new LoggedFood { Header = "Breakfast", CaloriesHeader = "Calories:", Icon = "\uf7b6", HeaderMargin = new Thickness(0, 0, 0, 75)});
                dummyList.Add(new LoggedFood { Header = "Lunch", CaloriesHeader = "Calories:", Icon = "\uf818", HeaderMargin = new Thickness(0, 0, 0, 75) });
                dummyList.Add(new LoggedFood { Header = "Dinner", CaloriesHeader = "Calories:", Icon = "\uf2e7", HeaderMargin = new Thickness(0, 0, 0, 75) });
                dummyList.Add(new LoggedFood { Header = "Snack", CaloriesHeader = "Calories:", Icon = "\uf5d1", HeaderMargin = new Thickness(0, 0, 0, 75) });
                dummyList.Add(new LoggedFood { Header = "Exercise", CaloriesHeader = "Calories Burned:", Icon = "\uf70c", HeaderMargin = new Thickness(0, 0, 0, 75) });
                lstLoggedFood.ItemsSource = dummyList;
            }
            else
            {
                GetGoalData();
                Read();

                if (App.adJustShown)
                {
                    DayRead(App.dayList[App.dayListPosition]);
                    App.adJustShown = false;
                }

                if (App.savedFood != null)
                {
                    DayRead(App.dayList[App.dayListPosition]);
                    LogSavedFood(App.savedFood);
                }
                else
                {
                    App.dayListPosition = App.dayList.Count - 1;
                }

                lstLoggedFood.ItemsSource = App.dayList[App.dayListPosition].mainList;
                

                App.dayList[App.dayListPosition].mainList[0].Header = "Breakfast";
                App.dayList[App.dayListPosition].mainList[0].CaloriesHeader = "Calories:";
                App.dayList[App.dayListPosition].mainList[0].Icon = "\uf7b6";

                App.dayList[App.dayListPosition].mainList[1].Header = "Lunch";
                App.dayList[App.dayListPosition].mainList[1].CaloriesHeader = "Calories:";
                App.dayList[App.dayListPosition].mainList[1].Icon = "\uf818";

                App.dayList[App.dayListPosition].mainList[2].Header = "Dinner";
                App.dayList[App.dayListPosition].mainList[2].CaloriesHeader = "Calories:";
                App.dayList[App.dayListPosition].mainList[2].Icon = "\uf2e7";

                App.dayList[App.dayListPosition].mainList[3].Header = "Snack";
                App.dayList[App.dayListPosition].mainList[3].CaloriesHeader = "Calories:";
                App.dayList[App.dayListPosition].mainList[3].Icon = "\uf5d1";

                App.dayList[App.dayListPosition].mainList[4].Header = "Exercise";
                App.dayList[App.dayListPosition].mainList[4].CaloriesHeader = "Calories Burned:";
                App.dayList[App.dayListPosition].mainList[4].Icon = "\uf70c";

                ChangeHeaderMargin();

                ResetUI();
            }            
        }

        void content_Focused(object sender, EventArgs e)
        {
            double listViewSpacingHeight = lstLoggedFood.Height - footerGrid.Height - lstLoggedFood.RowHeight;
            marginPerHeader = listViewSpacingHeight / 5;
            ChangeHeaderMargin();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (App.goalsSet)
            {
                DaySave(App.dayList[App.dayListPosition], App.dt.ToString("MMMM d, yyyy"), false, false, true);
            }
        }

        void ResetUI()
        {
            addExercisePopUp.IsVisible = quickAddPopUp.IsVisible = calendarPopUp.IsVisible = false;

            lblQuickAddSelectMeal.Text = "Select Meal";

            quickAddMealPicker.SelectedItem = null;

            lblQuickAddError.IsVisible = lblExerciseError.IsVisible = false;
            txtQuickAddCalories.Text = txtQuickAddFat.Text = txtQuickAddCarbs.Text = txtQuickAddProtein.Text = "";
            txtExerciseName.Text = txtExericseCalories.Text = txtExerciseMinutes.Text = "";
            quickAddMealPicker.SelectedItem = null;
        }
        
        void Read()
        {
            caloriesEaten = 0;
            fatEaten = carbsEaten = proteinEaten = 0;

            if (App.dayList.Count > 0)
            {
                Day lastDay = App.dayList[App.dayList.Count - 1]; //gets most recently logged day in the list

                if (App.dt < DateTime.Parse(lastDay.displayDate))
                {
                    //TODO: popup dialog showing date errors, telling user to either change the date or delete all days
                    dateErrorPopUp.IsVisible = true;
                    lblDateError.Text = $"The date on your system is a date that occurs before a previously saved date. This could be because you changed the date on your system. Please change the date of your system to {lastDay.displayDate}.";
                }

                if (lastDay.displayDate != App.dt.ToString("MMMM d, yyyy"))
                {
                    int daysPassed = Convert.ToInt32((DateTime.Parse(App.dt.ToString("MMMM d, yyyy")) - DateTime.Parse(lastDay.displayDate)).TotalDays);

                    for (int i = 0; i < daysPassed; i++)
                    {
                        Day day = new Day();
                        day.mainList = new ObservableCollection<LoggedFood>();
                        day.date = lastDay.date.AddDays(i + 1);
                        day.displayDate = day.date.ToString("MMMM d, yyyy");
                        DaySave(day, day.displayDate, true, false, false);
                        DayRead(day);
                    }
                }
                else
                {
                    DayRead(lastDay);
                }

                for (int i = 0; i < App.dayList.Count; i++)
                {
                    App.dayList[i].mainList[0].Header = "Breakfast";
                    App.dayList[i].mainList[0].CaloriesHeader = "Calories:";
                    App.dayList[i].mainList[0].Icon = "\uf7b6";

                    App.dayList[i].mainList[1].Header = "Lunch";
                    App.dayList[i].mainList[1].CaloriesHeader = "Calories:";
                    App.dayList[i].mainList[1].Icon = "\uf818";

                    App.dayList[i].mainList[2].Header = "Dinner";
                    App.dayList[i].mainList[2].CaloriesHeader = "Calories:";
                    App.dayList[i].mainList[2].Icon = "\uf2e7";

                    App.dayList[i].mainList[3].Header = "Snack";
                    App.dayList[i].mainList[3].CaloriesHeader = "Calories:";
                    App.dayList[i].mainList[3].Icon = "\uf5d1";

                    App.dayList[i].mainList[4].Header = "Exercise";
                    App.dayList[i].mainList[4].CaloriesHeader = "Calories Burned:";
                    App.dayList[i].mainList[4].Icon = "\uf70c";
                }
            }
            else //generates first day ever logged
            {
                Day day = new Day();
                DaySave(day, App.dt.ToString("MMMM d, yyyy"), true, true, false);
                
                DayRead(day);
            }

            lblDate.Text = App.dayList[App.dayList.Count - 1].displayDate;

            calendar.MinDate = DateTime.Parse(App.dayList[0].displayDate);
            calendar.MaxDate = DateTime.Parse(App.dayList[App.dayList.Count - 1].displayDate);
            if (App.dayList[0].date.Month != App.dayList[App.dayList.Count - 1].date.Month)
            {
                calendar.TitleRightArrowIsEnabled = false;
                calendar.TitleLeftArrowIsEnabled = false;
            }
        }

        void DayRead(Day readDay)
        {
            readDay.mainList[0].Calories = readDay.breakfastCalories;
            readDay.mainList[1].Calories = readDay.lunchCalories;
            readDay.mainList[2].Calories = readDay.dinnerCalories;
            readDay.mainList[3].Calories = readDay.snackCalories;
            readDay.mainList[4].Calories = readDay.exerciseCalories;

            lstLoggedFood.ItemsSource = readDay.mainList;

            lblDate.Text = readDay.displayDate;

            calorieGoalMin = readDay.calorieGoalMin;
            calorieGoalMax = readDay.calorieGoalMax;
            caloriesEaten = readDay.caloriesEaten;
            caloriesRemainingMin = readDay.caloriesRemainingMin;
            caloriesRemainingMax = readDay.caloriesRemainingMax;
            lblCalorieGoal.Text = $"{calorieGoalMin}-{calorieGoalMax}";
            lblCaloriesEaten.Text = caloriesEaten.ToString();
            lblCaloriesRemaining.Text = $"{caloriesRemainingMin}-{caloriesRemainingMax}";
            lblBigCalories.Text = $"{caloriesRemainingMin}-{caloriesRemainingMax}";

            fatGoalMin = readDay.fatGoalMin;
            fatGoalMax = readDay.fatGoalMax;
            fatEaten = readDay.fatEaten;
            fatRemainingMin = readDay.fatRemainingMin;
            fatRemainingMax = readDay.fatRemainingMax;
            lblFatGoal.Text = $"{fatGoalMin}-{fatGoalMax}";
            lblFatEaten.Text = fatEaten.ToString();
            lblFatRemaining.Text = $"{fatRemainingMin}-{fatRemainingMax}";

            carbGoalMin = readDay.carbGoalMin;
            carbGoalMax = readDay.carbGoalMax;
            carbsEaten = readDay.carbsEaten;
            carbsRemainingMin = readDay.carbsRemainingMin;
            carbsRemainingMax = readDay.carbsRemainingMax;
            lblCarbGoal.Text = $"{carbGoalMin}-{carbGoalMax}";
            lblCarbsEaten.Text = carbsEaten.ToString();
            lblCarbsRemaining.Text = $"{carbsRemainingMin}-{carbsRemainingMax}";

            proteinGoalMin = readDay.proteinGoalMin;
            proteinGoalMax = readDay.proteinGoalMax;
            proteinEaten = readDay.proteinEaten;
            proteinRemainingMin = readDay.proteinRemainingMin;
            proteinRemainingMax = readDay.proteinRemainingMax;
            lblProteinGoal.Text = $"{proteinGoalMin}-{proteinGoalMax}";
            lblProteinEaten.Text = proteinEaten.ToString();
            lblProteinRemaining.Text = $"{proteinRemainingMin}-{proteinRemainingMax}";

            CalculatePercentMacros(readDay.fatEaten, readDay.carbsEaten, readDay.proteinEaten);
        }

        void DaySave(Day day, string currentDate, bool addToList, bool changeTime, bool keepMainList)
        {
            day.caloriesRemainingMin = caloriesRemainingMin;
            day.caloriesRemainingMax = caloriesRemainingMax;

            day.fatRemainingMin = fatRemainingMin;
            day.fatRemainingMax = fatRemainingMax;

            day.carbsRemainingMin = carbsRemainingMin;
            day.carbsRemainingMax = carbsRemainingMax;

            day.proteinRemainingMin = proteinRemainingMin;
            day.proteinRemainingMax = proteinRemainingMax;

            if (changeTime)
            {
                day.date = App.dt;
                day.displayDate = currentDate;
            }

            if (keepMainList)
            {
                day.breakfastCalories = day.mainList[0].Calories;
                day.lunchCalories = day.mainList[1].Calories;
                day.dinnerCalories = day.mainList[2].Calories;
                day.snackCalories = day.mainList[3].Calories;
                day.exerciseCalories = day.mainList[4].Calories;
            }
            else
            {
                day.mainList = new ObservableCollection<LoggedFood>();
                day.mainList.Add(new LoggedFood() { Header = "Breakfast", CaloriesHeader = "Calories:", Icon = "\uf7b6" });
                day.mainList.Add(new LoggedFood() { Header = "Lunch", CaloriesHeader = "Calories:", Icon = "\uf818" });
                day.mainList.Add(new LoggedFood() { Header = "Dinner", CaloriesHeader = "Calories:", Icon = "\uf2e7" });
                day.mainList.Add(new LoggedFood() { Header = "Snack", CaloriesHeader = "Calories:", Icon = "\uf5d1" });
                day.mainList.Add(new LoggedFood() { Header = "Exercise", CaloriesHeader = "Calories Burned:", Icon = "\uf70c" });
            }

            day.calorieGoalMin = calorieGoalMin;
            day.calorieGoalMax = calorieGoalMax;
            day.caloriesEaten = caloriesEaten;

            day.fatGoalMin = fatGoalMin;
            day.fatGoalMax = fatGoalMax;
            day.fatEaten = fatEaten;

            day.carbGoalMin = carbGoalMin;
            day.carbGoalMax = carbGoalMax;
            day.carbsEaten = carbsEaten;

            day.proteinGoalMin = proteinGoalMin;
            day.proteinGoalMax = proteinGoalMax;
            day.proteinEaten = proteinEaten;

            //adds the logged day to the list of days if it is a previously unlogged day.

            if (addToList == true)
            {
                App.dayList.Add(day);
                using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
                {
                    conn.CreateTable<Day>();
                    conn.InsertWithChildren(day);
                }
            }
        }


        void btnDateLeft_Clicked(object sender, EventArgs args)
        {
            if (App.dayListPosition > 0)
            {
                EditLastSelectedItem();
                DaySave(App.dayList[App.dayListPosition], App.dayList[App.dayListPosition].displayDate, false, false, true);
                App.dayListPosition--;
                DayRead(App.dayList[App.dayListPosition]);
                ChangeHeaderMargin();
                lstLoggedFood.ItemsSource = App.dayList[App.dayListPosition].mainList;
                lastSelectedItem = null;
                calendar.SelectedDate = App.dayList[App.dayListPosition].date;
            }
        }

        void btnDateRight_Clicked(object sender, EventArgs args)
        {
            if (App.dayListPosition < App.dayList.Count - 1)
            {
                EditLastSelectedItem();
                DaySave(App.dayList[App.dayListPosition], App.dayList[App.dayListPosition].displayDate, false, false, true);
                App.dayListPosition++;
                DayRead(App.dayList[App.dayListPosition]);
                ChangeHeaderMargin();
                lstLoggedFood.ItemsSource = App.dayList[App.dayListPosition].mainList;
                lastSelectedItem = null;
                calendar.SelectedDate = App.dayList[App.dayListPosition].date;
            }
        }

        void btnCalendar_Clicked(object sender, EventArgs args)
        {
            calendarPopUp.IsVisible = true;
        }

        void calendarDate_Selected(object sender, EventArgs e)
        {
            for (int i = 0; i < App.dayList.Count; i++)
            {
                if (DateTime.Parse(App.dayList[i].displayDate) == calendar.SelectedDate)
                {
                    EditLastSelectedItem();
                    DaySave(App.dayList[App.dayListPosition], App.dayList[App.dayListPosition].displayDate, false, false, true);
                    DayRead(App.dayList[i]);
                    App.dayListPosition = i;
                    ChangeHeaderMargin();
                    lstLoggedFood.ItemsSource = App.dayList[App.dayListPosition].mainList;
                    lastSelectedItem = null;
                    calendarPopUp.IsVisible = false;
                    calendar.SelectedDate = App.dayList[i].date;
                }
            }
        }

        void btnQuickAdd_Clicked(object sender, EventArgs args)
        {
            quickAddPopUp.IsVisible = true;
            lblQuickAddSelectMeal.Text = "Select Meal";
        }

        SavedFood lastSelectedItem = new SavedFood();

        void EditLastSelectedItem()
        {
            SavedFood food = (SavedFood)lstLoggedFood.SelectedItem;

            if (lastSelectedItem != null)
            {
                int index;
                SavedFood replacement = lastSelectedItem.copy();
                replacement.MacrosVisible = false;
                replacement.GridColor = default;
                switch (lastSelectedItem.Group)
                {
                    case "Breakfast":
                        index = App.dayList[App.dayListPosition].mainList[0].IndexOf(lastSelectedItem);
                        if (index != App.dayList[App.dayListPosition].mainList[0].IndexOf(food))
                        {
                            App.dayList[App.dayListPosition].mainList[0].RemoveAt(index);
                            App.dayList[App.dayListPosition].mainList[0].Insert(index, replacement);
                        }
                        break;
                    case "Lunch":
                        index = App.dayList[App.dayListPosition].mainList[1].IndexOf(lastSelectedItem);
                        if (index != App.dayList[App.dayListPosition].mainList[1].IndexOf(food))
                        {
                            App.dayList[App.dayListPosition].mainList[1].RemoveAt(index);
                            App.dayList[App.dayListPosition].mainList[1].Insert(index, replacement);
                        }
                        break;
                    case "Dinner":
                        index = App.dayList[App.dayListPosition].mainList[2].IndexOf(lastSelectedItem);
                        if (index != App.dayList[App.dayListPosition].mainList[2].IndexOf(food))
                        {
                            App.dayList[App.dayListPosition].mainList[2].RemoveAt(index);
                            App.dayList[App.dayListPosition].mainList[2].Insert(index, replacement);
                        }
                        break;
                    case "Snack":
                        index = App.dayList[App.dayListPosition].mainList[3].IndexOf(lastSelectedItem);
                        if (index != App.dayList[App.dayListPosition].mainList[3].IndexOf(food))
                        {
                            App.dayList[App.dayListPosition].mainList[3].RemoveAt(index);
                            App.dayList[App.dayListPosition].mainList[3].Insert(index, replacement);
                        }
                        break;
                    case "Exercise":
                        index = App.dayList[App.dayListPosition].mainList[4].IndexOf(lastSelectedItem);
                        if (index != App.dayList[App.dayListPosition].mainList[4].IndexOf(food))
                        {
                            App.dayList[App.dayListPosition].mainList[4].RemoveAt(index);
                            App.dayList[App.dayListPosition].mainList[4].Insert(index, replacement);
                        }
                        break;
                }
            }

            if (food != null)
            {
                if (food.MacrosVisible == false)
                {
                    if (food.Group != "Exercise")
                    {
                        food.MacrosVisible = true;
                    }
                    food.GridColor = selectedGridColor;
                }
                else
                {
                    food.MacrosVisible = false;
                    food.GridColor = defaultGridColor;
                    lstLoggedFood.SelectedItem = null;
                }

                int position;
                switch (food.Group)
                {
                    case "Breakfast":
                        position = App.dayList[App.dayListPosition].mainList[0].IndexOf(food);
                        //this error checking is here because position = -1 but food != null when a food is deleted and the user swaps days
                        if (position >= 0)
                        {
                            App.dayList[App.dayListPosition].mainList[0][position] = food;
                        }
                        break;
                    case "Lunch":
                        position = App.dayList[App.dayListPosition].mainList[1].IndexOf(food);
                        if (position >= 0)
                        {
                            App.dayList[App.dayListPosition].mainList[1][position] = food;
                        }
                        break;
                    case "Dinner":
                        position = App.dayList[App.dayListPosition].mainList[2].IndexOf(food);
                        if (position >= 0)
                        {
                            App.dayList[App.dayListPosition].mainList[2][position] = food;
                        }
                        break;
                    case "Snack":
                        position = App.dayList[App.dayListPosition].mainList[3].IndexOf(food);
                        if (position >= 0)
                        {
                            App.dayList[App.dayListPosition].mainList[3][position] = food;
                        }
                        break;
                    case "Exercise":
                        position = App.dayList[App.dayListPosition].mainList[4].IndexOf(food);
                        if (position >= 0)
                        {
                            App.dayList[App.dayListPosition].mainList[4][position] = food;
                        }
                        break;
                }
                lastSelectedItem = food;
            }
        }

        private void lstLoggedFood_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            EditLastSelectedItem();
        }

        void btnChangeDate_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<IAppSettingsHelper>().OpenAppSettings();
        }

        void btnDeleteAllDays_Clicked(object sender, EventArgs e)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.FilePath))
            {
                conn.CreateTable<Day>();
                conn.DeleteAll<Day>();
            }
            Day day = new Day();
            DaySave(day, App.dt.ToString("MMMM d, yyyy"), true, true, false);
            DayRead(day);
            dateErrorPopUp.IsVisible = false;
        }

        void btnDateLeft_Pressed(object sender, EventArgs e)
        {
            dateLeftGrid.BackgroundColor = Color.FromHex("#1364e8");
        }

        void btnDateLeft_Released(object sender, EventArgs e)
        {
            dateLeftGrid.BackgroundColor = Color.Transparent;
        }

        void btnCalendar_Pressed(object sender, EventArgs e)
        {
            calendarGrid.BackgroundColor = Color.FromHex("#1364e8");
        }

        void btnCalendar_Released(object sender, EventArgs e)
        {
            calendarGrid.BackgroundColor = Color.Transparent;
        }

        void btnDateRight_Pressed(object sender, EventArgs e)
        {
            dateRightGrid.BackgroundColor = Color.FromHex("#1364e8");
        }

        void btnDateRight_Released(object sender, EventArgs e)
        {
            dateRightGrid.BackgroundColor = Color.Transparent;
        }

        async void btnOpenSavedFoodsMenu_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedFoods(false));
        }

        void btnOpenSavedFoodsMenu_Pressed(object sender, EventArgs e)
        {
            openSavedFoodsGrid.BackgroundColor = Color.FromHex("#1364e8");
        }

        void btnOpenSavedFoodsMenu_Released(object sender, EventArgs e)
        {
            openSavedFoodsGrid.BackgroundColor = Color.Transparent;
        }

        void quickAddMealPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblQuickAddSelectMeal.Text = "";
        }

        void lstLoggedFood_Scrolled(object sender, EventArgs e)
        {
            addExerciseGrid.BackgroundColor = Color.Transparent;
            logFoodOpenSavedFoodsGrid.BackgroundColor = Color.FromHex("#1463c4");
            quickAddGrid.BackgroundColor = Color.Green;
            deleteLoggedFoodGrid.BackgroundColor = Color.Red;
        }

        void btnAddExercise_Clicked(object sender, EventArgs e)
        {
            addExercisePopUp.IsVisible = true;
        }

        void btnAddExercise_Pressed(object sender, EventArgs e)
        {
            addExerciseGrid.BackgroundColor = Color.LightGray;
        }

        void btnAddExercise_Released(object sender, EventArgs e)
        {
            addExerciseGrid.BackgroundColor = Color.Transparent;
        }

        void btnAddExercisePopUp_Clicked(object sender, EventArgs e)
        {
            int minutes = 0;
            int calories = 0;
            bool inputError = false;
            string name = txtExerciseName.Text;

            try
            {
                minutes = Convert.ToInt32(txtExerciseMinutes.Text);
                calories = Convert.ToInt32(txtExericseCalories.Text);
            }
            catch
            {
                inputError = true;
            }

            if (name == "")
            {
                lblExerciseError.Text = "Please enter a name.";
                lblExerciseError.IsVisible = true;
            }
            else if (inputError)
            {
                lblExerciseError.Text = "Please enter whole numbers.";
                lblExerciseError.IsVisible = true;
            }
            else
            {
                SavedFood exercise = new SavedFood();
                exercise.Name = name;
                exercise.ServingsOrMinutes = Convert.ToDouble(minutes);
                exercise.Calories = Convert.ToInt32(calories);
                exercise.DisplayServingsOrMinutes = "Minutes:";
                exercise.Group = "Exercise";
                exercise.GridColor = defaultGridColor;

                App.dayList[App.dayListPosition].mainList[4].Add(exercise);
                App.dayList[App.dayListPosition].mainList[4].Calories += exercise.Calories;
                UpdateTotals(exercise, -1);
                addExercisePopUp.IsVisible = false;
                lblExerciseError.IsVisible = false;
                txtExerciseName.Text = txtExerciseMinutes.Text = txtExericseCalories.Text = "";
                ChangeHeaderMargin();
                ShowAd();
            }
        }

        void btnAddExercisePopUp_Pressed(object sender, EventArgs e)
        {
            addExercisePopUpGrid.BackgroundColor = Color.LightGray;
        }

        void btnAddExercisePopUp_Released(object sender, EventArgs e)
        {
            addExercisePopUpGrid.BackgroundColor = Color.Transparent;
        }

        async void btnLogFoodOpenSavedFoods_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SavedFoods(true));
        }

        void LogSavedFood(SavedFood food)
        {
            switch (food.Group)
            {
                case "Breakfast":
                    App.dayList[App.dayListPosition].mainList[0].Add(food);
                    App.dayList[App.dayListPosition].mainList[0].Calories += food.Calories;
                    break;
                case "Lunch":
                    App.dayList[App.dayListPosition].mainList[1].Add(food);
                    App.dayList[App.dayListPosition].mainList[1].Calories += food.Calories;
                    break;
                case "Dinner":
                    App.dayList[App.dayListPosition].mainList[2].Add(food);
                    App.dayList[App.dayListPosition].mainList[2].Calories += food.Calories;
                    break;
                case "Snack":
                    App.dayList[App.dayListPosition].mainList[3].Add(food);
                    App.dayList[App.dayListPosition].mainList[3].Calories += food.Calories;
                    break;
            }
            App.savedFood = null;
            UpdateTotals(food, 1);
            CalculatePercentMacros(fatEaten, carbsEaten, proteinEaten);
            ChangeHeaderMargin();
            ShowAd();
        }

        void btnQuickAddPopUp_Clicked(object sender, EventArgs e)
        {
            int calories = 0;
            double fat;
            double carbs;
            double protein;
            bool inputError = false;

            try
            {
                calories = Convert.ToInt32(txtQuickAddCalories.Text);
            }
            catch
            {
                inputError = true;
            }

            try
            {
                fat = Convert.ToDouble(txtQuickAddFat.Text);
            }
            catch
            {
                fat = 0;
            }

            try
            {
                carbs = Convert.ToDouble(txtQuickAddCarbs.Text);
            }
            catch
            {
                carbs = 0;
            }

            try
            {
                protein = Convert.ToDouble(txtQuickAddProtein.Text);
            }
            catch
            {
                protein = 0;
            }

            if (inputError)
            {
                lblQuickAddError.IsVisible = true;
                lblQuickAddError.Text = "Please enter a whole number in the calories field.";
            }
            else if (quickAddMealPicker.SelectedItem == null)
            {
                lblQuickAddError.IsVisible = true;
                lblQuickAddError.Text = "Please select a meal.";
            }
            else if (fat < 0 || carbs < 0 || protein < 0)
            {
                lblQuickAddError.IsVisible = true;
                lblQuickAddError.Text = "Please only enter numbers greater than or equal to 0.";
            }
            else if (calories < 1)
            {
                lblQuickAddError.IsVisible = true;
                lblQuickAddError.Text = "Please enter a number greater than 0 in the calories field.";
            }
            else
            {
                SavedFood food = new SavedFood();
                food.Name = "Quick Add";
                food.Calories = calories;
                food.Fat = fat;
                food.Carbs = carbs;
                food.Protein = protein;
                food.GridColor = defaultGridColor;
                food.ServingsOrMinutes = 1;
                food.DisplayServingsOrMinutes = "Servings:";
                food.Group = (string)quickAddMealPicker.SelectedItem;

                switch (food.Group)
                {
                    case "Breakfast":
                        App.dayList[App.dayListPosition].mainList[0].Add(food);
                        App.dayList[App.dayListPosition].mainList[0].Calories += food.Calories;
                        break;
                    case "Lunch":
                        App.dayList[App.dayListPosition].mainList[1].Add(food);
                        App.dayList[App.dayListPosition].mainList[1].Calories += food.Calories;
                        break;
                    case "Dinner":
                        App.dayList[App.dayListPosition].mainList[2].Add(food);
                        App.dayList[App.dayListPosition].mainList[2].Calories += food.Calories;
                        break;
                    case "Snack":
                        App.dayList[App.dayListPosition].mainList[3].Add(food);
                        App.dayList[App.dayListPosition].mainList[3].Calories += food.Calories;
                        break;
                }

                UpdateTotals(food, 1);
                CalculatePercentMacros(fatEaten, carbsEaten, proteinEaten);
                ChangeHeaderMargin();
                txtQuickAddCalories.Text = txtQuickAddFat.Text = txtQuickAddCarbs.Text = txtQuickAddProtein.Text = "";
                quickAddMealPicker.SelectedItem = null;
                lblQuickAddError.IsVisible = false;
                lblQuickAddSelectMeal.Text = "Select Meal";
                quickAddPopUp.IsVisible = false;
                ShowAd();
            }
        }

        void btnDeleteLoggedFood_Clicked(object sender, EventArgs args)
        {
            if (lstLoggedFood.SelectedItem != null)
            {
                SavedFood food = (SavedFood)lstLoggedFood.SelectedItem;
                switch (food.Group)
                {
                    case "Breakfast":
                        App.dayList[App.dayListPosition].mainList[0].Remove(food);
                        App.dayList[App.dayListPosition].mainList[0].Calories -= food.Calories;
                        UpdateTotals(food, -1);
                        break;
                    case "Lunch":
                        App.dayList[App.dayListPosition].mainList[1].Remove(food);
                        App.dayList[App.dayListPosition].mainList[1].Calories -= food.Calories;
                        UpdateTotals(food, -1);
                        break;
                    case "Dinner":
                        App.dayList[App.dayListPosition].mainList[2].Remove(food);
                        App.dayList[App.dayListPosition].mainList[2].Calories -= food.Calories;
                        UpdateTotals(food, -1);
                        break;
                    case "Snack":
                        App.dayList[App.dayListPosition].mainList[3].Remove(food);
                        App.dayList[App.dayListPosition].mainList[3].Calories -= food.Calories;
                        UpdateTotals(food, -1);
                        break;
                    case "Exercise":
                        App.dayList[App.dayListPosition].mainList[4].Remove(food);
                        App.dayList[App.dayListPosition].mainList[4].Calories -= food.Calories;
                        UpdateTotals(food, 1);
                        break;
                }
                lastSelectedItem = null;
                CalculatePercentMacros(fatEaten, carbsEaten, proteinEaten);
            }
            ChangeHeaderMargin();
        }

        void btnLogFoodOpenSavedFoods_Pressed(object sender, EventArgs e)
        {
            logFoodOpenSavedFoodsGrid.BackgroundColor = Color.FromHex("#09156b");
        }

        void btnLogFoodOpenSavedFoods_Released(object sender, EventArgs e)
        {
            logFoodOpenSavedFoodsGrid.BackgroundColor = Color.FromHex("#1463c4");
        }

        void btnQuickAdd_Pressed(object sender, EventArgs e)
        {
            quickAddGrid.BackgroundColor = Color.FromHex("#053b17");
        }

        void btnQuickAdd_Released(object sender, EventArgs e)
        {
            quickAddGrid.BackgroundColor = Color.Green;
        }

        void btnCloseQuickAddPopUp_Pressed(object sender, EventArgs e)
        {
            closeQuickAddPopUpGrid.BackgroundColor = Color.LightGray;
        }

        void btnCloseQuickAddPopUp_Released(object sender, EventArgs e)
        {
            closeQuickAddPopUpGrid.BackgroundColor = Color.Transparent;
        }

        void btnDeleteLoggedFood_Pressed(object sender, EventArgs e)
        {   
            deleteLoggedFoodGrid.BackgroundColor = Color.FromHex("#ab0e0e");
        }

        void btnDeleteLoggedFood_Released(object sender, EventArgs e)
        {
            deleteLoggedFoodGrid.BackgroundColor = Color.Red;
        }

        void btnLogFood_Pressed(object sender, EventArgs e)
        {
            logFoodOpenSavedFoodsGrid.BackgroundColor = Color.FromHex("#09156b");
        }

        void btnLogFood_Released(object sender, EventArgs e)
        {
            logFoodOpenSavedFoodsGrid.BackgroundColor = Color.FromHex("#1463c4");
        }

        void btnGreen_Pressed(object sender, EventArgs e)
        {
            quickAddGrid.BackgroundColor = Color.FromHex("#053b17");
        }

        void btnGreen_Released(object sender, EventArgs e)
        {
            quickAddGrid.BackgroundColor = Color.Green;
        }

        void btnAdd_Pressed(object sender, EventArgs e)
        {
            addGrid.BackgroundColor = Color.FromHex("#053b17");
        }

        void btnAdd_Released(object sender, EventArgs e)
        {
            addGrid.BackgroundColor = Color.Green;
        }

        void btnDelete_Pressed(object sender, EventArgs e)
        {
            deleteLoggedFoodGrid.BackgroundColor = Color.FromHex("#ab0e0e");
        }

        void btnDelete_Released(object sender, EventArgs e)
        {
            deleteLoggedFoodGrid.BackgroundColor = Color.Red;
        }

        void mealPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            lblQuickAddSelectMeal.Text = "";
        }

        void btnClosePopUp_Clicked(object sender, EventArgs e)
        {
            ResetUI();
        }

        void btnClosePopUp_Pressed(object sender, EventArgs e)
        {
            closeExercisePopUpGrid.BackgroundColor = Color.LightGray;
            closeQuickAddPopUpGrid.BackgroundColor = Color.LightGray;
            closeCalendarPopUpGrid.BackgroundColor = Color.LightGray;
        }

        void btnClosePopUp_Released(object sender, EventArgs e)
        {
            closeExercisePopUpGrid.BackgroundColor = Color.Transparent;
            closeQuickAddPopUpGrid.BackgroundColor = Color.Transparent;
            closeCalendarPopUpGrid.BackgroundColor = Color.Transparent;
        }

        async void btnNavigateSetGoals_Clicked(object sender, EventArgs e)
        {
            
            Navigation.InsertPageBefore(new SetGoals(), this);
            await Navigation.PopAsync();
        }

        async void btnNavigateCalculate_Clicked(object sender, EventArgs e)
        {
            Navigation.InsertPageBefore(new Calculate(), this);
            await Navigation.PopAsync();
        }

        void ChangeHeaderMargin()
        {
            
            //list of each groupheader without any items
            List<LoggedFood> list = new List<LoggedFood>();
            List<LoggedFood> noItemsList = new List<LoggedFood>();
            list.Add(App.dayList[App.dayListPosition].mainList[0]);
            list.Add(App.dayList[App.dayListPosition].mainList[1]);
            list.Add(App.dayList[App.dayListPosition].mainList[2]);
            list.Add(App.dayList[App.dayListPosition].mainList[3]);
            list.Add(App.dayList[App.dayListPosition].mainList[4]);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Count > 0)
                {
                    list[i].HeaderMargin = new Thickness(0, 0, 0, 0);
                }
                else
                {
                    noItemsList.Add(list[i]);
                }
            }

            if (noItemsList.Count > 0)
            {
                if (marginPerHeader < 50)
                {
                    marginPerHeader = 50;
                }

                for (int i = 0; i < noItemsList.Count; i++)
                {
                    noItemsList[i].HeaderMargin = new Thickness(0, 0, 0, marginPerHeader);
                }
            }
            lstLoggedFood.ItemsSource = list;
        }

        void txtTextCompleted(object sender, EventArgs e)
        {
            Entry entry = sender as Entry;

            if (entry.Id == txtQuickAddCalories.Id)
            {
                txtQuickAddFat.Focus();
            }
            else if (entry.Id == txtQuickAddFat.Id)
            {
                txtQuickAddCarbs.Focus();
            }
            else if (entry.Id == txtQuickAddCarbs.Id)
            {
                txtQuickAddProtein.Focus();
            }
            else if (entry.Id == txtExerciseName.Id)
            {
                txtExerciseMinutes.Focus();
            }
            else if (entry.Id == txtExerciseMinutes.Id)
            {
                txtExericseCalories.Focus();
            }

        }
    }
}