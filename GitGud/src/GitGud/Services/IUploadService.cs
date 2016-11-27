using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitGud.Services
{
    public interface IUploadService
    {
        void UploadSong(IFormFile file, string songName, string artistName, string strCategoryId, List<string> tags, string userName);
    }
}
