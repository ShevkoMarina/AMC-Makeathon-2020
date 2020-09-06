using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
            return JsonSerializer.Serialize(this);
        }

    }
}

