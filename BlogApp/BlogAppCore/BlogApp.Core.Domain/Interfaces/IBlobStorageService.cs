using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Interfaces
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string fileName);
        Task<Stream> DownloadImageAsync(string fileName);
    }
}
