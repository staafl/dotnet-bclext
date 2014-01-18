using System;
using System.Drawing;
using System.Windows.Forms;

using Fairweather.Service;

namespace Common.Controls
{
    public class Moving_Border : Label
    {
        readonly Control m_owner;
        readonly EventHandler m_refresh_border;

        static public void Create(Control owner) {

            var border = new Moving_Border(owner);

        }

        public Moving_Border(Control owner)
            : this(owner, 1, 1, 1, 1) { }


        public Moving_Border(Control owner, int left_off, int top_off, int right_off, int bottom_off) {

            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoSize = false;
            this.BackColor = Color.Transparent;

            m_owner = owner;
            m_owner.Parent.Controls.Add(this);
            this.Parent = m_owner.Parent;

            m_refresh_border =
                (sender, e) =>
                {
                    this.Bounds = m_owner.Bounds.Expand(left_off, top_off, right_off, bottom_off);
                };

            m_refresh_border(null, EventArgs.Empty);
            owner.ParentChanged += owner_ParentChanged;
            owner.SizeChanged += m_refresh_border;
            owner.Move += m_refresh_border;
        }

        void owner_ParentChanged(object sender, EventArgs e) {
            this.Parent = m_owner.Parent;
            m_refresh_border(sender, e);
        }

        protected override void OnEnabledChanged(EventArgs e) {

            // if(b_EnabledChanged)
            //   return;

            if (!this.Enabled)
                this.BackColor = Color.DarkGray;
            else
                this.BackColor = Color.Transparent;

            base.OnEnabledChanged(e);

        }
    }
}
