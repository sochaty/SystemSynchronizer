using Amazon.S3;
using Amazon.S3.Model;
using Synchronizer.Core.ApiCommunication.Bucket;
using Synchronizer.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Infrastructure.Repositories
{
    public class BucketsRepository: IBucketsRepository
    {
        private readonly IAmazonS3 _clientAmazonS3;
        public BucketsRepository(IAmazonS3 clientAmazonS3)
        {
            _clientAmazonS3 = clientAmazonS3;
        }

        public async Task<bool> DoesS3BucketExist(string bucketName)
        {
            return await _clientAmazonS3.DoesS3BucketExistAsync(bucketName);
        }

        public async Task<CreateBucketResponse> CreateBucket(string bucketName)
        {
            var putBucketRequest = new PutBucketRequest()
            {
                BucketName = bucketName,
                UseClientRegion = true
            };
            var response = await _clientAmazonS3.PutBucketAsync(putBucketRequest);
            return new CreateBucketResponse
            {
                BucketName = bucketName,
                RequestId = response.ResponseMetadata.RequestId
            };
        }

        public async Task<IEnumerable<BucketOverviewResponse>> ListBuckets()
        {
            var response = await _clientAmazonS3.ListBucketsAsync();
            return response.Buckets.Select(b => new BucketOverviewResponse
            {
                BucketName = b.BucketName,
                CreationDate = b.CreationDate
            });
        }

        public async Task DeleteBucket(string bucketName)
        {
            await _clientAmazonS3.DeleteBucketAsync(bucketName);
        }
    }
}
