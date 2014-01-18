
using Fairweather.Service;

namespace Versioning
{
    public interface ISageObject
    {
    }
    public interface ICount : ISageObject
    {
        int Count {
            get;
        }
    }
    public interface IReadWrite : ISageObject
    {
        bool Read(int IRecNo);
        bool Write(int IRecNo);
    }
    public interface IOpenClose : ISageObject
    {
        bool Open(OpenMode mode);
        void Close();
    }
    public interface IMove : ISageObject
    {
        bool MoveFirst();
        bool MoveNext();
        bool MovePrev();
        bool MoveLast();
    }
    public interface IFields : ISageObject, IReadWrite<string, object>
    {
        Fields Fields { get; }
        string Index_String { get; }
    }
    public interface IFind : ISageObject
    {
        bool Find(bool partial);
        string Key {
            get;
            set;
        }
    }
    public interface IUpdate : ISageObject
    {
        bool Update();
    }

    public interface IAddEditUpdate : ISageObject, IUpdate
    {
        bool AddNew();
        bool Edit();
    }

    public interface IFindFirstNext : ISageObject, IFields
    {
        bool FindFirst(object field, object value);
        bool FindNext(object field, object value);
    }
    public interface ILink : ISageObject
    {
        object Link { get; }

    }
    public interface ILink<T> : ISageObject
    {
        T Link { get; }
    }

    public interface IData : IOpenClose, IReadWrite, IFields, ICount
    {

    }

    public interface IIndex : IIndexOrRecord
    {
        int IndexRecordNumber { get; }
        int DataRecordNumber { get; }

        // DataRecordNumber etc.
    }
    public interface IIndexOrRecord : IFind, IMove, ICount
    {

    }

    public interface ISageRecord : IAddEditUpdate, IFields, IIndexOrRecord, IFind, ICount, IMove
    {
        bool Deleted { get; }
        // RecordNumber?
    }

    public interface ILinkRecord : ISageRecord, ILink
    {
    }

    public interface ILinkRecord<T> : ISageRecord, ILink<T>
    {
    }

}