using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

using Plugin.InAppBilling;
using Plugin.InAppBilling.Abstractions;
using Plugin.StoreReview;
using Plugin.Connectivity;
using Xamarin.Forms.PlatformConfiguration;

namespace CCSXamarin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        int savedDaysInfoCounter = 0;
        int deleteDaysInfoCounter = 0;

        public Settings()
        {
            InitializeComponent();
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
            bottomAd.IsVisible = removeAdsGrid.IsVisible = App.adsEnabled;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            bottomAd.IsVisible = removeAdsGrid.IsVisible = App.adsEnabled;
        }

        void btnSave_Clicked(object sender, EventArgs e)
        {
            int savedDays = 0;
            bool inputError = false;

            try
            {
                savedDays = Convert.ToInt32(txtSavedDays.Text);
            }
            catch
            {
                inputError = false;
            }

            if (inputError || savedDays == 0 || savedDays > 32)
            {
                lblSavedDays.IsVisible = true;
                lblSavedDays.TextColor = Color.Red;
                lblSavedDays.Text = "Please enter a whole number greater than 0 and less than or equal to 32.";
            }
            else
            {
                Application.Current.Properties["dayListLength"] = savedDays;
                App.dayListLength = savedDays;
                lblSavedDays.IsVisible = true;
                lblSavedDays.TextColor = Color.Black;
                lblSavedDays.Text = "Saved!";
            }
        }

        void btnSavedDaysInfo_Clicked(object sender, EventArgs e)
        {
            savedDaysInfoCounter++;

            if (savedDaysInfoCounter % 2 != 0)
            {
                lblSavedDays.IsVisible = true;
                lblSavedDays.TextColor = Color.Gray;
                lblSavedDays.Text = "Enter the amount of days you want the application to save. Once you hit this limit, the oldest day logged will automatically be deleted until the number of days saved equals this limit. This number cannot be greater than 32. The default value is 8.";
            }
            else
            {
                lblSavedDays.IsVisible = false;
                savedDaysInfoCounter = 0;
            }
        }

        void btnSavedDaysInfo_Pressed(object sender, EventArgs e)
        {
            saveDaysInfoGrid.BackgroundColor = Color.Gray;
        }

        void btnSavedDaysInfo_Released(object sender, EventArgs e)
        {
            saveDaysInfoGrid.BackgroundColor = Color.Transparent;
        }
        
        void btnDeleteDays_Clicked(object sender, EventArgs e)
        {
            deletePopUp.IsVisible = true;
        }

        void btnDeleteDaysConfirm_Clicked(object sender, EventArgs e)
        {
            App.dayList.Clear();
            App.dayListPosition = 0;
            deletePopUp.IsVisible = false;
            lblDeleteAllDays.IsVisible = true;
            lblDeleteAllDays.Text = "Deleted successfully.";
        }

        void btnDeleteDaysInfo_Clicked(object sender, EventArgs e)
        {
            deleteDaysInfoCounter++;

            if (deleteDaysInfoCounter % 2 != 0)
            {
                lblDeleteAllDays.IsVisible = true;
                lblDeleteAllDays.Text = "Deletes all previously saved days, as well as data on the food eaten for those days.";
            }
            else
            {
                lblDeleteAllDays.IsVisible = false;
                deleteDaysInfoCounter = 0;
            }
        }

        void btnDeleteDaysInfo_Pressed(object sender, EventArgs e)
        {
            deleteDaysInfoGrid.BackgroundColor = Color.Gray;
        }

        void btnDeleteDaysInfo_Released(object sender, EventArgs e)
        {
            deleteDaysInfoGrid.BackgroundColor = Color.Transparent;
        }

        async void btnRemoveAds_Clicked(object sender, EventArgs e)
        {
            try
            {
                var productId = "noads";

                var connected = await CrossInAppBilling.Current.ConnectAsync();

                if (!connected)
                {
                    //Couldn't connect to billing, could be offline, alert user
                    await DisplayAlert("No Connection", "You are not connected to the internet. Please reconnect and try again.", "OK");
                    return;
                }

                //try to purchase item
                var purchase = await CrossInAppBilling.Current.PurchaseAsync(productId, ItemType.InAppPurchase, "");
                if (purchase == null)
                {
                    //Not purchased, alert the user
                }
                else
                {
                    //Purchased, save this information
                    //var id = purchase.Id;
                    //var token = purchase.PurchaseToken;
                    //var state = purchase.State;

                    Application.Current.Properties["adsEnabled"] = false;
                    removeAdsGrid.IsVisible = false;
                    App.adsEnabled = false;
                    bottomAd.IsVisible = App.adsEnabled;
                }
            }
            catch (InAppBillingPurchaseException ex)
            {
                //Something bad has occurred, alert user
                var message = string.Empty;
                switch (ex.PurchaseError)
                {
                    case PurchaseError.AppStoreUnavailable:
                        message = "Currently the app store seems to be unavailble. Try again later.";
                        break;
                    case PurchaseError.BillingUnavailable:
                        message = "Billing seems to be unavailable, please try again later.";
                        break;
                    case PurchaseError.PaymentInvalid:
                        message = "Payment seems to be invalid, please try again.";
                        break;
                    case PurchaseError.PaymentNotAllowed:
                        message = "Payment does not seem to be enabled/allowed, please try again.";
                        break;
                }
                if (string.IsNullOrWhiteSpace(message))
                {
                    message = "There was an error. Please try again later.";
                }
                await DisplayAlert("Error", message, "OK");
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                await DisplayAlert("Error", "There was an error. Please try again later.", "OK");
                Console.WriteLine(ex);
            }
            finally
            {
                //Disconnect, it is okay if we never connected
                await CrossInAppBilling.Current.DisconnectAsync();
            }
        }

        void btnReview_Clicked(object sender, EventArgs e)
        {
            //TODO: integrate iOS app id
            string iOSAppID = "";
            string androidAppID = "com.companyname.ccsxamarin";

            if (Device.RuntimePlatform == Device.iOS)
            {
                CrossStoreReview.Current.OpenStoreListing(iOSAppID);
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                CrossStoreReview.Current.OpenStoreListing(androidAppID);
            }
        }

        async void btnEmail_Clicked(object sender, EventArgs e)
        {
            List<string> recipients = new List<string>();
            recipients.Add("loganstachdev@gmail.com");
            var message = new EmailMessage
            {
                To = recipients
            };
            await Email.ComposeAsync(message);
        }

        void btnClosePopUp_Clicked(object sender, EventArgs e)
        {
            deletePopUp.IsVisible = false;
        }

        void btnClosePopUp_Pressed(object sender, EventArgs e)
        {
            deletePopUpGrid.BackgroundColor = Color.LightGray;
        }

        void btnClosePopUp_Released(object sender, EventArgs e)
        {
            deletePopUpGrid.BackgroundColor = Color.Transparent;
        }
    }
}