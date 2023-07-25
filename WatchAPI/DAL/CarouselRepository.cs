using Azure.Storage.Blobs;
using Azure.Storage;
using WatchAPI.Data;
using WatchAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace WatchAPI.DAL
{
    public class CarouselRepository : ICarouselRepository
    {
        private readonly WatchApiContext _context;

        public CarouselRepository(WatchApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Carousel>> GetCarousel()
        {
            return await _context.Carousel.ToListAsync();
        }
    }
}
