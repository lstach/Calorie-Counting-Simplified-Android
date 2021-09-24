using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CCSXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppMasterDetailPage : MasterDetailPage
    {
        public static AppMasterDetailPageMasterMenuItem lastSelectedItem = new AppMasterDetailPageMasterMenuItem();

        public AppMasterDetailPage()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            MasterPage.ListView.SelectedItem = new AppMasterDetailPageMasterMenuItem { Id = 0, Title = "Log Food", Icon = "\uf044", TargetType = typeof(LogFood) };

            
        }



        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as AppMasterDetailPageMasterMenuItem;
            if (item == null)
                return;

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page) { BarBackgroundColor = Color.FromHex("#FF1083E8") };
            IsPresented = false;

            AppMasterDetailPageMaster.lastSelectedItem = item;

            MasterPage.ListView.SelectedItem = null;
        }


    }
}