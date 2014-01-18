using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Common;
using Common.Controls;
using Common.Dialogs;
using Fairweather.Service;
using Standardization;
using System.IO;
namespace Config_Gui
{

    public partial class Tab_Quick : DTA_Tab
    {

        public Tab_Quick() {

            InitializeComponent();

            EventHandler add_click = (_1, _2) =>
                {
                var _file = man.New_Tab_File("New Tab");

                var _id = Enter_Tab("New Tab", _file);

                lb_tabs.SelectedItem = _id;
                lb_tabs.Select_Focus();

            };
            but_add.Click += add_click;
            but_delete.Click += (_1, _2) =>
            {
                var _tab = Selected_Tab;

                if (_tab == null)
                    return;

                var _name = _tab.Name;

                var _file = files[_tab];

                var _path = _file.Path;

                if (_file != null && File.Exists(_path) && !H.Is_File_Empty(_path)) {
                    if (!Standard.Ask("Delete tab '{0}'?".spf(_name)))
                        return;
                }

                lb_tabs.Items.Remove(_tab);
                lb_tabs.Select_Focus();

                man.Delete(_file);
                files.Remove(_tab);


            };

            EventHandler edit_click = (_1, _2) =>
            {
                var _tab = Selected_Tab;

                if (_tab == null)
                    return;

                var _file = files[_tab];

                using (var form = new Edit_Quick_Tab_Form()) {

                    var _null_result = form.Show_And_Collect(_file);

                    if (!_null_result.HasValue)
                        return;

                    var _result = _null_result.Value;

                    _file.Write(_result.Second);

                    var name = _file.Name;

                    var new_name = _result.First;

                    if (name != new_name) {
                        man.Rename(_file, new_name);
                        _tab.Name = new_name;
                        lb_tabs.Refresh_Item(lb_tabs.Items.IndexOf(_tab));

                    }



                }

                lb_tabs.Select_Focus();

            };
            but_edit.Click += edit_click;
            but_preview.Click += (_1, _2) =>
            {
                using (var _form = new Quick_Items_Form(this.FindForm(), () => new System.Drawing.Point(200, 280))) {
                    _form.Setup(dir, _ => true);
                    var _ix = lb_tabs.SelectedIndex;
                    if (_ix != -1)
                        _form.tab_control.Active_Tab = _ix;
                    _form.Clicked += _item =>
                    {
                        Standard.Tell("You have selected item '{0}' with Barcode '{1}'.".spf(_item.Name, _item.Barcode));
                    };
                    _form.ShowDialog(this);
                }
            };
            but_up.Click += (_1, _2) => Move_Tab(true);
            but_down.Click += (_1, _2) => Move_Tab(false);
            lb_tabs.MouseDoubleClick += (_1, _2) => edit_click(null, null);


        }



        public override Dictionary<Control, Ini_Field> Get_Fields() {
            return new Dictionary<Control, Ini_Field>
            {
                //{tb_super_pass, DTA_Fields.POS_super_pass},


            };
        }

        public void Setup(Company_Number company) {
            this.company = company;
            this.dir = Data.Get_Quick_Tabs_Dir(company);
            this.man = new Quick_Tab_Man(dir);
            foreach (var tab in man.Tabs)
                Enter_Tab(tab.Name, tab);

        }



        Tab_Identity Enter_Tab(string name, Quick_Tab_File file) {

            var tab = new Tab_Identity();
            tab.Name = name;

            lb_tabs.Items.Add(tab);

            files.Add(tab, file);

            return tab;

        }



        Company_Number company;
        string dir;
        Quick_Tab_Man man;
        Button but_preview;

        Tab_Identity Selected_Tab {
            get {
                return lb_tabs.SelectedItem as Tab_Identity;

            }
        }

        void Move_Tab(bool up) {

            var tab = Selected_Tab;

            if (tab == null)
                return;

            var ix = lb_tabs.Items.IndexOf(tab);
            
            var new_ix = ix + (up ? -1 : 1);

            if (new_ix < 0 || new_ix >= lb_tabs.Items.Count)
                return;

            var file = files[tab];

            files.Move(tab, up ? -1 : 1);
            man.Reorder(file, up ? -1 : 1);

            lb_tabs.Items.RemoveAt(ix);
            lb_tabs.Items.Insert(new_ix, tab);
            lb_tabs.SelectedItem = tab;
            lb_tabs.Select_Focus();

        }



        /* tab <-> file */
        readonly Insord<Tab_Identity, Quick_Tab_File>
        files = new Insord<Tab_Identity, Quick_Tab_File>();


        class Custom_List_Box : ListBox
        {
            public Custom_List_Box() : base() { }
            public void Refresh_Item(int ix) {
                if (ix < 0)
                    return;
                base.RefreshItem(ix); Refresh();
            }
        }
        class Tab_Identity
        {
            public Tab_Identity() { }
            public string Name { get; set; }
            public override string ToString() {
                return Name;
            }
        }

        Custom_List_Box lb_tabs;

        #region designer

        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private Button but_add;
        private Button but_edit;
        private Button but_delete;
        private Button but_up;
        private Button but_down;

        System.ComponentModel.IContainer components = null;

        void InitializeComponent() {
            this.but_add = new System.Windows.Forms.Button();
            this.but_edit = new System.Windows.Forms.Button();
            this.but_delete = new System.Windows.Forms.Button();
            this.but_up = new System.Windows.Forms.Button();
            this.but_down = new System.Windows.Forms.Button();
            this.lb_tabs = new Config_Gui.Tab_Quick.Custom_List_Box();
            this.but_preview = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // but_add
            // 
            this.but_add.Location = new System.Drawing.Point(259, 17);
            this.but_add.Name = "but_add";
            this.but_add.Size = new System.Drawing.Size(75, 23);
            this.but_add.TabIndex = 1;
            this.but_add.Text = "&Add";
            this.but_add.UseVisualStyleBackColor = true;
            // 
            // but_edit
            // 
            this.but_edit.Location = new System.Drawing.Point(259, 46);
            this.but_edit.Name = "but_edit";
            this.but_edit.Size = new System.Drawing.Size(75, 23);
            this.but_edit.TabIndex = 2;
            this.but_edit.Text = "&Edit...";
            this.but_edit.UseVisualStyleBackColor = true;
            // 
            // but_delete
            // 
            this.but_delete.Location = new System.Drawing.Point(259, 75);
            this.but_delete.Name = "but_delete";
            this.but_delete.Size = new System.Drawing.Size(75, 23);
            this.but_delete.TabIndex = 3;
            this.but_delete.Text = "De&lete";
            this.but_delete.UseVisualStyleBackColor = true;
            // 
            // but_up
            // 
            this.but_up.Location = new System.Drawing.Point(259, 104);
            this.but_up.Name = "but_up";
            this.but_up.Size = new System.Drawing.Size(75, 23);
            this.but_up.TabIndex = 4;
            this.but_up.Text = "&Up";
            this.but_up.UseVisualStyleBackColor = true;
            // 
            // but_down
            // 
            this.but_down.Location = new System.Drawing.Point(259, 133);
            this.but_down.Name = "but_down";
            this.but_down.Size = new System.Drawing.Size(75, 23);
            this.but_down.TabIndex = 5;
            this.but_down.Text = "&Down";
            this.but_down.UseVisualStyleBackColor = true;
            // 
            // lb_tabs
            // 
            this.lb_tabs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lb_tabs.FormattingEnabled = true;
            this.lb_tabs.Location = new System.Drawing.Point(10, 16);
            this.lb_tabs.Name = "lb_tabs";
            this.lb_tabs.Size = new System.Drawing.Size(243, 262);
            this.lb_tabs.TabIndex = 0;
            // 
            // but_preview
            // 
            this.but_preview.Location = new System.Drawing.Point(259, 162);
            this.but_preview.Name = "but_preview";
            this.but_preview.Size = new System.Drawing.Size(75, 23);
            this.but_preview.TabIndex = 5;
            this.but_preview.Text = "&Preview...";
            this.but_preview.UseVisualStyleBackColor = true;
            // 
            // Tab_Quick
            // 
            this.Controls.Add(this.lb_tabs);
            this.Controls.Add(this.but_preview);
            this.Controls.Add(this.but_down);
            this.Controls.Add(this.but_up);
            this.Controls.Add(this.but_delete);
            this.Controls.Add(this.but_edit);
            this.Controls.Add(this.but_add);
            this.Name = "Tab_Quick";
            this.Size = new System.Drawing.Size(416, 337);
            this.ResumeLayout(false);

        }

        #endregion



    }
}

