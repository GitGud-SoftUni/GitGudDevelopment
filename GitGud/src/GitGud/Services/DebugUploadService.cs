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
        private GitGudContext _context;
        private IHostingEnvironment _environment;

        public DebugUploadService(IHostingEnvironment environemnt, GitGudContext context)
        {
            _environment = environemnt;
            _context = context;

        }

        public void UploadSong(IFormFile file, string songName, string artistName, string strCategoryId, List<string> tags, string userName)
        {
            //TODO change stuff so it has user Id or something and Upload it to Database ?? Profit ??
            var uploads = Path.Combine(_environment.WebRootPath, "uploads");
			Directory.CreateDirectory(uploads);
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

                int categoryId = int.Parse(strCategoryId);
                Category songCategory = _context.Categories.Find(categoryId);


                var song = new Song
                {
                    Name = songName,
                    ArtistName = artistName,
                    fileAdress = file.FileName,//Think about this some more
                    UploaderName = userName,//TODO use the real username
                    Category = songCategory,
                    Tags = tagsToAdd

                };


                _context.Songs.Add(song);
                _context.Tags.AddRange(song.Tags);
                _context.SaveChanges();

            }

        }
    }
}
