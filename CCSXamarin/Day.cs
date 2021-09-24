using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions;
using SQLiteNetExtensions.Attributes;
using Newtonsoft.Json;
using Newtonsoft;
using Xamarin.Forms.Internals;

namespace CCSXamarin
{
    public class Day
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [TextBlob("MainListBlobbed")]
        public ObservableCollection<LoggedFood> mainList { get; set; }
        public string MainListBlobbed { get; set; }

        public bool GoalsSet { get; set; }

        public DateTime date { get; set; }
        public string displayDate { get; set; }

        public int breakfastCalories { get; set; }
        public int lunchCalories { get; set; }
        public int dinnerCalories { get; set; }
        public int snackCalories { get; set; }
        public int exerciseCalories { get; set; }

        public int calorieGoalMin { get; set; }
        public int calorieGoalMax { get; set; }
        public int caloriesEaten { get; set; }
        public int caloriesRemainingMin { get; set; }
        public int caloriesRemainingMax { get; set; }

        public int fatGoalMin { get; set; }
        public int fatGoalMax { get; set; }
        public double fatEaten { get; set; }
        public double fatRemainingMin { get; set; }
        public double fatRemainingMax { get; set; }

        public int carbGoalMin { get; set; }
        public int carbGoalMax { get; set; }
        public double carbsEaten { get; set; }
        public double carbsRemainingMin { get; set; }
        public double carbsRemainingMax { get; set; }

        public int proteinGoalMin { get; set; }
        public int proteinGoalMax { get; set; }
        public double proteinEaten { get; set; }
        public double proteinRemainingMin { get; set; }
        public double proteinRemainingMax { get; set; }
    }
}
