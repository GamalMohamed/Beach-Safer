using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApp.Services
{
    public class BlobServices
    {
        private const string StorageName = "espblob";

        private const string AccountKey =
            "F+q7zg+bARTa4FQX1vS++TUGRikvGOCUZLeO6MB2FxzysQpOxDwKkUmUm94Fl6b4vwN7T+yEHYkznJzGU4W3CQ==";

        public readonly string BlobUrl = "https://espblob.blob.core.windows.net/";

        public StorageCredentials Credentials { get; set; }

        public CloudStorageAccount StorageAccount { get; set; }

        public CloudBlobClient BlobClient { get; set; }

        public CloudBlobContainer BlobContainer { get; set; }

        public BlobServices()
        {
            Credentials = new StorageCredentials(StorageName, AccountKey);
            StorageAccount = new CloudStorageAccount(Credentials, true);
            BlobClient = StorageAccount.CreateCloudBlobClient();
        }

        private static byte[] ByteArray(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, img.RawFormat);
                return ms.ToArray();
            }
        }

        public void BlobImageUpload(string image, string path, string type)
        {

            BlobContainer = BlobClient.GetContainerReference(type);
            var blockBlob = BlobContainer.GetBlockBlobReference(image);
            var x = Path.GetExtension(path)?.ToLower();
            if (x != ".pdf")
            {
                var image1 = Image.FromFile(path);
                var arr = ByteArray(image1);
                var stream = new MemoryStream(arr);
                blockBlob.UploadFromStream(stream);
            }
            else
            {
                using (FileStream file = File.OpenRead(path))
                {
                    blockBlob.UploadFromStream(file);
                }
            }
        }

        public void BlobDelete(string blobName, string containerName)
        {
            BlobContainer = BlobClient.GetContainerReference(containerName);
            var blockBlob = BlobContainer.GetBlockBlobReference(blobName);
            blockBlob.Delete();
        }
    }

}