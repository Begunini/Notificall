using System;
using System.IO;
using System.Threading.Tasks;
using Infrastructure.Enums;
using Infrastructure.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

namespace Infrastructure
{
    public class BlobManager : IBlobManager
    {
        private readonly string _connectionString = Environment.GetEnvironmentVariable("BlobStorage_ConnectionString");
        private readonly string _containerName = Environment.GetEnvironmentVariable("BlobStorage_Container");

        private readonly CloudBlobContainer _container;

        public BlobManager()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);

            var blobClient = storageAccount.CreateCloudBlobClient();
            var retryPolicy = new LinearRetry(TimeSpan.FromMilliseconds(300), 3);
            blobClient.DefaultRequestOptions.RetryPolicy = retryPolicy;

            _container = blobClient.GetContainerReference(_containerName);
            _container.CreateIfNotExistsAsync();

            BlobContainerPermissions permissions = new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            };
            _container.SetPermissionsAsync(permissions);
        }

        public async Task<string> UploadFile(byte[] file, string name, AudioFormat format)
        {
            var cloudBlockBlob = _container.GetBlockBlobReference(name);

            var formatName = Enum.GetName(typeof(AudioFormat), format);
            cloudBlockBlob.Properties.ContentType = $"audio/{formatName}";

            using (var stream = new MemoryStream(file))
            {
                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }

            return cloudBlockBlob.Uri.AbsoluteUri;
        }
    }
}
