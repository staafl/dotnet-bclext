using System.Windows.Forms;

namespace Activation
{
    public interface IAddCompanyForm
    {
         TextBox Tb_Pass1 {
            get;
         }

         TextBox Tb_Pass2 {
            get;
         }
                
         TextBox Tb_Path {
            get;
         }
                
         TextBox Tb_User {
            get;
                
         }
        
         Button But_Browse {
            get;
                
         }

         FolderBrowserDialog Fbd_Browser {
            get;
         }
        
    }
}
