using System.Windows.Forms;


namespace Activation
{
    public class Add_Company_Helper
    {
        public Add_Company_Helper(IAddCompanyForm form) {

            this.tb_pass_1 = form.Tb_Pass1;
            this.tb_pass_2 = form.Tb_Pass2;
            this.tb_path = form.Tb_Path;
            this.tb_user = form.Tb_User;
            this.but_browse = form.But_Browse;
            this.fbd_browser = form.Fbd_Browser;

            this.m_form = form;

        }

        readonly TextBox tb_pass_1;

        readonly TextBox tb_pass_2;

        readonly TextBox tb_path;

        readonly TextBox tb_user;

        readonly Button but_browse;

        readonly FolderBrowserDialog fbd_browser;

        readonly IAddCompanyForm m_form;





    }

}
