using System.Drawing;


namespace Common.Controls
{
    //********************************//
    // Structs
    //********************************//
    public struct Item
    {
        public Item(string key_p,
                    string value_p,
                    int index_p) {

            m_key = key_p;
            m_value = value_p;
            m_index = index_p;
        }

        public string Key { get { return m_key; } }
        public string Value { get { return m_value; } }
        public int Index { get { return m_index; } }

        readonly string m_key;
        readonly string m_value;

        readonly int m_index;

        /// <summary>  Returns the item's index
        /// </summary>
        public static explicit operator int(Item? item) {
            return item.HasValue ? item.Value.Index : -1;
        }
        /// <summary>  Returns the item's Key
        /// </summary>
        public static explicit operator string(Item? item) {
            return item.HasValue ? item.Value.Key : "";
        }

        public override string ToString() {
            return this.Key;
        }
    }

    internal struct ItemBox
    {
        public ItemBox(Rectangle boundary1,
                       Rectangle boundary2,
                       Rectangle total_boundary) {

            m_boundary1 = boundary1;
            m_boundary2 = boundary2;
            m_tot_boundary = total_boundary;

        }

        public RectangleF Boundary1 { get { return m_boundary1; } }
        public RectangleF Boundary2 { get { return m_boundary2; } }
        public RectangleF TotalBoundary { get { return m_tot_boundary; } }

        RectangleF m_boundary1;
        RectangleF m_boundary2;
        RectangleF m_tot_boundary;
    }
}