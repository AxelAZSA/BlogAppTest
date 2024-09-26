using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlogApp.Core.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Core.Domain.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public BlobStorageService(IConfiguration configuration)
        {
            var connectionString = configuration["AzureBlobStorage:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerName = configuration["AzureBlobStorage:ContainerName"];
        }

        // Método para cargar una imagen al Blob Storage
        public async Task<string> UploadImageAsync(Stream imageStream, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(imageStream, true);

            return blobClient.Uri.ToString();  // Retorna la URL de la imagen cargada
        }

        // Método para descargar una imagen desde Blob Storage
        public async Task<Stream> DownloadImageAsync(string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                var download = await blobClient.DownloadAsync();
                return download.Value.Content;  // Retorna el contenido del stream
            }
            throw new FileNotFoundException("Image not found in Blob Storage.");
        }
    }
}
