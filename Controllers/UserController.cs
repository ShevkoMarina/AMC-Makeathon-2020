using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Birds_JSON;
using HSEApiTraining.Models;
using Microsoft.AspNetCore.Mvc;
using HSEApiTraining.Models.DataBase;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        // GET api/<controller>/IDшник
        // Отдаёт пользователя по его ID
        [HttpGet("GetUserData/{ID}")]
        public async Task<string> GetUserData(ulong ID)
            => (await AzureDataBase.DownloadUserData(ID)).Serialize();

        // POST api/<controller>
        // Добавляем пользователю
        [HttpPost]
        public async Task<ulong> Post()
        {
            var user = new User();
            await AzureDataBase.UploadUserData(user);
            return user.ID;
        }

        // PUT api/<controller>/5
        // Добавляем пользователю птицу
        [HttpPut("AddBird")]
        public async Task Put([FromBody] string birdName, [FromBody] ulong userID)
        {
            var bird = await AzureDataBase.DownloadBirdData(birdName);
            var user = await AzureDataBase.DownloadUserData(userID);
            user.AddBird(bird);
            await AzureDataBase.UploadUserData(user);
        }
        
    }
}
