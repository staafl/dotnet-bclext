namespace Common
{
    using System.Windows.Forms;
    public interface ICalculatorTextBox : ITextBox
    {
        /// <summary> Usually returns the object itself
        /// </summary>
        Control Control { get; }

        void NotifyChanged();

        void AcceptChar(char ch);

        /// <summary> Specifies whether the object considers itself a
        /// target / source of calculable information.
        /// If the method returns true, the Text property MUST
        /// only include NUMBERS and one or zero DECIMAL POINTS
        /// or a format exception will be thrown
        /// </summary>
        bool IsCalculable { get; }

    }
}
