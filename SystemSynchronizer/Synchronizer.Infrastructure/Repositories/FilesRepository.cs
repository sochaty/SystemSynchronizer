using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http.Internal;
using Synchronizer.Core.ApiCommunication.Files;
using Synchronizer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Infrastructure.Repositories
{
    public class FilesRepository:IFilesRepository
    {
        private readonly IAmazonS3 _clientAmazonS3;
        public FilesRepository(IAmazonS3 clientAmazonS3)
        {
            _clientAmazonS3 = clientAmazonS3;
        }

        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<FormFile> formFiles)
        {
            var response = new List<string>();
            foreach (var item in formFiles)
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                { 
                    InputStream = item.OpenReadStream(),
                    Key = item.FileName,
                    BucketName = bucketName,
                    CannedACL =  S3CannedACL.NoACL
                };
            }
        }
    }
}
