
using Fairweather.Service;

namespace Common
{
    public interface IQuick_Search_Form_Host
    {
        void Collect_QSF_Result(Pair<string> result, Quick_Search_Form_Mode mode);
        void Handle_QSF_Event(Quick_Search_Form_Event event_type);
        void Refresh();
    }
}
