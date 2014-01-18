using System.Drawing;

namespace Standardization
{
    /*       TODO: Make this automatic        */

    public struct Colors
    {
        // public static readonly Color 

        static Color default_button_back_color = Color.Azure;

        public static readonly Color DefaultGray = Color.FromArgb(226, 226, 226);

        public static readonly Color LightGray = Color.LightGray;

        public static readonly Color FormBackground = DefaultGray;

        public static readonly Color FormForeground = Color.Black;

        public static readonly Color SortedColumnBG = Color.FromArgb(230, 230, 230);

        public static readonly Color ColumnBG = Color.White;

        public static readonly Color SelectedRow = Color.FromArgb(240, 240, 242);

        public static readonly Color NormalText = Color.FromArgb(0, 0, 0);

        public struct Buttons
        {
            #region
            public static readonly Color Background = FormBackground;

            public static readonly Color Toggled_Background = Color.FromKnownColor(KnownColor.ButtonHighlight);
            #endregion
        }

        public struct Calendar_Control
        {
            //////public static readonly Color Button_Color = Color.DarkGray;
            //////public static readonly Color Back_Color = FormBackground;

            //////public static readonly Color Cell_Color = Color.Gray;
            //////public static readonly Color Strip_Color = Color.Gray;


            //////public static readonly Color Cell_Selected_Color = Color.DarkGray;
            //////public static readonly Color Title_Color = Color.Black; // used to be Color.Gold
            //////public static readonly Color Month_Color = Color.Black;
            //////public static readonly Color Strip_Text_Color = Color.Black;
            //////public static readonly Color Trailing_Color = Color.DarkGray;
            //////public static readonly Color Weekend_Color = Color.DarkRed;

            ////public static readonly Color Button_Color = Color.DarkGray;
            ////public static readonly Color Back_Color = Color.DarkGray;

            ////public static readonly Color Cell_Color = FormBackground;
            ////public static readonly Color Strip_Color = FormBackground;


            ////public static readonly Color Cell_Selected_Color = Color.Gray;
            ////public static readonly Color Title_Color = Color.Black; // used to be Color.Gold
            ////public static readonly Color Month_Color = Color.Black;
            ////public static readonly Color Strip_Text_Color = Color.Black;
            ////public static readonly Color Trailing_Color = Color.DarkGray;
            ////public static readonly Color Weekend_Color = Color.DarkRed;

            public static readonly Color Button_Color = Color.DarkGray;
            public static readonly Color Back_Color = Color.DarkGray;
            public static readonly Color Cell_Color = Color.LightGray;
            public static readonly Color Cell_Selected_Color = Color.Gray;
            public static readonly Color Title_Color = Color.Black;
            public static readonly Color Month_Color = Color.Black;
            public static readonly Color Strip_Color = Color.Gray;
            public static readonly Color Strip_Text_Color = Color.Gold;
            public static readonly Color Trailing_Color = Color.DarkGray;
            public static readonly Color Weekend_Color = Color.DarkRed;
        }


        public struct CheckBoxes
        {
            #region
            public static readonly Color Normal_Flat_Color = Color.White;
            public static readonly Color Disabled_Flat_Color = Color.Gray;

            #endregion
        }

        public struct Date_Time_Control
        {
            #region
            public static readonly Color ButtonBackColor = default_button_back_color;
            public static readonly Color ButtonDisabledBackColor = Color.Gray;

            #endregion
        }

        public struct FloatingQtyBox
        {
            #region
            public static readonly Color Background = Color.FromArgb(210, 210, 210);
            #endregion
        }

        public struct GridView
        {
            #region
            public static readonly Color Background = Color.White;

            public static readonly Color Red = Color.Red;

            public static readonly Color Black = Color.Black;

            public static readonly Color Blue = Color.Blue;


            public struct OutputOnlyCell
            {

                public static readonly Color SelectedBackground = LightGray;
                public static readonly Color SelectedForeground = Color.Black;

            }

            #endregion

        }

        public struct Keyboard
        {
            #region
            public static readonly Color Back_Color = FormBackground;
            public static readonly Color Key_Color = Color.DarkGray;

            public static readonly Color Border_Color = Color.Black;
            public static readonly Color Text_Color = Color.Black;
            #endregion
        }


        public struct NumericBox
        {
            #region
            public static readonly Color ButtonBackColor = default_button_back_color;
            #endregion
        }

        public struct Our_ListView
        {
            #region
            public readonly static Color Column_Header_Color = Color.Silver;
            public readonly static Color Clicked_Header_Color = Color.Gray;
            #endregion
        }
        public struct Tab_Control
        {
            #region
            public static readonly Color Default_Button_Back_Color = Color.LightGray;
            public static readonly Color Default_Button_Crown = Color.MediumBlue;

            public static readonly Color Default_Back_Color = FormBackground;
            public static readonly Color Default_Border_Color = Color.Black;
            public static readonly Color Default_Highlight_Color = FormBackground;

            public static readonly Color Alternate_Back_Color = Color.LightGray;
            public static readonly Color Alternate_Button_Back_Color = Color.LightGray;

            public static readonly Color Alternate_Highlight_Color = Color.White;
            public static readonly Color Alternate_Button_Crown = Color.FromArgb(0xFF, 0x82, 0x34);
            // Color.FromArgb(227, 145, 79); <-- Sage's brownish orange


            #endregion
        }

        public struct TextBoxes
        {
            #region
            public static readonly Color DefaultForeColor = Color.Black;
            public static readonly Color RedForeColor = Color.FromArgb(255, 0, 0);

            public static readonly Color ReadOnlyBackGround = DefaultGray;
            public static readonly Color NormalBackGround = Color.White;

            //public static readonly Color TextBox_Readonly_BG = Color.FromArgb(224, 224, 224);
            //public static readonly Color TextBox_Normal_BG = Color.White;

            #endregion
        }



    }


}