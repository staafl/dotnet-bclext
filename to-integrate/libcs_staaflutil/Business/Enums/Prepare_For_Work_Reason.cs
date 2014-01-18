namespace Common
{
    public enum Prepare_For_Work_Reason
    {
        Launching,
        Discard_or_Reload,
        Posting,
        Posting_Credit_Note,
        Recall,
        /// <summary>
        /// Indicates that an account has been selected (throught the combo box or 
        /// through an auxiliary dialog, etc)
        /// </summary>
        Client_Selected,
    }
}
