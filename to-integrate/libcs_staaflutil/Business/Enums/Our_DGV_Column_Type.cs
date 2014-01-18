namespace Common
{
    // These indicate only the type of the column. They are connected to,
    // but do not completely define the column's control(s) and its (their)
    // behavior. In many cases, examining the type of the control is recommended,
    // instead of relying on the column type, in order to guarantee consistent
    // behavior.
    // 
    // Many of these values are or have been in the past commented out, not because
    // of their inapplicability, but because the particular occasion called for using
    // of a 'Custom' column, because of more sophisticated logic.
    public enum Our_DGV_Column_Type
    {
        // No control, just the regular DGV cell
        // Normal = 0x1,
        Readonly_Decimal = 0x2,

        NumericBox = 0x4,
        AdvancedComboBox = 0x8,

        ComboBox = 0x10,
        DateTime = 0x20,
        TextBox = 0x40,

        // This assumes that the particular (grid) deriver will take care
        // to select and display the required control(s) on its own
        Custom = 0x80,

    }
}
