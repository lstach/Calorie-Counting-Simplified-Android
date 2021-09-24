using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using SQLite;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Generic;
using Newtonsoft;
using Newtonsoft.Json;
using SQLiteNetExtensions.Extensions;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using Plugin.Connectivity;

namespace CCSXamarin
{
    public partial class App : Application
    {
        public static string FilePath;
        public static SavedFood savedFood { get; set; }
        public static ObservableCollection<SavedFood> savedFoodsList = new ObservableCollection<SavedFood>();
        public static string strNavPageTitle { get; set; }

        public static List<Day> dayList = new List<Day>();
        public static int dayListPosition { get; set; }
        public static int dayListLength { get; set; }
        public static DateTime dt = DateTime.Now;

        public static string androidInterstitialAdId = "ca-app-pub-4377533152176814/7490958813";
        public static int adCounter = 0;
        public static bool adsEnabled = true;
        public static bool goalsSet = true;
        public static bool adJustShown = false;

        public App(string filePath)
        {
            InitializeComponent();

            MainPage = new AppMasterDetailPage();

            FilePath = filePath;
        }

        protected override void OnStart()
        {
            if (Properties.ContainsKey("dayListLength"))
            {
                dayListLength = Convert.ToInt32(Application.Current.Properties["dayListLength"]);
            }
            else
            {
                dayListLength = 8;
            }

            using (SQLiteConnection conn = new SQLiteConnection(FilePath))
            {
                conn.CreateTable<SavedFood>();
                var food = conn.Table<SavedFood>().ToList();

                for (int i = 0; i < food.Count; i++)
                {
                    food[i].GridColor = Color.FromHex("#FFFFFF");
                    savedFoodsList.Add(food[i]);
                }
            }

            using (SQLiteConnection conn = new SQLiteConnection(FilePath))
            {
                conn.CreateTable<Day>();
                //use this to delete all days
                //TESTING PURPOSES ONLY
                //conn.DeleteAll<Day>();
                dayList = conn.GetAllWithChildren<Day>().ToList();
            }

            if (Properties.ContainsKey("adsEnabled"))
            {
                adsEnabled = Convert.ToBoolean(Properties["adsEnabled"]);
            }
            else if (!CrossConnectivity.Current.IsConnected)
            {
                adsEnabled = false;
            }

            LogFood logFood = new LogFood();
            goalsSet = logFood.GetGoalData();

        }

        protected override void OnSleep()
        {
            if (dayList.Count > 0)
            {
                while (dayList.Count > dayListLength)
                {
                    dayList.RemoveAt(0); 
                }
            }

            using (SQLiteConnection conn = new SQLiteConnection(FilePath))
            {
                conn.DeleteAll<Day>();
            }

            if (goalsSet)
            {
                for (int i = 0; i < dayList.Count; i++)
                {
                    using (SQLiteConnection conn = new SQLiteConnection(FilePath))
                    {
                        conn.CreateTable<Day>();
                        conn.InsertWithChildren(dayList[i]);
                    }
                }
            }
            else
            {
                dayList.Clear();
            }

            LogFood.caloriesEaten = LogFood.caloriesRemainingMin = LogFood.caloriesRemainingMax = 0;
            LogFood.fatEaten = LogFood.fatRemainingMin = LogFood.fatRemainingMax = 0;
            LogFood.carbsEaten = LogFood.carbsRemainingMin = LogFood.carbsRemainingMax = 0;
            LogFood.proteinEaten = LogFood.proteinRemainingMin = LogFood.proteinRemainingMax = 0;
        }

        protected override void OnResume()
        {
            dt = DateTime.Now;
        }
    }
}
