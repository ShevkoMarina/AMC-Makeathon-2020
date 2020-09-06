using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;

namespace Birds_JSON
{
    [DataContract]
    public class Bird
    {
        #region Bird's properties
        //-------------Координаты
        [DataMember]
        public string Coords { get; set; }
        //-------------

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public string Size { get; set; }

        [DataMember]
        public string Picture { get; set; }

        [DataMember]
        public string Sound { get; set; }

        [DataMember]
        public string Familia { get; set; }

        [DataMember]
        public string Ordo { get; set; }

        [DataMember]
        public string Genus { get; set; }

        [DataMember]
        public string Meal { get; set; }
        #endregion

        public Bird(string name, string color, string size, string picture, string sound,
                    string familia, string ordo, string genus)
        {
            Name = name;
            Color = color;
            Size = size;
            Picture = picture;
            Sound = sound;
            Familia = familia;
            Ordo = ordo;
            Genus = genus;
        }

        public override string ToString()
            => $"{Familia} {Meal}";


        public string Serialize() // Переводит корректный this [Экземпляр класса Bird] в не ту кодировку
        {
            //string result = "JSON для проверки";
            //using (var stream = Stream.Null)
            //{
            //    new DataContractJsonSerializer(typeof(Bird)).WriteObject(stream, this);
            //    using (var streamReader = new StreamReader(stream))
            //    {
            //        result = streamReader.ReadToEnd();
            //    }
            //}


            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the User object to the stream.
            var ser = new DataContractJsonSerializer(typeof(Bird));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);


            /*UTF8Encoding utf8 = new UTF8Encoding();
             string jsonUnicode = JsonSerializer.Serialize(this);*/
            /*string changed = /*JsonSerializer.Serialize(this); StringEncodingConvert(jsonUnicode, "Unicode", "UTF-8");

            return changed;*/

            /*Byte[] encodedBytes = utf8.GetBytes(jsonUnicode);
            string changed = utf8.GetString(encodedBytes);
            return Encoding.GetEncoding(jsonUnicode).ToString();*/
        }

        /// <summary>
        /// Convert a string from one charset to another charset
        /// </summary>
        /// <param name="strText">source string</param>
        /// <param name="strSrcEncoding">original encoding name</param>
        /// <param name="strDestEncoding">dest encoding name</param>
        /// <returns></returns>
        public static String StringEncodingConvert(String strText, String strSrcEncoding, String strDestEncoding)
        {
            System.Text.Encoding srcEnc = Encoding.Unicode/*System.Text.Encoding.GetEncoding(strSrcEncoding)*/;
            System.Text.Encoding destEnc = Encoding.UTF8; // System.Text.Encoding.GetEncoding(strDestEncoding);
            byte[] bData = srcEnc.GetBytes(strText);
            byte[] bResult = System.Text.Encoding.Convert(srcEnc, destEnc, bData);
            return destEnc.GetString(bResult);
        }

    }
}

