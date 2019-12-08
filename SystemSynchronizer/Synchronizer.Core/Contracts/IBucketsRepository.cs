using Synchronizer.Core.ApiCommunication.Bucket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizer.Core.Contracts
{
    public interface IBucketsRepository
    {
        Task<bool> DoesS3BucketExist(string bucketName);
        Task<CreateBucketResponse> CreateBucket(string bucketName);
        Task<IEnumerable<BucketOverviewResponse>> ListBuckets();
        Task DeleteBucket(string bucketName);
    }
}
