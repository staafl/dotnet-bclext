using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

//using System.Web.UI;

// I believe some of the code comes from MiscUtil or BCLExtras...
using System.Linq;
namespace Fairweather.Service
{
    public enum Serialization_Method
    {
        Xml,
        Binary,
        Simple,
        Csv,
    }
    /// <summary>
    /// Utility class used for serializing objects
    /// </summary>
    public static class S
    {


        /*       Travails  */


        public static bool
        Serialize(Stream s, bool binary, params object[] objs) {


            XmlSerializer xml_ser;
            BinaryFormatter bin_ser;
            // Lazy_Dict<Type, XmlSerializer> xml_sers;
            Get_Chassis(binary, out bin_ser, out xml_ser, objs.Select(obj => obj.GetType()).arr());

            try {
                if (binary)
                    foreach (var obj in objs) {

                        //if (binary) {
                        bin_ser.Serialize(s, obj);
                        //}
                        //else {
                        //    var xml_ser = xml_sers[obj.GetType()];
                        //    xml_ser.Serialize(xml_writer, obj);
                        //}

                    }
                else {
                    object to_serialize;

                    to_serialize = objs.lst();
                    // to_serialize = objs.Length == 1 ? objs[0] : objs.lst();

                    xml_ser.Serialize(s, to_serialize);
                }
            }
            catch (SecurityException) {

                return false;
            }
            catch (SerializationException) {

                return false;
            }


            return true;


        }

        class XmlSerDict : Lazy_Dict<Type, XmlSerializer>
        {

            public XmlSerDict(int count) :
                base(type => new XmlSerializer(type)) {
            }

        }
        public static bool
        Deserialize(Stream s,
                    bool binary,
                    out List<object> objs,
                    params Type[] types) {

            objs = null;

            var ret = new List<object>();

            XmlSerializer xml_ser;
            BinaryFormatter bin_ser;

            Get_Chassis(binary, out bin_ser, out xml_ser, types);

            try {

                List<object> tmp = null;

                if (!binary) {

                    var deser = xml_ser.Deserialize(s);

                    // http://www.codeguru.com/forum/showthread.php?p=1353245#post1353245
                    if (types.Length == 1 && deser.GetType() != typeof(List<object>)) {
                        tmp = new List<object>();
                        tmp.Add(deser);
                    }
                    else {
                        tmp = (List<object>)deser;
                    }

                    if (tmp.Count < types.Length)
                        return false;

                    objs = new List<object>(types.Length);


                }


                for (int ii = 0; ii < types.Length; ++ii) {

                    var obj = binary ? bin_ser.Deserialize(s) : tmp[ii];

                    /*       Type_Checking        */


                    if (obj.GetType() != types[ii])
                        return false;

                    ret.Add(obj);

                }

            }
            catch (FileNotFoundException) {
                return false;
            }
            catch (SecurityException) {
                return false;
            }
            catch (SerializationException) {
                return false;
            }
            catch (InvalidOperationException) {
                return false;
            }

            objs = ret;

            return true;

        }




        static void Get_Chassis(bool binary, out BinaryFormatter bin_ser, out XmlSerializer xml_ser, params Type[] types) {

            xml_ser = binary ? null :
                //types.Length == 1 ?
                //new XmlSerializer(types[0]) :
                new XmlSerializer(typeof(List<object>), types);

            bin_ser = binary ? new BinaryFormatter() : null;

        }


        /*       Other        */

        /// <summary>
        /// No nulls allowed!
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="binary"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static bool
        Serialize_To_File(string filename, bool binary, params object[] objs) {

            bool ret;
            using (var fs = new FileStream(filename, FileMode.Create)) {

                ret = Serialize(fs, binary, objs);

            }

            if (!ret)
                H.Safe_Delete(filename);


            return true;


        }

        public static bool
        Deserialize_From_File(string filename, bool binary, out List<object> objs, params Type[] types) {

            objs = null;
            if (!File.Exists(filename))
                return false;

            using (var fs = new FileStream(filename, FileMode.Open)) {
                return Deserialize(fs, binary, out objs, types);
            }

        }

        public static bool
        Deserialize_From_File<T>(string filename, bool binary, out T obj) {

            List<object> objs;
            bool ret = Deserialize_From_File(filename, binary, out objs, new[] { typeof(T) });

            if (ret)
                obj = (T)objs[0];
            else
                obj = default(T);

            return ret;

        }

        public static bool
        Deserialize<T>(Stream s,
            bool binary,
            out T obj) {

            List<object> objs;
            bool ret = Deserialize(s, binary, out objs, new[] { typeof(T) });

            if (ret)
                obj = (T)objs[0];
            else
                obj = default(T);

            return ret;
        }


        /// <summary>
        /// Binary serialization will not work if the class in question has 
        /// been compiled into different assemblies, since BinaryFormatter 
        /// includes assemmbly/versioning information.
        /// 
        /// XML serialization requires a () ctor and only works on public fields
        /// and properties with a working getter and setter
        /// </summary>
        //public static bool
        //Serialize_To_File(string filename, object obj, bool binary) {

        //    Action cleanup = () => H.Safe_Delete(filename);

        //    using (var fs = new FileStream(filename, FileMode.Create)) {

        //        bool xml = !binary;

        //        var xml_ser = xml ? new XmlSerializer(obj.GetType()) : null;
        //        var bin_ser = xml ? null : new BinaryFormatter();

        //        try {
        //            if (xml)
        //                xml_ser.Serialize(fs, obj);
        //            else
        //                bin_ser.Serialize(fs, obj);
        //        }
        //        catch (SecurityException) {
        //            cleanup();
        //            return false;
        //        }
        //        catch (SerializationException) {
        //            cleanup();
        //            return false;
        //        }

        //    }

        //    return true;


        //}


        //public static bool
        //Serialize_To_File(string filename, params object[] objs) {

        //    Action cleanup = () => H.Safe_Delete(filename);

        //    using (var fs = new FileStream(filename, FileMode.Create)) {

        //        var ser = new BinaryFormatter();

        //        try {
        //            foreach (var obj in objs)
        //                ser.Serialize(fs, obj);
        //        }
        //        catch (SecurityException) {
        //            cleanup();
        //            return false;
        //        }
        //        catch (SerializationException) {
        //            cleanup();
        //            return false;
        //        }

        //    }

        //    return true;


        //}

        /// <summary>
        /// See remark on Serialize_To_File
        /// </summary>
        //public static bool
        //Deserialize_From_File<T>(string filename, out T value, bool binary) {

        //    value = default(T);

        //    if (!File.Exists(filename))
        //        return false;

        //    try {
        //        using (var fs = new FileStream(filename, FileMode.Open)) {
        //            bool xml = !binary;
        //            object obj;
        //            if (xml) {
        //                // http://stackoverflow.com/questions/134224/generating-an-xml-serialization-assembly-as-part-of-my-build
        //                var ser = new XmlSerializer(typeof(T));

        //                try {
        //                    obj = ser.Deserialize(fs);
        //                }
        //                catch (InvalidOperationException) {
        //                    return false;
        //                }
        //            }
        //            else {
        //                var ser = new BinaryFormatter();

        //                obj = ser.Deserialize(fs);
        //            }

        //            value = (T)obj;
        //        }

        //    }
        //    catch (FileNotFoundException) { return false; }
        //    catch (SerializationException) { return false; }
        //    catch (InvalidCastException ex) { Console.WriteLine(ex); return false; }
        //    catch (SecurityException) { return false; }

        //    return true;

        //}

        //public static bool
        //Deserialize_From_File(string filename, int count, out List<object> objs) {

        //    objs = null;
        //    if (!File.Exists(filename))
        //        return false;

        //    var ret = new List<object>();

        //    try {
        //        using (var fs = new FileStream(filename, FileMode.Open)) {

        //            var ser = new BinaryFormatter();

        //            for (int ii = 0; ii < count; ++ii)
        //                ret.Add(ser.Deserialize(fs));

        //        }
        //    }
        //    catch (FileNotFoundException) {
        //        return false;
        //    }
        //    catch (SecurityException) {
        //        return false;
        //    }
        //    catch (SerializationException) {
        //        return false;
        //    }
        //    objs = ret;

        //    return true;

        //}


        // ****************************


        /// <summary>
        /// Deserializes an object from a Base64String string
        /// </summary>
        /// <param name="s">Base64String string</param>
        /// <returns>Returns the object that was serialize using the SaveObjectToString() method</returns>
        public static object ReadObjectFromString(string s) {
            using (MemoryStream memory = new MemoryStream(Convert.FromBase64String(s))) {
                return ReadObjectFromStream(memory);
            }
        }
        /// <summary>
        /// Deserializes an object from a stream
        /// </summary>
        /// <param name="s">stream that the object was serialized into</param>
        /// <returns>Returns the object that was serialize using the SaveObjectToStream() method</returns>
        public static object ReadObjectFromStream(Stream s) {
            s.Position = 0;
            return new BinaryFormatter().Deserialize(s);
        }
        //public static object ReadObjectFromStreamLos(Stream s) {
        //    s.Position = 0;
        //    return new LosFormatter().Deserialize(s);
        //}
        /// <summary>
        /// Serializes an object into a memory stream
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static MemoryStream SaveObjectToStream(object obj) {
            MemoryStream memory = new MemoryStream();
            new BinaryFormatter().Serialize(memory, obj);
            return memory;
        }
        //public static MemoryStream SaveObjectToStreamLos(object obj) {
        //    MemoryStream memory = new MemoryStream();
        //    new LosFormatter().Serialize(memory, obj);
        //    return memory;
        //}
        public static byte[] SaveObjectToBytes(object obj) {
            using (MemoryStream memory = new MemoryStream()) {
                new BinaryFormatter().Serialize(memory, obj);
                memory.Position = 0;
                return memory.ToArray();
            }
        }
        /// <summary>
        /// Serializes an object into a Base64 string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SaveObjectToString(object obj) {
            using (MemoryStream memory = SaveObjectToStream(obj)) {
                memory.Position = 0;
                return Convert.ToBase64String(memory.ToArray());
            }
        }
        /// <summary>
        /// Deserilizes an object from a file
        /// </summary>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static object ReadObjectFromFile(string FileName) {
            using (Stream stm = File.Open(FileName, FileMode.Open)) {
                BinaryFormatter bformatter = new BinaryFormatter();
                stm.Position = 0;
                return bformatter.Deserialize(stm);
            }
        }
        /// <summary>
        /// Serializes an object into a file
        /// </summary>
        /// <param name="Object"></param>
        /// <param name="FileName"></param>
        /// <returns></returns>
        public static bool SaveObjectToFile(object data, string FileName) {
            using (Stream stm = File.Open(FileName, FileMode.Create)) {
                BinaryFormatter bformatter = new BinaryFormatter();
                bformatter.Serialize(stm, data);
                return true;
            }
        }
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        static String UTF8ByteArrayToString(byte[] characters) {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }
        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        static Byte[] StringToUTF8ByteArray(string xml) {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(xml);
            return byteArray;
        }
        public static String SerializeXmlObject(object pObject, Type objectType) {
            using (MemoryStream memoryStream = new MemoryStream()) {
                XmlSerializer xs = new XmlSerializer(objectType);
                XmlSerializerNamespaces xmlNamespace = new XmlSerializerNamespaces();
                xmlNamespace.Add("", "");
                xs.Serialize(memoryStream, pObject, xmlNamespace);
                return UTF8ByteArrayToString(memoryStream.ToArray());
            }
        }
        /// <summary>
        /// Method to reconstruct an Object from XML string
        /// </summary>
        /// <param name="pXmlizedString"></param>
        /// <returns></returns>
        public static object DeserializeXmlObject(string xml, Type objectType) {
            XmlSerializer xs = new XmlSerializer(objectType);
            using (MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xml))) {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }
        }
    }
}
