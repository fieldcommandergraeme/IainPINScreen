using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MPUI1
{
    public class Page1 : ContentPage
    {
        private Button btnOK = null;
        private Entry txtPIN = null;

        public Page1()
        {
            Grid grid = new Grid();

            for (int i = 0; i < 40; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Star });
            }
            for (int i = 0; i < 40; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
            }

            var title = new Label();
            title.Text = "Title";
            title.BackgroundColor = Color.Green;
            title.HorizontalTextAlignment = TextAlignment.Center;

            var message = new Label();
            message.Text = "Loremx ipsum dolor sit amet, consectetur adipiscing elit. Nulla sed sodales nulla. In ornare pellentesque lectus a aliquet. Maecenas gravida nec justo imperdiet imperdiet. Nulla facilisi.";
            message.HorizontalTextAlignment = TextAlignment.Start;

            var image = new Image();
            image.Source = ImageSource.FromFile("fractal.jpg");
            image.Aspect = Aspect.AspectFill;

            var txtName = new Entry();
            txtName.Placeholder = "Enter last name";
            txtName.Keyboard = Keyboard.Plain;
            txtName.TextChanged += OnNameChanged;

            txtPIN = new Entry();
            txtPIN.Placeholder = "Enter PIN number";
            txtPIN.Keyboard = Keyboard.Numeric;
            txtPIN.TextChanged += OnPINChanged;
            txtPIN.IsPassword = true;
            txtPIN.Completed += (s, e) =>
            {
                txtPIN.IsPassword = true;
                txtPIN.Keyboard = Keyboard.Numeric;
            };

            btnOK = new Button();
            btnOK.Text = "OK";
            btnOK.IsEnabled = false;

            Button btnBack = new Button();
            btnBack.Text = "Back";
            btnBack.Clicked += OnBackClicked;

            grid.Children.Add(title, 1, 38, 1, 5);
            grid.Children.Add(message, 1, 19, 6, 20);
            grid.Children.Add(image, 21, 38, 6, 26);
            grid.Children.Add(txtName, 1, 19, 21, 25);
            grid.Children.Add(txtPIN, 1, 10, 27, 31);
            grid.Children.Add(btnOK, 30, 38, 34, 38);
            grid.Children.Add(btnBack, 1, 10, 34, 38);

            Content = grid;

        }

        private void OnPINChanged(object sender, TextChangedEventArgs e)
        {
            var value = e.NewTextValue;

            if (String.IsNullOrWhiteSpace(value))
            {
                return;
            }

            if (value.Equals("111"))
            {
                txtPIN.Text = "";
                txtPIN.IsPassword = false;
                txtPIN.Keyboard = Keyboard.Telephone;
            }
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void OnNameChanged(object sender, TextChangedEventArgs e)
        {
            var text = e.NewTextValue;

            if (String.IsNullOrWhiteSpace(text))
            {
                btnOK.IsEnabled = false;
            }
            else
            {
                btnOK.IsEnabled = true;
            }
        }
    }
}