using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WatchAPI.Data;
using WatchAPI.Model;
using Azure.Storage.Blobs;
using Azure.Storage;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace WatchAPI.DAL
{
    public class WatchRepository : IWatchRepository
    {
        private readonly WatchApiContext _context;

        private readonly string _storageAccount = "tempstorage1989";
        private readonly string? _key = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["storageKey"];
        private readonly BlobContainerClient _filesContainer;
        private readonly string blobUrl = $"https://tempstorage1989.blob.core.windows.net/";
        private readonly string containerName = "testcontainer";

        public WatchRepository(WatchApiContext context)
        {
            _context = context;

            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobServiceClient = new BlobServiceClient(new Uri(blobUrl), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient(containerName);
        }

  

        public async Task<IEnumerable<Watch>> GetWatch()
        {
            return await _context.Watches.ToListAsync();
        }

        public async Task<IEnumerable<Watch>> GetWatchById(int id)
        {
            return await _context.Watches.Where(x => x.ID == id).ToListAsync();
        }

        public void AddWatch(Watch watch)
        {
            if (watch.FormFile is not null)
            {

                string prefix = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName = watch.FormFile.FileName;

                BlobClient client = _filesContainer.GetBlobClient($"{prefix}{fileName}");

                using (Stream? data = watch.FormFile.OpenReadStream())
                {
                    client.Upload(data);
                }

                string sUrl = blobUrl + containerName + $"/" + prefix + fileName;

                watch.URL = sUrl;

                _context.Watches.Add(watch);
                _context.SaveChanges();
            }
        }

        public void UpdateWatch(Watch watch)
        {
            if (watch.FormFile is not null)
            {
                string prefix = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string fileName = watch.FormFile.FileName;

                BlobClient client = _filesContainer.GetBlobClient($"{prefix}{fileName}");

                using (Stream? data = watch.FormFile.OpenReadStream())
                {
                    client.Upload(data);
                }


                if (watch.URL is not null)
                {
                    string blobFilename = watch.URL.Remove(0, 60);
                    BlobClient file = _filesContainer.GetBlobClient(blobFilename);
                    file.Delete();
                }

                string sUrl = blobUrl + containerName + $"/" + prefix + fileName;

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

            Watch? watch = _context.Watches.Find(id);

            if (watch is not null)
            {
                if (watch.URL is not null)
                {
                    string blobFilename = watch.URL.Remove(0, 60);
                    BlobClient file = _filesContainer.GetBlobClient(blobFilename);
                    file.Delete();
                }

                _context.Watches.Remove(watch);
                _context.SaveChanges();
            }
        }

       
    }
}

