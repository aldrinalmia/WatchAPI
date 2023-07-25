using WatchAPI.Model;

namespace WatchAPI.DAL
{
    public interface ICarouselRepository
    {
        Task<IEnumerable<Carousel>> GetCarousel();
    }
}
