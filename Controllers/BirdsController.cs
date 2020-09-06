using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HSEApiTraining.Models.DataBase;
using Birds_JSON;
using System.Text.Json;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;

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

        [HttpGet("{color}&{size}")]
        public async Task<string> Filter([FromRoute] string color, [FromRoute] string size)
        {
            try
            {
                // Подтянули всех птиц
                List<Bird> allBirds = (List<Bird>)new DataContractJsonSerializer(typeof(List<Bird>))
                .ReadObject(await AzureDataBase.DownloadData("все_птицы.json", AzureDataBase.BlobContainerBirds));

                //List<Bird> allBirds = JsonSerializer.Deserialize<List<Bird>>(await AzureDataBase.DownloadAllBirds());

                // Фильтруем птиц
                List<Bird> found_birds = allBirds.Where(x => x.Color.Contains(color) && x.Size.Contains(size)).GroupBy(x => x.Name).Select(g => g.First()).ToList();

                //if (color != "") allBirds = allBirds.Where(bird => bird.Color == color).ToList();
                //if (size != "") allBirds = allBirds.Where(bird => bird.Color == color).ToList();

                // Сереализуем массив и отправляем обратно
                var ms = new MemoryStream();
                // Serializer the List<Bird> object to the stream.
                new DataContractJsonSerializer(typeof(List<Bird>)).WriteObject(ms, found_birds);
                byte[] json = ms.ToArray();
                ms.Close();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
            catch (BirdBaseException e) { return "Ошибка: " + e.Message; }
            catch (Exception) { return "Неизвестная ошибка"; }
        }
    }
}
