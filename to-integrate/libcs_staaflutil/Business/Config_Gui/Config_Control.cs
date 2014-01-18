using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Activation;
using Common;
using Common.Controls;
using Common.Dialogs;
using DTA;
using Fairweather.Service;
using Standardization;
using Versioning;

namespace Config_Gui
{

    class Config_Control : DTA_Tab_Control
    {
        public Config_Control() {

            tab1 = new Tab_1();
            tab2 = new Tab_2();
            tab3 = new Tab_3();

            tab1.Name = "Companies";
            tab2.Name = "Setup";
            tab3.Name = "Program Details";
            this.Setup(tab1, tab2, tab3);
            this.Activate_Tab(2);

            Set_Event_Handlers();

        }


        /// <summary>
        /// Loads the DTA data and prepares the control and its 
        /// children for use
        /// </summary>
        public void Setup() {

            ini = Data.Ini_File;
            helper = new Activation_Helper(ini);

            base.Setup(ini);

            tab1.tabc_settings.Setup(ini);

            version = Ini_Main.Get_Version(ini[DTA_Fields.VERSION]);

            Prepare_Controls();

            Examine_Companies();


            tab2.cb_def_company.SelectedIndex = Default_Company.As_Number - 1;

            Prepare_Tab_3();


            Select_Company(Default_Company);


        }

        void Set_Event_Handlers() {

            tab1.but_add_company.Click += But_add_company_Click;
            tab1.but_change_cred.Click += But_change_cred_Click;
            tab1.cb_company.SelectedIndexChanged += Cb_company_SelectedIndexChanged;

            if (Data.Is_Entry_Screens_Suite) {
                tab1.tabc_settings.But_POS_Settings_advanced.Click += (But_POS_Settings_advanced_Click);
                tab1.tabc_settings.But_Edit_Details_History.Click += (But_Edit_Details_History_Click);
                tab1.tabc_settings.But_TE_Settings_advanced.Click += (But_TE_Settings_advanced_Click);

            }

            tab2.but_upgrade.Click += But_upgrade_Click;

            tab2.cb_def_company.SelectedIndexChanged += Cb_def_company_SelectedIndexChanged;

            tab2.rb_auth_src_internal.CheckedChanged += Auth_src_Internal_RB_CheckedChanged;
            tab2.rb_auth_src_prompt.CheckedChanged += Auth_src_Command_Line_RB_CheckedChanged;

            tab3.but_change_license.Click += But_change_license_Click;
            tab3.but_files_version.Click += But_files_version_Click;

        }





        void Prepare_Tab_3() {

            var name_texts = Data.Module_Names;


            var modules = Triple.Make1(1, 2, 3);

            var name_labels = tab3.Labels_module_name;

            var enabled_labels = tab3.Labels_module_enabled;

            Triple.Apply(name_labels, name_texts,
                        (label, name) =>
                        {
                            label.Text = name + " Enabled:";
                        });




            var right_most = (from name_label in name_labels.To_Array()
                              let vert = name_label.Bounds.Vertex(1)
                              select vert).Max(pt => pt.X);

            Triple.Apply(enabled_labels, modules,
                               (label, module) => label.Text = helper.Module_Enabled(module) ? "YES" : "NO");

            tab3.lab_num_companies.Text = helper.Max_Companies.ToString();

            if (Data.Is_Entry_Screens) {

                name_labels.Third.Visible = false;
                enabled_labels.Third.Visible = false;

            }

            Action<Label> set_location_1 =
           (label) =>
           {
               //label.BackColor = Color.Red;

               label.Bounds = label.Bounds.Move_Edge(2, right_most + 40);
           };

            //Action<Label> set_location_1 =
            //    (label) => label.Bounds = label.Bounds.Move_Edge(2, 230);



            Triple.Apply(enabled_labels, set_location_1);

            set_location_1(tab3.lab_num_companies);


            tab3.lab_serial.Text = "{0}-{1}".spf(Data.Serial_Number_Prefix,
                                                 Activation_Data.Get_Serial(helper.PIN));

            Triple.Apply(
                  Triple.Fmap(name_labels, enabled_labels, (lab1, lab2) => new Pair<Label>(lab1, lab2)),
                   name_texts,
                  (Pair<Label> pair, string name) =>
                  {
                      Pair.Apply(pair, lab =>
                      {
                          lab.Visible = !name.IsNullOrEmpty();
                      });
                  });

        }

        void Prepare_Check_Box(CheckBox chb, Func<bool> get, Action<bool> set) {
            chb.Checked = get();
            chb.CheckedChanged += (_1, _2) => set(chb.Checked);
        }

        void Prepare_Controls() {

            Prepare_Check_Box(tab2.chb_allow_other_companies,
                              () => helper.Allow_Other_Companies,
                              _b => helper.Allow_Other_Companies = _b);

            Prepare_Check_Box(tab2.chb_reset_of_active_users,
                              () => helper.Allow_Reset_Of_Active_Users,
                              _b => helper.Allow_Reset_Of_Active_Users = _b);

            Prepare_Check_Box(tab2.chb_debug,
                              () => helper.Debug,
                              _b => helper.Debug = _b);

            tab2.rb_auth_src_internal.Checked = helper.InternalCredentials;
            tab2.rb_auth_src_prompt.Checked = helper.CommandLineCredentials;

            tab2.lab_ver.Text = Ini_Main.Get_Version(version);

            foreach (var pair in tab3.Tech_Support()) {

                string line;
                if (!ini.Try_Get_Data(pair.First, out line))
                    line = "";

                pair.Second.Text = line;

            }
        }

        void Examine_Companies() {

            tab1.cb_company.Items.Clear();
            tab2.cb_def_company.Items.Clear();

            int max_companies = helper.Max_Companies;

            int current_companies = helper.Number_Of_Companies;

            var general = new General_Helper(ini);

            foreach (var item in general.Company_Strings) {

                tab1.cb_company.Items.Add(item);
                tab2.cb_def_company.Items.Add(item);

            }

            if (max_companies <= current_companies) {
                tab1.but_add_company.Enabled = false;
            }

            tab2.cb_def_company.SelectedIndex = Default_Company.As_Number - 1;
        }

        void Select_Company(Company_Number company_number) {

            Select_Company(company_number, true);

        }

        readonly Block flag1 = new Block();


        void Select_Company(Company_Number company_number, bool force) {

            if (flag1)
                return;

            if (!force && current_company == company_number)
                return;

            if (b_select_company)
                return;

            var prev = current_company;
            string user, pass;
            Company_Number number;

            while (true) {


                string path = null;

                var index = company_number.As_Number - 1;

                // Set the combobox          
                if (tab1.cb_company.SelectedIndex != index) {

                    b_select_company = true;
                    try {
                        tab1.cb_company.SelectedIndex = index;
                    }
                    finally {
                        b_select_company = false;
                    }
                }

                tab1.tabc_settings.Set_Company(company_number, true, true);

                try {
                    using (flag1.Lock())
                        tab1.cb_company.Items[index] = helper.Get_Company_String(company_number);

                    using (var company_helper = helper.Get_Company_Helper(company_number)) {

                        path = company_helper.Company_Path;
                        number = company_helper.Company_Number;
                        pass = company_helper.Password;
                        user = company_helper.Company_Name;

                        var username = company_helper.Username;

                        var period = company_helper.Company_Period;

                        tab1.lab_company_name.Text = user;

                        tab1.lab_company_path.Text = path;

                        tab1.lab_company_number.Text = number.As_String;

                        tab1.lab_username.Text = username;

                        tab1.lab_period.Text = period;
                    }

                    break;
                }
                catch (XSage_Conn ex) {

                    unavailable_companies[company_number] = true;

                    string message = "{0} is unable to connect to the following data folder:\n\n{1}"
                        .spf(Data.App_Name, path);

                    bool invalid_credentials, invalid_directory, deleted_directory;
                    invalid_credentials = invalid_directory = deleted_directory = false;

                    new Dictionary<Sage_Connection_Error, Action>{
                        {Sage_Connection_Error.Invalid_Credentials, () => invalid_credentials = true},
                        {Sage_Connection_Error.Invalid_Folder, () => invalid_directory = true},
                        {Sage_Connection_Error.Folder_Does_Not_Exist, () => deleted_directory = true},
                    }.Get_Or_Default_(ex.Connection_Error, () => { })();

                    Company_Number? new_number = null;

                    if (invalid_credentials) {

                        message += "\n\nThe supplied credentials appear to be invalid.";
                        message += "\n\nWould you like to enter new Sage credentials?";

                        // Ask to supply credentials
                        if (Message_Type.Warning_Yes_No.Show(message) == DialogResult.Yes) {
                            if (Show_Credentials_Form(company_number)) {
                                // retry using same company
                                new_number = company_number;
                            }
                        }
                    }
                    else {
                        if (deleted_directory)
                            message += "\n\nThe directory has been moved or deleted.";

                        else if (invalid_directory)
                            message += "\n\nThe directory is invalid.";

                        Message_Type.Warning.Show(message);
                    }

                    if (new_number == null) {

                        if (prev != null) {
                            new_number = prev;
                        }
                        else if (unavailable_companies.Contains(Default_Company)) {
                            var companies = helper.Companies;

                            var available = companies.Except(unavailable_companies);

                            if (available.Is_Empty())
                                new_number = null;
                            else
                                new_number = available.First();
                        }
                        else {
                            new_number = Default_Company;
                        }

                    }

                    if (new_number == null) {

                        var msg = "{0} is unable to connect to any of the registered Sage Companies.";
                        msg = msg.spf(Data.App_Name);

                        if (helper.Max_Companies > helper.Number_Of_Companies) {

                            msg += "\n\nWould you like to register a new Company?";

                            if (Message_Type.Warning_Yes_No.Show(msg) == DialogResult.Yes) {

                                Show_New_Company_Dialog(out new_number);
                                if (new_number.HasValue)
                                    Examine_Companies();

                            }

                        }
                        else {
                            msg += "\n\nThe program will now close.";

                            Message_Type.Error.Show(msg);
                        }
                    }

                    if (new_number == null) {
                        if (this.Parent is Form_Base)
                            (this.Parent as Form_Base).Close();

                        Application.Exit();
                        return;

                    }
                    else {
                        company_number = new_number.Value;
                        // loop
                    }
                }
            }

            current_company = company_number;
            unavailable_companies[company_number] = false;
            Data.Default_Creds = new Credentials(number, user, pass);

        }

        public void Save_Settings() {
            tab1.tabc_settings.Store_Data();
            Store_Data();
            ini.Write_Data();
        }

        bool Show_Credentials_Form(Company_Number company) {

            var company_string = company.As_String;
            bool ret = false;
            string path = ini[company_string, DTA_Fields.COMPANY_PATH];
            string username = ini[company_string, DTA_Fields.USERNAME];
            string password = ini[company_string, DTA_Fields.PASSWORD];

            var result = new Pair<string>();

            using (var pform = new Password_Form(path, username, password, version)) {

                pform.StartPosition = FormStartPosition.CenterScreen;
                pform.ShowDialog();

                var null_result = pform.Result;

                if (null_result.HasValue) {
                    result = null_result.Value;
                    ret = true;
                }

            }

            if (ret) {
                string user = result.First;
                string pass = result.Second;

                ini[company_string, DTA_Fields.USERNAME] = user;
                ini[company_string, DTA_Fields.PASSWORD] = pass;

                ini.Write_Data();

                tab1.lab_username.Text = user;
            }

            return ret;

        }

        Company_Number Default_Company {
            get {
                return helper.Default_Company;
            }
        }


        // ****************************


        Set<Company_Number> unavailable_companies = new Set<Company_Number>();

        Activation_Helper helper;

        readonly Tab_1 tab1;
        readonly Tab_2 tab2;
        readonly Tab_3 tab3;

        int version;

        bool b_select_company;



        // ****************************


        /*       Plumbing        */


        void But_Edit_Details_History_Click(object sender, EventArgs e) {

            if (current_company == null)
                return;

            var file = Data.Details_History_File(current_company.Value);

            H.Run_Notepad(file, true, false);

        }

        void But_files_version_Click(object sender, EventArgs e) {

            var files = Data.Get_Versioned_Files();
            var custom = Data.Get_Custom_Versions();

            using (var dialog = new Version_Form(files, custom)) {
                dialog.ShowDialog(this);
            }

        }

        void But_POS_Settings_advanced_Click(object sender, EventArgs e) {

            using (var aform = new POS_Advanced_Settings(this.ini, Current_Company)) {
                aform.ShowDialog();
            }

            Border.Instances(tab1.cb_company).First().Refresh();

        }


        void But_TE_Settings_advanced_Click(object sender, EventArgs e) {

            using (var teform = new TE_Advanced_Settings(this.ini, Current_Company)) {
                teform.ShowDialog();
            }

            Border.Instances(tab1.cb_company).First().Refresh();

        }


        void Auth_src_Internal_RB_CheckedChanged(object sender, EventArgs e) {

            helper.InternalCredentials = (sender as RadioButton).Checked;

        }

        void Auth_src_Command_Line_RB_CheckedChanged(object sender, EventArgs e) {

            helper.CommandLineCredentials = (sender as RadioButton).Checked;

        }



        void Cb_def_company_SelectedIndexChanged(object sender, EventArgs e) {

            helper.Default_Company = new Company_Number(tab2.cb_def_company.SelectedIndex + 1);

        }

        void But_upgrade_Click(object sender, EventArgs e) {

            if (Activation_Helpers.Perform_Upgrade(helper))
                Message_Type.Success.Show("Upgrade completed successfully.");

            else

                Message_Type.Error.Show("Upgrade not successful.");
        }

        void But_change_license_Click(object sender, EventArgs e) {

            License_Result result;

            using (var clform = new Change_License_Form()) {

                clform.ShowDialog();

                var null_result = clform.Result;

                if (null_result.HasValue.False())
                    return;

                result = null_result.Value;

            }
            helper.Activate_Application(result, false);

        }

        void Cb_company_SelectedIndexChanged(object sender, EventArgs e) {

            int index = tab1.cb_company.SelectedIndex;
            Select_Company(new Company_Number(index + 1));

        }


        void But_change_cred_Click(object sender, EventArgs e) {

            Show_Credentials_Form(Current_Company);

        }

        void But_add_company_Click(object sender, EventArgs e) {

            Company_Number? number;
            Show_New_Company_Dialog(out number);


            if (number.HasValue) {
                Examine_Companies();
                Select_Company(number.Value);
            }

        }

        void Show_New_Company_Dialog(out Company_Number? null_company) {

            null_company = null;
            Company_Registration_Result result;


            var companies = helper.Number_Of_Companies;
            var max_companies = helper.Max_Companies;

            if (companies >= max_companies) {
                Message_Type.Error.Show("Your license does not allow you to register any more companies.");
                return;
            }


            var registered = (from company in helper.Companies
                              select ini[company.As_String, DTA_Fields.COMPANY_PATH]).ToList();

            using (var company_form = new Add_Company_Form(version, registered)) {

                company_form.ShowDialog(this);

                var null_result = company_form.Result;

                if (!null_result.HasValue)
                    return;

                result = null_result.Value;

            }

            var new_company = new Company_Number(companies + 1);
            null_company = new_company;

            helper.Register_Company(result);


            Message_Type.Success.Show("Company registered successfully.");


        }

    }
}
