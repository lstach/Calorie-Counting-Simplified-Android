using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;


namespace CCSXamarin
{
    public class LoggedFood : ObservableCollection<SavedFood>
    {
        public string Header { get; set; }
        public string Icon { get; set; }
        public Thickness HeaderMargin { get; set; }
        public string CaloriesHeader { get; set; }
        public int Calories { get; set; }
        public ObservableCollection<SavedFood> LoggedFoods => this;
    }
}
