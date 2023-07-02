using WatchAPI.Model;

namespace WatchAPI.DAL
{
    public interface IWatchRepository
    {
        Task<IEnumerable<Watch>> GetWatch();

        Task<IEnumerable<Watch>> GetWatchById(int id);

        void AddWatch(Watch watch);
        void UpdateWatch(Watch watch);
        void DeleteWatch(int id);

       
    }
}
