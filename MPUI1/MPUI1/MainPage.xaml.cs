using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MPUI1
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnBtnPage1Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new Page1());

            Button button = (Button)sender;
            //await DisplayAlert("Clicked!",
            //    "The button labeled '" + button.Text + "' has been clicked",
            //    "OK");

            await Navigation.PushAsync(new Page1());
        }
        private async void OnBtnPINScreenClicked(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            //await DisplayAlert("Clicked!",
            //    "The button labeled '" + button.Text + "' has been clicked",
            //    "OK");

            await Navigation.PushAsync(new ScreenPIN());
        }
    }
}
