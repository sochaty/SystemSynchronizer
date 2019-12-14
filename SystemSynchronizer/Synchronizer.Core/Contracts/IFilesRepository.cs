using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Synchronizer.Core.ApiCommunication.Files;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Core.Contracts
{
    public interface IFilesRepository
    {
        Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles);
        Task<IEnumerable<FileOverviewResponse>> ListFiles(string bucketName);
        Task DownloadFile(string bucketName, string fileName, string downloadPath);
        Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName);
    }
}
