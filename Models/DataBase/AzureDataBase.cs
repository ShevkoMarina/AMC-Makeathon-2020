using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Microsoft.Azure;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System;
using HSEApiTraining.Models;
using HSEApiTraining.Models.DataBase;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Birds_JSON;

namespace HSEApiTraining.Models.DataBase
{
    public class AzureDataBase
    {
        #region Secrets
        private const string connectionString = "DefaultEndpointsProtocol=https;AccountName=ornithologicalvault;AccountKey=7gZjHzPZPFy/EaKM9ZCPh9iwoDcCy/PW8azghWLI3i/IjFwmV2dp13VwfVI67m/2ycnBLzn1Xyi+EQhyFStCBg==;EndpointSuffix=core.windows.net";
        public readonly static BlobContainerClient BlobContainerUsers = new BlobServiceClient(connectionString).GetBlobContainerClient("accounts");
        public readonly static BlobContainerClient BlobContainerBirds = new BlobServiceClient(connectionString).GetBlobContainerClient("birds");

        #endregion Secrets

        #region Upload
        public static async Task UploadUserData(User user)
        {
            await UploadData(user.SerializeStream(), user.ID + ".json", "application/json", BlobContainerUsers);
        }

        /// <summary>
        /// Загружает любые данные в хранилище Azure
        /// </summary>
        /// <param name="DataStream">Данные, представленные ввиде экзепляра Stream</param>
        /// <param name="name">Путь до данных в хранилище Azure.</param>
        /// <param name="contentType">MIME-тип, который загружается в хранилище Azure.</param>
        /// <returns></returns>
        public static async Task UploadData(Stream DataStream, string name, string contentType, BlobContainerClient BlobContainer)
        {
            try
            {
                await BlobContainer.SetAccessPolicyAsync(PublicAccessType.None);
                BlobClient blob = BlobContainer.GetBlobClient(name);
                await blob.UploadAsync(DataStream, overwrite: true);
                await blob.SetHttpHeadersAsync(new BlobHttpHeaders() { ContentType = contentType });
            }
            catch (Exception) { throw new BirdBaseException("Uploading is not possible"); }
        }
        #endregion Upload

        #region Download
        /// <summary>
        /// Скачивает данные конкретного пользователя из хранилища Azure.
        /// </summary>
        /// <returns>Данные пользователя</returns>
        public async static Task<User> DownloadUserData(ulong userID)
            => (User)new DataContractJsonSerializer(typeof(User))
                .ReadObject(await DownloadData(userID + ".json", BlobContainerUsers));

        public async static Task<string> DownloadUserDataString(ulong userID)
        {
            string result;
            using (var allbirdsStream = new StreamReader(await DownloadData(userID + ".json", BlobContainerUsers)))
                result = allbirdsStream.ReadToEnd();

            return result;
        }


        /// <summary>
        /// Скачивает данные птицы
        /// </summary>
        /// <returns>Данные пользователя</returns>
        public async static Task<string> DownloadBirdDataString(string name)
        {
            try
            {
                string result;
                using (var allbirdsStream = new StreamReader(await DownloadData(name + ".json", BlobContainerBirds)))
                    result = allbirdsStream.ReadToEnd();

                return result;
            }
            catch (BirdBaseException e) { throw e; }
            catch (Exception e) { throw new BirdBaseException("Метод: DownloadBirdData ][ текст ошибки: " + e.Message); }
        }

        /// <summary>
        /// Скачиваем и переводим в объект Bird
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async static Task<Bird> DownloadBirdData(string name)
        {
            try
            {
                return (Bird)new DataContractJsonSerializer(typeof(Bird))
                    .ReadObject(await DownloadData(name + ".json", BlobContainerBirds));
            }
            catch (BirdBaseException e) { throw e; }
            catch (Exception e) { throw new BirdBaseException("Метод: DownloadBirdData ][ текст ошибки: " + e.Message); }
        }

        public async static Task<string> DownloadAllBirds()
        {
            string result;
            using (var allbirdsStream = new StreamReader(await DownloadData("имена_птиц.json", BlobContainerBirds)))
                result = allbirdsStream.ReadToEnd();

            return result;
        }

        /// <summary>
        /// Загружает данные из хранилища Azure.
        /// </summary>
        /// <param name="name">Путь до данных</param>
        /// <returns>Данные в формате экземпляра Stream</returns>
        public static async Task<Stream> DownloadData(string path, BlobContainerClient BlobContainer)
        {
            try
            {
                //await BlobContainer.GetBlobsAsync();
            }
            catch (Exception e) { throw new BirdBaseException("Метод: DownloadData ][ текст ошибки: " + "____ В БД НЕТ ФАЙЛОВ ____"); }


            try
            {
                return (await BlobContainer
                .GetBlobClient(path)
                .DownloadAsync()).Value.Content;
            }
            catch (Exception e) { throw new BirdBaseException("Метод: DownloadData ][ текст ошибки: " + e.Message + " ][ " + BlobContainerBirds.Uri + "/" + path); }
        }
        #endregion Download
    }
}

#region BaseInteraction
/// <summary>
/// Алгоритм аутентификация пользователя.
/// Является проверкой идентификации полей логина и пароля 
/// у введённого пользователя и о его представлении в базе данных. 
/// </summary>
/// <returns>Заключение проверки</returns>
//public async Task<bool> Authentication(User ProofUser)
//{
//    User user = new User("", "");
//    try
//    {
//        //user = await DownloadUserData(ProofUser);
//    }
//    catch (Exception) { throw new BirdBaseException("Account is not exist"); }

//    if (ProofUser.Equals(user)) return true;
//    throw new BirdBaseException("Incorrect password");
//}
/// <summary>
/// Регистрация нового пользователя.
/// </summary>
/// <param name="newUser">Представление о пользователе</param>
//public async Task Registration(User NewUser)
//{
//    if (await UserExistence(NewUser))
//        throw new BirdBaseException("Non unique login");
//    //await UploadUserData(NewUser);
//}
/// <summary>
/// Изменяет данные пользователя в базе данных.
/// </summary>
/// <param name="CurrentUser">Текущее представление о пользователе, чьи данные будут изменены</param>
/// <param name="changedUser">Новые данные пользователя</param>
//public async void UpdateData(User CurrentUser, User changedUser)
//{
//    //await UploadUserData(changedUser);
//}
/// <summary>
/// Получает всех пользователей из хранилища Azure 
/// для демонстрации работы программы.
/// </summary>
/// <returns>пользователи</returns>
//public static async Task<List<User>> GetUsers()
//{
//    var users = new List<User>();
//    foreach (var blob in BlobContainer.GetBlobs())
//    {
//        if (!blob.Name.Contains("/"))
//        {
//            //User userDB = await new AzureDataBase().DownloadUserData(new User(blob.Name.Replace(".xml", ""), ""));
//            //if (userDB.FaceID != null)
//            //    userDB.Portrait = (byte[])new ImageConverter().ConvertTo(
//            //                                new Bitmap(await DownloadPortrait(userDB)), typeof(byte[]));
//            //users.Add(userDB);
//        }
//    }
//    return users;
//}
/// <summary>
/// Проверяет, существует ли пользователь в базе данных.
/// </summary>
/// <param name="user">Пользователь, существование которого необходимо проверить</param>
/// <returns>Заключение проверки</returns>
//private static async Task<bool> UserExistence(User user)
//{
//    try
//    {
//        return (await BlobContainer
//          .GetBlobClient(user.Login + ".xml")
//          .DownloadAsync()).Value.ContentLength != 0;
//    }
//    catch (Exception) { return false; }
//}
#endregion BaseInteraction