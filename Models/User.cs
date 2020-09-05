using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Birds_JSON;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HSEApiTraining.Models
{
    [DataContract]
    public class User
    {
        private static ulong userAmount = 0;
        #region Properties
        //[DataMember]
        //public string Login { get; private set; }
        //[DataMember]
        //public string Password { get; private set; }
        [DataMember]
        public ulong ID { get; set; }

        //----------------Коллекция всех птиц пользователя
        [DataMember]
        public List<Bird> BirdsCollection { get; set; }
        //----------------
        #endregion

        public User()
        {
            ID = userAmount;
            userAmount++;
        }

        public string Serialize()
        {
            //JsonSerializer.Serialize(this, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)});
            //JsonSerializer.SerializeToUtf8Bytes();
            //----------------
            return JsonConvert.SerializeObject(this);
            //return JsonSerializer.Serialize(this, new JsonSerializerOptions() { Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All) });
        }

        public Stream SerializeStream() // Скорее всего тут ошибка
        {
            Stream stream = new MemoryStream();
            using (var str = new StreamWriter(stream))
            {
                new DataContractJsonSerializer(typeof(User)).WriteObject(str.BaseStream, this);
                if (str.BaseStream.Length == 0)
                {
                    throw new System.Exception();
                }
            }
            
            return stream;
        }
        /// <summary>
        /// Проверка на идентичность 2-х пользователей. 
        /// Осуществляется путём сравнения логина и пароля.
        /// </summary>
        /// <param name="obj">Другой пользователь</param>
        /// <returns>Заключение проверки</returns>
        //public override bool Equals(object obj) =>
        //           Login == ((User)obj).Login
        //     && Password == ((User)obj).Password;


        public void AddBird(Bird bird) =>
            BirdsCollection.Add(bird);

    }
}