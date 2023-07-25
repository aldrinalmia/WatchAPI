using Microsoft.AspNetCore.Mvc;
using WatchAPI.DAL;
using WatchAPI.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WatchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarouselController : ControllerBase
    {

        readonly ICarouselRepository carouselRepository;


        public CarouselController(ICarouselRepository _carouselRepository)
        {
            this.carouselRepository = _carouselRepository;
        }


        // GET: api/<CarouselController>
        [HttpGet]
        public Task<IEnumerable<Carousel>> Get()
        {
            return carouselRepository.GetCarousel();
        }

      
    }
}
