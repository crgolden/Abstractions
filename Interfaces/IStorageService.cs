namespace Clarity.Abstractions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public interface IStorageService
    {
        Task<Uri> UploadFileToStorageAsync(
            IFormFile file,
            string containerName,
            CancellationToken token);

        Task<Uri> UploadByteArrayToStorageAsync(
            byte[] buffer,
            string fileName,
            string containerName,
            CancellationToken token);

        string GetSharedAccessSignature(
            string fileName,
            string containerName);

        Task<bool> DeleteFileFromStorageAsync(Uri blobUri, CancellationToken token);

        Task DeleteAllFromStorageAsync(string containerName, CancellationToken token);
    }
}