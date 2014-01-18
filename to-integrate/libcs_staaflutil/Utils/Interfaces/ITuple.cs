
namespace Fairweather.Service
{
    public interface ITuple
    {
        object[] ToArray();
    }

    public interface IPair<T1, T2> : ITuple
    {
        T1 First { get; }
        T2 Second { get; }
    }
    public interface ITriple<T1, T2, T3> : IPair<T1, T2>, ITuple
    {
        //T1 First { get; }
        //T2 Second { get; }
        T3 Third { get; }
    }
    public interface IQuad<T1, T2, T3, T4> : ITriple<T1, T2, T3>, ITuple
    {
        //T1 First { get; }
        //T2 Second { get; }
        //T3 Third { get; }
        T4 Fourth { get; }
    }
}
