using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Forms;

namespace MPUI1
{
	public class ScreenPIN : ContentPage
	{
        public ScreenPIN ()
		{
            //int appHeight = App.DisplaySettings.GetHeight();
            //int appWidth = App.DisplaySettings.GetWidth();

            PUILayout puilayout = new PUILayout();
            Screen pinscreen = puilayout.GetPINScreen();
            DisplayAlert("PIN Screen",pinscreen.ToString(),"OK");

            //int gridHeight = (int)(appHeight / pinscreen.Rows);
            //int gridWidth = (int)(appWidth / pinscreen.Columns);
            int gridHeight = 100;
            int gridWidth = 75;

            Grid grid = new Grid();

            for (int i = 0; i < pinscreen.Rows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = gridHeight });
            }
            for (int i = 0; i < pinscreen.Columns; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = gridWidth });
            }

            foreach (Widget widget in pinscreen.Widgets)
            {
                string widgetType = widget.WidgetType;
                string widgetName = widget.Name;
                View pageWidget;

                switch (widgetType)
                {
                    case "description":
                        if (widget.Name.Equals(widget.Text) && widget.Name.StartsWith("txt"))
                        {
                            var entry = new Entry();
                            entry.Placeholder = "Enter your" + widget.Text.Substring(3);
                            entry.BackgroundColor = Color.AliceBlue;
                            entry.Margin = 5;
                            entry.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Entry));
                            pageWidget = entry;
                        }
                        else
                        {
                            var label = new Label();
                            label.Text = widget.Text;
                            label.BackgroundColor = Color.LightGreen;
                            label.Margin = 5;
                            label.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
                            pageWidget = label;
                        }
                        break;
                    case "button":
                        var button = new Button();
                        button.Text = widget.Text;
                        button.BackgroundColor = Color.LightCoral;
                        button.Margin = 5;
                        pageWidget = button;
                        break;
                    default:
                        continue;
                }

                grid.Children.Add(pageWidget, widget.Left, widget.Right, widget.Top, widget.Bottom);
            }

            Content = grid;
        }
    }
}