namespace Fairweather.Service
{
    using System;

    public static class Args
    {
        public static Args<TMut> Make<TMut>(TMut mut) {

            return new Args<TMut>(mut);

        }

        public static Args<TMut, TIm> Make<TMut, TIm>(TMut mut, TIm im) {

            return new Args<TMut, TIm>(mut, im);

        }

        public static Args_RO<TIm> Make_RO<TIm>(TIm im) {

            return new Args_RO<TIm>(im);

        }
    }

    public class Args_RO<TIm> : EventArgs
    {

        public Args_RO(TIm im) {

            m_data = im;

        }

        public TIm Im {
            get { return m_data; }
        }

        readonly TIm m_data;

        //public static implicit operator EArgs_RO<TIm>(TIm value) {
        //    return new EArgs_RO<TIm>(value);
        //}
    }
    public class Args<TMut> : EventArgs
    {
        public Args(TMut mut) {

            m_data = mut;

        }      

        public TMut Mut {
            get { return m_data; }
            set { m_data = value; }
        }

        TMut m_data;

    }

    public class Args<TMut, TIm> : EventArgs
    {
        TMut m_data_1;
        readonly TIm m_data_2;

        public TMut Mut {
            get { return m_data_1; }
            set { m_data_1 = value; }
        }

        public TIm Im {
            get { return m_data_2; }
        }

        public Args(TMut mut, TIm im) {

            m_data_1 = mut;
            m_data_2 = im;

        }
    }

}