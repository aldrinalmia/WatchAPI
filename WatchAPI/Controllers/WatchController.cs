using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using WatchAPI.DAL;
using WatchAPI.Data;
using WatchAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WatchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchController : ControllerBase
    {

        readonly IWatchRepository watchRepository;


        public WatchController(IWatchRepository _watchRepository)
        {
            this.watchRepository = _watchRepository;
        }


        //GET: api/<WatchController>
        [HttpGet]
        public Task<IEnumerable<Watch>> Get()
        {
            return watchRepository.GetWatch();
        }

        // GET api/<WatchController>/5
        [HttpGet("{id}")]
        public Task<IEnumerable<Watch>> Get(int id)
        {
            return watchRepository.GetWatchById(id);
        }

        // POST api/<WatchController>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public void Post([FromForm] Watch watch)
        {
            watchRepository.AddWatch(watch);
        }

        // PUT api/<WatchController>/5
        [HttpPut("{id}")]
        public void Put([FromForm] Watch watch)
        {
            watchRepository.UpdateWatch(watch);
        }

        // DELETE api/<WatchController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            watchRepository.DeleteWatch(id);
        }

        

    }
}
