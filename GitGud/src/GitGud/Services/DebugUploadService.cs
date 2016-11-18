using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using GitGud.Models;

namespace GitGud.Services
{
    public class DebugUploadService : IUploadService
    {
        private IHostingEnvironment _environment;

        public DebugUploadService(IHostingEnvironment environemnt)
        {
            _environment = environemnt;

        }

        public void UploadSong(IFormFile file, string songName, string artistName, List<string> tags)
        {
            //TODO change stuff so it has user Id or something and Upload it to Database ?? Profit ??
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");

            var path = Path.Combine(uploads, file.FileName);


            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }



                List<Tag> tagsToAdd = new List<Tag>();
                foreach (var tagg in tags)
                {
                    tagsToAdd.Add(new Tag { Name = tagg });
                }




                var song = new Song
                {
                    Name = file.FileName,
                    ArtistName = artistName,
                    fileAdress = path,//this is not the best way TODO: change it later
                    UploaderName = "Gosho",//TODO use the real username
                    Tags = tagsToAdd//TODO See if this works in the new DB

                };


                //TODO: context.Songs.Add(song);
                //TODO: context.SaveChanges();

            }

        }
    }
}
