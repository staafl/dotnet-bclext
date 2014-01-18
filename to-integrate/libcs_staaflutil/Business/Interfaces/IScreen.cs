using System.Collections.Generic;

namespace Common
{

    public interface IScreen : IControlHost, IQuick_Search_Form_Host
    {
        SortedList<string, string> AccountAutoComplete { set; }
        SortedList<string, string> BankAutoComplete { set; }

        Selection_Result SelectAccount(string account);
        Selection_Result SelectBank(string account);

        bool SelectDept(int department);
    }
}
