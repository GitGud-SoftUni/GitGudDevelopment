using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace GitGud.Services
{
    public class DebugUploadService : IUploadService
    {
        private IHostingEnvironment _environment;

        public DebugUploadService(IHostingEnvironment environemnt)
        {
            _environment = environemnt;
        }

        public async void UploadSong(IFormFile file, string songName, string artistName, string[] tags)
        {
            //TODO change stuff so it has user Id or something and Upload it to Database ?? Profit ??
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

        }
    }
}
