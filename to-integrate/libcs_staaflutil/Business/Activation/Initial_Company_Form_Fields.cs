using System.Drawing;
using System.Threading;

using Fairweather.Service;

namespace Activation
{
    public partial class Initial_Company_Form
    {
        Thread sageusr_thread;
        Thread extract_thread;

        static readonly string temp_dir = Activation_Helpers.temp_dir;

        static readonly bool cst_debug = Activation_Helpers.cst_debug;
        static readonly Size cst_def_size = new Size(282, 380);

        const int cst_ver_0_year = Activation_Helpers.cst_ver_0_year;
        const int min_ver = Activation_Helpers.min_ver;
        const int max_ver = Activation_Helpers.max_ver;

        Block b_user = new Block(); //Controls who has changed the text in pathTB
        bool b_first = true;    //Whether we should ask the user to confirm the dll registration

        Set<int> set_sgregister = new Set<int>();
        Set<int> set_sdoeng = new Set<int>();
        Set<int> set_sdoeng_usr = new Set<int>();

        Set<int> installed = new Set<int>();

        int selected_ver;
        string sageusr_path;


    }
}