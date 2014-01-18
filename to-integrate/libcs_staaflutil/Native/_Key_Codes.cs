namespace Fairweather.Service
{
    using System;
    public struct Key_Codes
    {
        public const Int32 VK_LSHIFT = 0xA0;
        public const Int32 VK_RSHIFT = 0xA1;
        public const Int32 VK_LCONTROL = 0xA2;
        public const Int32 VK_RCONTROL = 0xA3;
        public const Int32 VK_LMENU = 0xA4;
        public const Int32 VK_RMENU = 0xA5;

        public const Int32 VK_LBUTTON = 0x01;
        public const Int32 VK_RBUTTON = 0x02;

        #if EXTENDED_VK
                public const Int32 VK_CANCEL = 0x03;

                /// <summary> NOT contiguous with L & RBUTTON
                /// </summary>
                public const Int32 VK_MBUTTON = 0x04;

                public const Int32 VK_BACK = 0x08;
                public const Int32 VK_TAB = 0x09;
                public const Int32 VK_CLEAR = 0x0C;
                public const Int32 VK_RETURN = 0x0D;
                public const Int32 VK_SHIFT = 0x10;
                public const Int32 VK_CONTROL = 0x11;
                public const Int32 VK_MENU = 0x12;

                public const Int32 VK_PAUSE = 0x13;
                public const Int32 VK_CAPITAL = 0x14;

                public const Int32 VK_KANA = 0x15;

                /// <summary> old name - should be here for compatibility
                /// </summary>
                public const Int32 VK_HANGEUL = 0x15;

                public const Int32 VK_HANGUL = 0x15;
                public const Int32 VK_JUNJA = 0x17;
                public const Int32 VK_FINAL = 0x18;
                public const Int32 VK_HANJA = 0x19;
                public const Int32 VK_KANJI = 0x19;
                public const Int32 VK_ESCAPE = 0x1B;
                public const Int32 VK_CONVERT = 0x1c;
                public const Int32 VK_NOCONVERT = 0x1d;
                public const Int32 VK_SPACE = 0x20;
                public const Int32 VK_PRIOR = 0x21;
                public const Int32 VK_NEXT = 0x22;
                public const Int32 VK_END = 0x23;
                public const Int32 VK_HOME = 0x24;
                public const Int32 VK_LEFT = 0x25;
                public const Int32 VK_UP = 0x26;
                public const Int32 VK_RIGHT = 0x27;
                public const Int32 VK_DOWN = 0x28;
                public const Int32 VK_SELECT = 0x29;
                public const Int32 VK_PRINT = 0x2A;
                public const Int32 VK_EXECUTE = 0x2B;
                public const Int32 VK_SNAPSHOT = 0x2C;
                public const Int32 VK_INSERT = 0x2D;
                public const Int32 VK_DELETE = 0x2E;
                public const Int32 VK_HELP = 0x2F;

                /// <summary> VK_0 thru VK_9 are the same as ASCII '0' thru '9' (0x30; -= 0x39;)
                /// </summary>
                public const Int32 VK_9 = 0x30;

                /// <summary> VK_A thru VK_Z are the same as ASCII 'A' thru 'Z' (0x41; -= 0x5A;)
                /// </summary>
                public const Int32 VK_A = 0x41;
                public const Int32 VK_LWIN = 0x5B;
                public const Int32 VK_RWIN = 0x5C;
                public const Int32 VK_APPS = 0x5D;
                public const Int32 VK_SLEEP = 0x5F;

                public const Int32 VK_NUMPAD0 = 0x60;
                public const Int32 VK_NUMPAD1 = 0x61;
                public const Int32 VK_NUMPAD2 = 0x62;
                public const Int32 VK_NUMPAD3 = 0x63;
                public const Int32 VK_NUMPAD4 = 0x64;
                public const Int32 VK_NUMPAD5 = 0x65;
                public const Int32 VK_NUMPAD6 = 0x66;
                public const Int32 VK_NUMPAD7 = 0x67;
                public const Int32 VK_NUMPAD8 = 0x68;
                public const Int32 VK_NUMPAD9 = 0x69;

                public const Int32 VK_MULTIPLY = 0x6A;
                public const Int32 VK_ADD = 0x6B;
                public const Int32 VK_SEPARATOR = 0x6C;
                public const Int32 VK_SUBTRACT = 0x6D;
                public const Int32 VK_DECIMAL = 0x6E;
                public const Int32 VK_DIVIDE = 0x6F;

                public const Int32 VK_F1 = 0x70;
                public const Int32 VK_F2 = 0x71;
                public const Int32 VK_F3 = 0x72;
                public const Int32 VK_F4 = 0x73;
                public const Int32 VK_F5 = 0x74;
                public const Int32 VK_F6 = 0x75;
                public const Int32 VK_F7 = 0x76;
                public const Int32 VK_F8 = 0x77;
                public const Int32 VK_F9 = 0x78;
                public const Int32 VK_F10 = 0x79;
                public const Int32 VK_F11 = 0x7A;
                public const Int32 VK_F12 = 0x7B;

                public const Int32 VK_F13 = 0x7C;
                public const Int32 VK_F14 = 0x7D;
                public const Int32 VK_F15 = 0x7E;
                public const Int32 VK_F16 = 0x7F;
                public const Int32 VK_F17 = 0x80;
                public const Int32 VK_F18 = 0x81;
                public const Int32 VK_F19 = 0x82;
                public const Int32 VK_F20 = 0x83;
                public const Int32 VK_F21 = 0x84;
                public const Int32 VK_F22 = 0x85;
                public const Int32 VK_F23 = 0x86;
                public const Int32 VK_F24 = 0x87;

                public const Int32 VK_NUMLOCK = 0x90;
                public const Int32 VK_SCROLL = 0x91;

                /* VK_L & VK_R - left and right Alt, Ctrl and Shift virtual keys.*/
                public const Int32 VK_LSHIFT = 0xA0;
                public const Int32 VK_RSHIFT = 0xA1;
                public const Int32 VK_LCONTROL = 0xA2;
                public const Int32 VK_RCONTROL = 0xA3;
                public const Int32 VK_LMENU = 0xA4;
                public const Int32 VK_RMENU = 0xA5;

                public const Int32 VK_EXTEND_BSLASH = 0xE2;
                public const Int32 VK_OEM_102 = 0xE2;
                public const Int32 VK_PROCESSKEY = 0xE5;
                public const Int32 VK_ATTN = 0xF6;
                public const Int32 VK_CRSEL = 0xF7;
                public const Int32 VK_EXSEL = 0xF8;
                public const Int32 VK_EREOF = 0xF9;
                public const Int32 VK_PLAY = 0xFA;
                public const Int32 VK_ZOOM = 0xFB;
                public const Int32 VK_NONAME = 0xFC;
                public const Int32 VK_PA1 = 0xFD;
                public const Int32 VK_OEM_CLEAR = 0xFE;
                public const Int32 VK_SEMICOLON = 0xBA;
                public const Int32 VK_EQUAL = 0xBB;
                public const Int32 VK_COMMA = 0xBC;
                public const Int32 VK_HYPHEN = 0xBD;
                public const Int32 VK_PERIOD = 0xBE;
                public const Int32 VK_SLASH = 0xBF;
                public const Int32 VK_BACKQUOTE = 0xC0;

                public const Int32 VK_BROWSER_BACK = 0xA6;
                public const Int32 VK_BROWSER_FORWARD = 0xA7;
                public const Int32 VK_BROWSER_REFRESH = 0xA8;
                public const Int32 VK_BROWSER_STOP = 0xA9;
                public const Int32 VK_BROWSER_SEARCH = 0xAA;
                public const Int32 VK_BROWSER_FAVORITES = 0xAB;
                public const Int32 VK_BROWSER_HOME = 0xAC;

                public const Int32 VK_VOLUME_MUTE = 0xAD;
                public const Int32 VK_VOLUME_DOWN = 0xAE;
                public const Int32 VK_VOLUME_UP = 0xAF;
                public const Int32 VK_MEDIA_NEXT_TRACK = 0xB0;
                public const Int32 VK_MEDIA_PREV_TRACK = 0xB1;
                public const Int32 VK_MEDIA_STOP = 0xB2;
                public const Int32 VK_MEDIA_PLAY_PAUSE = 0xB3;
                public const Int32 VK_LAUNCH_MAIL = 0xB4;
                public const Int32 VK_LAUNCH_MEDIA_SELECT = 0xB5;
                public const Int32 VK_LAUNCH_APP1 = 0xB6;
                public const Int32 VK_LAUNCH_APP2 = 0xB7;
                public const Int32 VK_LBRACKET = 0xDB;
                public const Int32 VK_BACKSLASH = 0xDC;
                public const Int32 VK_RBRACKET = 0xDD;
                public const Int32 VK_APOSTROPHE = 0xDE;
                public const Int32 VK_OFF = 0xDF;

                public const Int32 VK_DBE_ALPHANUMERIC = 0x0f0;
                public const Int32 VK_DBE_KATAKANA = 0x0f1;
                public const Int32 VK_DBE_HIRAGANA = 0x0f2;
                public const Int32 VK_DBE_SBCSCHAR = 0x0f3;
                public const Int32 VK_DBE_DBCSCHAR = 0x0f4;
                public const Int32 VK_DBE_ROMAN = 0x0f5;
                public const Int32 VK_DBE_NOROMAN = 0x0f6;
                public const Int32 VK_DBE_ENTERWORDREGISTERMODE = 0x0f7;
                public const Int32 VK_DBE_ENTERIMECONFIGMODE = 0x0f8;
                public const Int32 VK_DBE_FLUSHSTRING = 0x0f9;
                public const Int32 VK_DBE_CODEINPUT = 0x0fa;
                public const Int32 VK_DBE_NOCODEINPUT = 0x0fb;
                public const Int32 VK_DBE_DETERMINESTRING = 0x0fc;
                public const Int32 VK_DBE_ENTERDLGCONVERSIONMODE = 0x0fd;
        #endif
    }
}