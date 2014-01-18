using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Controls;
using Fairweather.Service;


namespace Config_Gui
{
    public class Settings_Control : DTA_Tab_Control
    {


        public Settings_Control() {

            var tabs = Get_Tabs();

            this.Setup(tabs.First(), tabs.Skip(1).arr());

            this.Activate_Tab(0);


        }

        IEnumerable<UserControl> Get_Tabs() {

            if (Data.Is_Entry_Screens_Suite) {
                ses_tab_1 = new Ses_Settings_Tab_1();
                ses_tab_2 = new Ses_Settings_Tab_2();
                ses_tab_3 = new Ses_Settings_Tab_3();
                ses_tab_4 = new Ses_Settings_Tab_4();

                ses_tab_1.Name = "Customer" + "s";// Invoices";
                ses_tab_2.Name = "Supplier" + "s"; // Payments";
                // ses_tab_3.Name = "POS";
                ses_tab_3.Name = "Point of Sales";
                ses_tab_4.Name = "Transactions Entry";

                return new UserControl[] { ses_tab_1, ses_tab_2, ses_tab_3, ses_tab_4 };
            }
            else {
                sit_tab_1 = new Sit_Settings_Tab(null);
                sit_tab_2 = new Sit_Settings_Tab(Record_Type.Sales);
                sit_tab_3 = new Sit_Settings_Tab(Record_Type.Purchase);
                sit_tab_5 = new Sit_Settings_Tab(Record_Type.Expense);
                sit_tab_4 = new Sit_Settings_Tab(Record_Type.Bank);
                sit_tab_6 = new Sit_Settings_Tab(Record_Type.Stock);
                sit_tab_7 = new Sit_Settings_Tab(Record_Type.Invoice_Or_Credit);
                sit_tab_8 = new Sit_Settings_Tab(Record_Type.Audit_Trail);
                //sit_tab_9 = new Sit_Settings_Tab(null);

                sit_tab_1.Name = "General";
                sit_tab_2.Name = "Sales";
                sit_tab_3.Name = "Purchase";
                sit_tab_4.Name = "Nominal";
                sit_tab_5.Name = "Bank";
                sit_tab_6.Name = "Product";
                sit_tab_7.Name = "Invoice or Credit Note";
                sit_tab_8.Name = "Audit Trail";
                //sit_tab_9.Name = "Stock Transactions";

                return new UserControl[] { sit_tab_1, sit_tab_2, sit_tab_3, sit_tab_4, sit_tab_5, sit_tab_6, sit_tab_7, sit_tab_8, /*sit_tab_9*/ };
            }
        }

        Ses_Settings_Tab_1 ses_tab_1;
        Ses_Settings_Tab_2 ses_tab_2;
        Ses_Settings_Tab_3 ses_tab_3;
        Ses_Settings_Tab_4 ses_tab_4;

        public Ses_Settings_Tab_1 Ses_Tab_1 {
            get { return ses_tab_1; }
        }

        public Ses_Settings_Tab_2 Ses_Tab_2 {
            get { return ses_tab_2; }
        }

        public Ses_Settings_Tab_3 Ses_Tab_3 {
            get { return ses_tab_3; }
        }

        public Ses_Settings_Tab_4 Ses_Tab_4 {
            get { return ses_tab_4; }
        }



        public Button But_POS_Settings_advanced {
            get { return ses_tab_3.But_advanced; }
        }
        public Button But_Edit_Details_History {
            get { return ses_tab_4.but_edit; }
        }

        public Button But_TE_Settings_advanced {
            get { return ses_tab_4.but_advanced; }
        }

        Sit_Settings_Tab sit_tab_1;
        Sit_Settings_Tab sit_tab_2;
        Sit_Settings_Tab sit_tab_3;
        Sit_Settings_Tab sit_tab_4;
        Sit_Settings_Tab sit_tab_5;
        Sit_Settings_Tab sit_tab_6;
        Sit_Settings_Tab sit_tab_7;
        Sit_Settings_Tab sit_tab_8;
 

        public Sit_Settings_Tab Sit_Tab_1 { get { return sit_tab_1; } }
        public Sit_Settings_Tab Sit_Tab_2 { get { return sit_tab_2; } }
        public Sit_Settings_Tab Sit_Tab_3 { get { return sit_tab_3; } }
        public Sit_Settings_Tab Sit_Tab_4 { get { return sit_tab_4; } }
        public Sit_Settings_Tab Sit_Tab_5 { get { return sit_tab_5; } }
        public Sit_Settings_Tab Sit_Tab_6 { get { return sit_tab_6; } }
        public Sit_Settings_Tab Sit_Tab_7 { get { return sit_tab_7; } }
        public Sit_Settings_Tab Sit_Tab_8 { get { return sit_tab_8; } }


    }

}