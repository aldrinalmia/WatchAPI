using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using WatchAPI.Model;

namespace WatchAPI.DAL
{
    public interface IWatchRepository
    {
        Task<IEnumerable<Watch>> GetWatch();
        ActionResult<Watch?> GetWatchById(int id);
        ActionResult<int> AddWatch(Watch watch);
        void UpdateWatch(Watch watch);
        void DeleteWatch(int id);
    }
}
