using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Birds_JSON;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

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
        public List<string> BirdsCollection { get; set; }
        //----------------
        #endregion

        public User()
        {
            ID = userAmount;
            userAmount++;
            BirdsCollection = new List<string>();
        }

        public string Serialize() => JsonSerializer.Serialize(this);


        public Stream SerializeStream()
            => GenerateStreamFromString(JsonSerializer.Serialize(this));

        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
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


        public void AddBird(string bird) =>
            BirdsCollection.Add(bird);

    }
}