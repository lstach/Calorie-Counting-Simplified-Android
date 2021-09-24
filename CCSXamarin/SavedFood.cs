using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using SQLite;

namespace CCSXamarin
{
    public class SavedFood
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }
        public int Calories { get; set; }
        public double Fat { get; set; }
        public double Carbs { get; set; }
        public double Protein { get; set; }
        public string DisplayServingsOrMinutes { get; set; }
        public double ServingsOrMinutes { get; set; }
        public string ServingSize { get; set; }
        public string Group { get; set; }
        public string ExerciseName { get; set; }

        public bool MacrosVisible { get; set; }

        [Ignore]
        public Color GridColor { get; set; }

        public SavedFood copy()
        {
            SavedFood food = new SavedFood { Name = this.Name, Calories = this.Calories, Fat = this.Fat, Carbs = this.Carbs, Protein = this.Protein, DisplayServingsOrMinutes = this.DisplayServingsOrMinutes, ServingsOrMinutes = this.ServingsOrMinutes, ServingSize = this.ServingSize, Group = this.Group, MacrosVisible = this.MacrosVisible, GridColor = this.GridColor };
            return food;
        }
    }
}
