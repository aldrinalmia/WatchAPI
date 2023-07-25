using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchAPI.Data;
using WatchAPI.Model;
using Azure.Storage.Blobs;
using Azure.Storage;
using System.Net;
using Microsoft.Extensions.Configuration;
using System.Web.Http;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WatchAPI.DAL
{
    public class WatchRepository : IWatchRepository
    {
        private readonly WatchApiContext _context;
        private readonly BlobServiceClient _filesContainer;


        public WatchRepository(WatchApiContext context, BlobServiceClient filesContainer)
        {
            _context = context;
            _filesContainer = filesContainer;
        }


        public async Task<IEnumerable<Watch>> GetWatch()
        {
            return await _context.Watches.OrderByDescending(x=>x.ID).ToListAsync();
        }

        public ActionResult<Watch?> GetWatchById(int id)
        {
            return  _context.Watches.Where(x => x.ID == id).SingleOrDefault();
        }

        public ActionResult<int> AddWatch(Watch watch)
        {
            if (watch.FormFile is not null)
            {
                var container = _filesContainer.GetBlobContainerClient("blobcontainer");

                string prefix = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName = watch.FormFile.FileName;

                var client = container.GetBlobClient($"{prefix}{fileName}");
    
                using (Stream? data = watch.FormFile.OpenReadStream())
                {
                    client.Upload(data);
                }


                string sUrl = client.Uri.ToString();

                watch.URL = sUrl;

                _context.Watches.Add(watch);
                _context.SaveChanges();

            }
            return watch.ID;
        }

        public void UpdateWatch(Watch watch)
        {

            if (watch.FormFile is not null)
            {
                
                var container = _filesContainer.GetBlobContainerClient("blobcontainer");

                string prefix = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName = watch.FormFile.FileName;

                var client = container.GetBlobClient($"{prefix}{fileName}");

                using (Stream? data = watch.FormFile.OpenReadStream())
                {
                    client.Upload(data);
                }

                if (watch.URL is not null)
                {
                    string blobFilename = Path.GetFileName(watch.URL);
                    var file = container.GetBlobClient(blobFilename);
                    file.Delete();
                }

                string sUrl = client.Uri.ToString();

                watch.URL = sUrl;
            }
            else
            {
                string? url = _context.Watches.Where(p => p.ID == watch.ID).Select(x => x.URL).SingleOrDefault();
                watch.URL = url;
            }

            _context.Watches.Update(watch);
            _context.SaveChanges();
        }

        public void DeleteWatch(int id)
        {
            var container = _filesContainer.GetBlobContainerClient("blobcontainer");
            Watch? watch = _context.Watches.Find(id);

            if (watch is not null)
            {
                if (watch.URL is not null)
                {
                    string blobFilename = Path.GetFileName(watch.URL);
                    var file = container.GetBlobClient(blobFilename);
                    file.Delete();
                }

                _context.Watches.Remove(watch);
                _context.SaveChanges();
            }
        }

        public async Task<IEnumerable<Watch>> GetSlide()
        {
            return await _context.Watches.ToListAsync();
        }
    }
}

