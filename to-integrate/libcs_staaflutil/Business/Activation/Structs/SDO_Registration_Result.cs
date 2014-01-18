namespace Common
{
    public struct SDO_Registration_Result
    {
        readonly string m_activation;

        readonly string m_serial;

        readonly int m_version;


        public string Activation {
            get { return m_activation; }
        }

        public string Serial {
            get { return m_serial; }
        }

        public int Version {
            get { return m_version; }
        }

        public SDO_Registration_Result(string serial,
                                         string activation,
                                         int version) {
            m_serial = serial;
            m_activation = activation;
            m_version = version;
        }

    }
}
