using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MPUI1
{
    public interface IDisplaySettings { int GetHeight(); int GetWidth(); }

    class PUILayout
    {
        internal const string PINSCREEN = "PinScreen";
        internal const string NODE_PUI = "pui";
        internal const string NODE_SCREENS = "screens";
        internal const string NODE_SCREEN = "screen";
        internal const string ATTR_NAME = "name";
        internal const string ATTR_ROWS = "rows";
        internal const string ATTR_COLS = "cols";
        internal const string ATTR_X = "x";
        internal const string ATTR_Y = "y";
        internal const string ATTR_HEI = "hei";
        internal const string ATTR_WID = "wid";
        internal const string ATTR_TYPE = "type";
        internal const string ATTR_TEXT = "text";
        internal const string ATTR_FSIZE = "fsize";

        private string xmldoc =
            @"<?xml version='1.0'?>
            <pui>
	            <displaysizes>
		            <size name = 'Tablet' default='true' height='800' width='1280' />
		            <size name = 'iPhone' height='960' width='640' />
		            <size name = 'iPad' height='1536' width='2048' />
	            </displaysizes>
	            <styles>
	            </styles>
                <screens>
   		<screen name = 'PinScreen' basescreen='true' cols='6' rows='8'>
			<widget name = 'lblDesc' hei='1' shrink='10' text='Please enter your PIN number' type='description' wid='3' x='0' y='0' />
			<widget name = 'txtPIN' shrink='10' text='txtPIN' type='description' wid='3' x='0' y='2' />
			<widget name = 'lblDisc' fsize='40' hei='3' shrink='10' 
                text='This HomePod improves the way that your health is monitored, however if you feel unwell, you will need to contact your doctor, or dial 999 in the usual way' 
                type='description' wid='3' x='0' y='3' />
			<widget name = 'kpPIN' fsize='35' hei='7' text='kpPIN' type='keypad' wid='3' x='3' y='0' />
		</screen>
                </screens>
            </pui>
        ";

        private string xmldoc2 =
            @" <?xml version='1.0'?>
            <pui>
	            <displaysizes>
		            <size name = 'Tablet' default='true' height='800' width='1280' />
		            <size name = 'iPhone' height='960' width='640' />
		            <size name = 'iPad' height='1536' width='2048' />
	            </displaysizes>
	            <styles>
	            </styles>
                <screens>
                    <screen name='PinScreen' basescreen='true' cols='6' rows='8'>
			            <widget name = 'lblDesc' hei='1' shrink='10' text='Please enter your PIN number' 
                            type='description' wid='3' x='0' y='0' />
			            <widget name = 'txtPIN' shrink='10' text='txtPIN' type='description' wid='3' x='3' y='3' />
			            <widget name = 'lblDisc' fsize='40' hei='4' shrink='10' 
                            text='This HomePod improves the way that your health is monitored, however if you feel unwell, you will need to contact your doctor, or dial 999 in the usual way' 
                            type='description' wid='3' x='0' y='3' />
			            <widget name = 'kpPIN' fsize='35' hei='7' text='kpPIN' type='keypad' wid='3' x='3' y='0' />
			            <widget name = 'btnOK' fsize='35' hei='2' text='OK' type='button' wid='2' x='3' y='5' />
		            </screen>
                </screens>
            </pui>
        ";

        internal static string GetStringValue(XElement element, string attributeName)
        {
            if (element == null) return String.Empty;

            string value = element.Attribute(attributeName).Value;
            return value;
        }

        internal static int GetIntValueOrDefault(XElement element, string attributeName, int defaultValue)
        {
            int value = defaultValue;
            if (element == null) return value;
            XAttribute attr = element.Attribute(attributeName);
            if (attr == null) return value;
            int.TryParse(attr.Value, out value);
            return value;
        }

        internal static int GetIntValue(XElement element, string attributeName)
        {
            return GetIntValueOrDefault(element, attributeName,-1);
        }

        private Dictionary<string, Screen> screens = new Dictionary<string, Screen>();

        public PUILayout()
        {
        }

        public PUILayout(string xmldoc)
        {
            this.xmldoc = xmldoc;
        }

        public Screen GetPINScreen()
        {
            Screen screen = null;

            if (screens.TryGetValue(PINSCREEN, out screen))
            {
                return screen;
            }

            XElement layout = GetPINScreenLayout();
            screen = Screen.GenerateScreenFromXML(layout);
            screens.Add(PINSCREEN, screen);
            return screen;
        }

        private XElement GetPINScreenLayout()
        {
            return GetScreenLayout(PINSCREEN);
        }

        private XElement GetScreenLayout(string screenName)
        {
            XDocument xml = XDocument.Parse(xmldoc);

            IEnumerable<XElement> screen =
                from elements in xml.Element(NODE_PUI).Element(NODE_SCREENS).Elements(NODE_SCREEN)
                where (string)elements.Attribute(ATTR_NAME) == screenName
                select elements;

            if (screen == null)
            {
                throw new Exception(String.Format("Screen not found in xml: {0}", screenName));
            }

            var screenCount = screen.Count();

            if (screenCount != 1)
            {
                throw new Exception(
                    String.Format("Single screen not found in xml: {0}. Count:{1}", screenName, screenCount)
                    );
            }

            var screenLayout = screen.ElementAt(0);
            System.Diagnostics.Debug.WriteLine(screenLayout.ToString());
            return screenLayout;
        }
    }

    public class Screen
    {
        public string Name { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public IEnumerable Widgets { get; set; }
        public int WidgetCount
        {
            get
            {
                int count = Widgets == null ? 0 : ((List<Widget>)Widgets).Count;
                return count;
            }
        }

        public Screen()
        {
        }

        public Screen(string name, int rows, int columns)
        {
            Name = name;
            Rows = rows;
            Columns = columns;
            Widgets = new List<Widget>();
        }

        public override string ToString()
        {
            return String.Format(
                "Screen Name:{0} Rows:{1} Columns:{2} Widgets:{3}", Name, Rows, Columns, WidgetCount
                );
        }

        private void AddWidget(Widget widget)
        {
            var list = (List<Widget>)Widgets;
            list.Add(widget);
        }

        //Generates screen from XML
        public static Screen GenerateScreenFromXML(XElement element)
        {
            var name = PUILayout.GetStringValue(element, PUILayout.ATTR_NAME);
            var rows = PUILayout.GetIntValue(element, PUILayout.ATTR_ROWS);
            var cols = PUILayout.GetIntValue(element, PUILayout.ATTR_COLS);
            var screen = new Screen(name, rows, cols);

            //Gathers all of the widget elements from the XML
            IEnumerable<XElement> widgetElements =
                from elements in element.Elements("widget")
                select elements;

            //Generates the element for the widget to be places on the screen
            foreach (var widgetElement in widgetElements)
            {
                Widget widget = Widget.GenerateWidgetFromXML(widgetElement);
                screen.AddWidget(widget);
            }

            //returns the coordinates for the widget from above 
            return screen;
        }
    }

    public class Widget
    {
        public string Name { get; set; }
        public string WidgetType { get; set; }
        public string Text { get; set; }
        public int FontSize { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
        public int Top { get; set; }
        public int Bottom { get; set; }

        public static Widget GenerateWidgetFromXML(XElement widgetElement)
        {
            Widget widget = new Widget();
            string name = PUILayout.GetStringValue(widgetElement, PUILayout.ATTR_NAME);
            string type = PUILayout.GetStringValue(widgetElement, PUILayout.ATTR_TYPE);
            string text = PUILayout.GetStringValue(widgetElement, PUILayout.ATTR_TEXT);
            int left = PUILayout.GetIntValue(widgetElement, PUILayout.ATTR_X);
            int top = PUILayout.GetIntValue(widgetElement, PUILayout.ATTR_Y);
            int height = PUILayout.GetIntValueOrDefault(widgetElement, PUILayout.ATTR_HEI,1);
            int width = PUILayout.GetIntValueOrDefault(widgetElement, PUILayout.ATTR_WID,1);
            int fontsize = PUILayout.GetIntValue(widgetElement, PUILayout.ATTR_FSIZE);
            widget.Name = name;
            widget.WidgetType = type;
            widget.Text = text;
            widget.Left = left;
            widget.Top = top;
            widget.Right = left + width;
            widget.Bottom = top + height;
            widget.FontSize = fontsize;
            return widget;
        }

    }

}