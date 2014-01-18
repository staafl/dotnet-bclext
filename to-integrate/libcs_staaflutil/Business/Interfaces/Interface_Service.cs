namespace Common
{

    using Common.Controls;
    using Standardization;

    public static class Interface_Service
    {
        public static void Set_Text(this ICalculatorTextBox box, decimal value) {
            var as_vb = box as Validating_Box;
            if (as_vb != null) {
                as_vb.Value = value;
                return;
            }
            var as_nb = box as Numeric_Box;
            if (as_nb != null) {
                as_nb.Value = value;
                return;

            }
            box.Text = value.ToString(true);
        }
    }
}
