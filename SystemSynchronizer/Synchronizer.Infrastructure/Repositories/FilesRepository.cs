using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Synchronizer.Core.ApiCommunication.Files;
using Synchronizer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task<AddFileResponse> UploadFiles(string bucketName, IList<IFormFile> formFiles)
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
                using (var ftutility = new TransferUtility(_clientAmazonS3))
                {
                    await ftutility.UploadAsync(uploadRequest);
                }
                var presignedUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = item.FileName,
                    Expires = DateTime.Now.AddDays(1)
                };
                var url = _clientAmazonS3.GetPreSignedURL(presignedUrlRequest);
                response.Add(url);
            }
            return new AddFileResponse
            {
                PreSignedUrl = response
            };
        }

        public async Task<IEnumerable<FileOverviewResponse>> ListFiles(string bucketName)
        {
            var response = await _clientAmazonS3.ListObjectsAsync(bucketName);
            return response.S3Objects.Select(x => new FileOverviewResponse
            {
                BucketName = x.BucketName,
                Key = x.Key,
                Owner = x.Owner.DisplayName,
                Size = x.Size
            });
        }
        public async Task DownloadFile(string bucketName,string fileName,string downloadPath)
        {
            var pathAndFilename = $"{downloadPath}\\{fileName}";
            var downloadRequest = new TransferUtilityDownloadRequest
            {
                BucketName = bucketName,
                Key = fileName,
                FilePath = pathAndFilename
            };
            using (var ftutility = new TransferUtility(_clientAmazonS3))
            {
                await ftutility.DownloadAsync(downloadRequest);
            }
        }

        public async Task<DeleteFileResponse> DeleteFile(string bucketName, string fileName)
        {
            var objectDeleteRequest = new DeleteObjectsRequest
            {
                BucketName = bucketName
            };
            objectDeleteRequest.AddKey(fileName);
            var response = await _clientAmazonS3.DeleteObjectsAsync(objectDeleteRequest);
            return new DeleteFileResponse
            {
                NumberOfDeletedObjects = response.DeletedObjects.Count
            };
        }

        public async Task AddJsonObject(string bucketName, AddJsonObjectRequest request)
        {
            var createdOn = DateTime.UtcNow;
            var key = $"{createdOn:yyyy}/{createdOn:MM}/{createdOn:dd}/{request.Id}";
            var putObjectRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = key,
                ContentBody = JsonConvert.SerializeObject(request)
            };
            await _clientAmazonS3.PutObjectAsync(putObjectRequest);
        }

        public async Task<GetJsonObjectResponse> GetJsonObject(string bucketName, string fileName)
        {
            var request = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };
            var response = await _clientAmazonS3.GetObjectAsync(request);
            using (var reader = new StreamReader(response.ResponseStream))
            {
                var content = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<GetJsonObjectResponse>(content);
            }
        }
    }
}
