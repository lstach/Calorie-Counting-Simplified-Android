using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace CCSXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMasterDetailPageMaster : ContentPage
    {
        public ListView ListView;

        public static AppMasterDetailPageMasterMenuItem lastSelectedItem = new AppMasterDetailPageMasterMenuItem();

        public AppMasterDetailPageMaster()
        {
            InitializeComponent();

            BindingContext = new AppMasterDetailPageMasterViewModel();
            ListView = MenuItemsListView;
        }

        class AppMasterDetailPageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<AppMasterDetailPageMasterMenuItem> MenuItems { get; set; }

            public AppMasterDetailPageMasterViewModel()
            {
                MenuItems = new ObservableCollection<AppMasterDetailPageMasterMenuItem>(new[]
                {
                    new AppMasterDetailPageMasterMenuItem { Id = 0, Title = "Log Food", Icon ="\uf044", TargetType = typeof(LogFood)},
                    new AppMasterDetailPageMasterMenuItem { Id = 1, Title = "Set Goals", Icon="\uf024", TargetType = typeof(SetGoals)},
                    new AppMasterDetailPageMasterMenuItem { Id = 2, Title = "Calculate Goals", Icon="\uf1ec", TargetType = typeof(Calculate)},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }

        void btnCloseNav_Clicked(object sender, EventArgs e)
        {
            int i = lastSelectedItem.Id;

            switch (i)
            {
                case 0:
                    ListView.SelectedItem = new AppMasterDetailPageMasterMenuItem { Id = 0, Title = "Log Food", Icon = "\uf044", TargetType = typeof(LogFood) };
                    break;
                case 1:
                    ListView.SelectedItem = new AppMasterDetailPageMasterMenuItem { Id = 1, Title = "Set Goals", Icon = "\uf024", TargetType = typeof(SetGoals) };
                    break;
                case 2:
                    ListView.SelectedItem = new AppMasterDetailPageMasterMenuItem { Id = 2, Title = "Calculate Goals", Icon = "\uf1ec", TargetType = typeof(Calculate) };
                    break;
                case 3:
                    ListView.SelectedItem = new AppMasterDetailPageMasterMenuItem { Id = 3, Title = "Settings", TargetType = typeof(Settings) };
                    break;
            }
        }

        void btnCloseNav_Pressed(object sender, EventArgs e)
        {
            closeNavGrid.BackgroundColor = Color.FromHex("#0b50a3");
        }

        void btnCloseNav_Released(object sender, EventArgs e)
        {
            closeNavGrid.BackgroundColor = Color.FromHex("#1867c7");
        }

        void btnSettings_Clicked(object sender, EventArgs e)
        {
            ListView.SelectedItem = new AppMasterDetailPageMasterMenuItem { Id = 3, Title = "Settings", TargetType = typeof(Settings) };
        }

        void btnSettings_Pressed(object sender, EventArgs e)
        {
            settingsGrid.BackgroundColor = Color.LightGray;
        }

        void btnSettings_Released(object sender, EventArgs e)
        {
            settingsGrid.BackgroundColor = Color.Transparent;
        }
    }
}