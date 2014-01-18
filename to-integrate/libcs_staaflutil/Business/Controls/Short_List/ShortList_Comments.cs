using System.Windows.Forms;


namespace Common.Controls
{
    public partial class Our_Short_List : UserControl
    {
        /// NOTE:
        /// <summary>  To be called upon creation and resize
        /// </summary>
        partial void Prepare_Boxes();

        /// NOTE:
        /// <summary>  To be called upon creation and resize
        /// </summary>
        partial void Adjust_Size();

        /// NOTE:
        /// <summary>  To be called upon creation and resize
        /// </summary>
        partial void Adjust_Border();

        /// NOTE: 
        /// <summary>  To be called on resize and construction
        /// </summary>
        partial void Adjust_Scrbar_Height_Location();

        /// NOTE: 
        /// <summary>  To be called on resize and construction
        /// </summary>
        partial void Adjust_Rszbox_Location();

        /// NOTE:
        /// <summary>  To be called when items are added or removed
        /// 
        /// Sets the scrollbar's Minimum and Maximum properties and 
        /// determines whether it's Enabled, depending on the number of 
        /// Items
        /// </summary>
        partial void Refresh_Scrollbar_Settings();

        /// NOTE: 
        /// <summary>  To be called when the user begins dragging the 
        /// box
        /// </summary>
        partial void BeginDragging();

        /// NOTE: 
        /// <summary>  To be called once the use has stopped dragging the
        /// box
        /// </summary>
        partial void FinishDragging();

        /// NOTE:
        /// <summary>  To becalled once the final size has 
        /// been determined
        /// </summary>
        partial void FinishResizing();
    }
}