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


        public string Serialize()
        {
            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the Bird object to the stream.
            var ser = new DataContractJsonSerializer(typeof(Bird));
            ser.WriteObject(ms, this);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }

    }
}

