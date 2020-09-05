using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HSEApiTraining.Models.DataBase;
using Birds_JSON;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HSEApiTraining.Controllers
{
    [Route("api/[controller]")]
    public class BirdsController : Controller
    {
        // имена_птиц.json
        // GET: api/<controller>
        [HttpGet("GetAllBirds")]
        public async Task<string> GetAllBirds()
            => await AzureDataBase.DownloadAllBirds();

        // Получаем данные по птице через её название
        // GET api/<controller>/название
        [HttpGet("{name}")]
        public async Task<string> Get(string name)
        {
            try
            {
                return (await AzureDataBase.DownloadBirdData(name)).Serialize();
            }
            catch (BirdBaseException e) { return "Ошибка: " + e.Message; }
            catch (Exception) { return "Неизвестная ошибка"; }

        }
    }
}
