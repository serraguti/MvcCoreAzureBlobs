using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MvcCoreAzureBlobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAzureBlobs.Services
{
    public class ServiceStorageBlobs
    {
        private BlobServiceClient client;

        public ServiceStorageBlobs(string keys)
        {
            this.client = new BlobServiceClient(keys);
        }

        //METODO PARA DEVOLVER TODOS LOS CONTENEDORES
        public async Task<List<string>> GetContainersAsync()
        {
            List<string> containers = new List<string>();
            await foreach (var container in this.client.GetBlobContainersAsync())
            {
                containers.Add(container.Name);
            }
            return containers;
        }

        //METODO PARA CREAR NUEVOS CONTENEDORES
        public async Task CreateContainerAsync(string nombre)
        {
            await this.client.CreateBlobContainerAsync(nombre.ToLower()
                , Azure.Storage.Blobs.Models.PublicAccessType.Blob);
        }

        public async Task DeleteContainerAsync(string nombre)
        {
            await this.client.DeleteBlobContainerAsync(nombre);
        }

        //METODO PARA MOSTRAR LOS BLOBS DE UN CONTENEDOR
        public async Task<List<Blob>> GetBlobsAsync(string containerName)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            List<Blob> blobs = new List<Blob>();
            await foreach (BlobItem blob in containerClient.GetBlobsAsync())
            {
                BlobClient blobClient = containerClient.GetBlobClient(blob.Name);
                blobs.Add(
                    new Blob { Nombre = blob.Name
                    , Url = blobClient.Uri.AbsoluteUri});
            }
            return blobs;
        }

        //METODO PARA ELIMINAR UN BLOB
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            await containerClient.DeleteBlobAsync(blobName);
        }

        //METODO PARA SUBIR UN BLOB A AZURE
        public async Task UploadBlobAsync(string containerName
            , string blobName, Stream stream)
        {
            BlobContainerClient containerClient =
                this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync(blobName, stream);
        }
    }
}
