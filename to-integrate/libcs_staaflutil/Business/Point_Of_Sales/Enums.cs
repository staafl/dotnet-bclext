
namespace Fairweather.Service
{

    public static class Enum_Service
    {
    }

    public enum Bixolon_Font_Type
    {
          /* Fonts A: 
           to get the character size the first digit is multiplied by 12,
           and the second digit is multiplied by 24*/
          FontA1x1,
          FontA1x2,
          FontA2x1,
          FontA2x2,
          FontA2x4,
          FontA4x2,
          FontA4x4,
          FontA4x8,
          FontA8x4,
          FontA8x8,

          // Fonts B: first digit is multiplied by 9,
          //               second digit is multiplied by 17
          FontB1x1,
          FontB1x2,
          FontB2x1,
          FontB2x2,
          FontB2x4,
          FontB4x2,
          FontB4x4,
          FontB4x8,
          FontB8x4,
          FontB8x8,

    };

    public enum HAlignment
    {
        Left,
        Center,
        Right,
    }

    public enum Pos_Printer_Type
    {
        Notepad = 0x1,
        Bixolon_Printer,
        POS_NET_Generic,
    }
    public enum Print_Font_Type
    {
        Generic,
        Bixolon_Font,
        Pos_For_Net_Font,
        GDI,
    }

    public enum Pos_Known_Font
    {

    }

}
