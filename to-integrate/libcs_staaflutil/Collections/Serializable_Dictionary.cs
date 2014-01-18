using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Fairweather.Service
{
    public static class XmlSerDict
    {
        public static XmlSerDict<TKey, TValue> Make<TKey, TValue>(IDictionary<TKey, TValue> idict) {
            return new XmlSerDict<TKey, TValue>(idict);
        }

    }
    // http://weblogs.asp.net/pwelter34/archive/2006/05/03/444961.aspx

    [XmlRoot("dictionary")]
    public class XmlSerDict<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
    {
        const string STR_Kvp = "item";
        const string STR_Key = "key";
        const string STR_Value = "value";

        public XmlSerDict() : base() { }
        public XmlSerDict(int capacity) : base(capacity) { }
        public XmlSerDict(IDictionary<TKey, TValue> idict) : base(idict) { }


        public XmlSchema
        GetSchema() {

            return null;

        }

        public void
        ReadXml(XmlReader reader) {

            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();

            if (wasEmpty)
                return;

            while (reader.NodeType != XmlNodeType.EndElement) {

                reader.ReadStartElement(STR_Kvp);
                reader.ReadStartElement(STR_Key);

                TKey key = (TKey)keySerializer.Deserialize(reader);

                reader.ReadEndElement();

                reader.ReadStartElement(STR_Value);

                TValue value = (TValue)valueSerializer.Deserialize(reader);

                reader.ReadEndElement();


                this.Add(key, value);

                reader.ReadEndElement();

                reader.MoveToContent();

            }

            reader.ReadEndElement();

        }

        public void
        WriteXml(XmlWriter writer) {

            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (TKey key in this.Keys) {

                writer.WriteStartElement(STR_Kvp);

                writer.WriteStartElement(STR_Key);

                keySerializer.Serialize(writer, key);

                writer.WriteEndElement();

                writer.WriteStartElement(STR_Value);

                TValue value = this[key];

                valueSerializer.Serialize(writer, value);

                writer.WriteEndElement();

                writer.WriteEndElement();

            }

        }


    }
}
