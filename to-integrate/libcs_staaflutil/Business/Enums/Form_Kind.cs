namespace Common
{
    using System;
    [Flags]
    public enum Form_Kind
    {

        Main_Form = 0x1,

        Unfixed_Modal_Dialog = 0x2,

        Fixed = 0x4,

        Modal_Dialog = Unfixed_Modal_Dialog | Fixed,

        Fixed_Main_Form = Main_Form | Fixed,


    }
}
