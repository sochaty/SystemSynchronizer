using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synchronizer.Core.ApiCommunication.Bucket;
using Synchronizer.Core.Contracts;

namespace Synchronizer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketsController : ControllerBase
    {
        private readonly IBucketsRepository _bucketsRepository;
        public BucketsController(IBucketsRepository bucketsRepository)
        {
            _bucketsRepository = bucketsRepository;
        }

        [HttpPost]
        [Route("{bucketName}")]
        public async Task<ActionResult<CreateBucketResponse>> CreateBucket([FromRoute]string bucketName)
        {
            var bucketExists = await _bucketsRepository.DoesS3BucketExist(bucketName);
            if (bucketExists)
            {
                return BadRequest("S3 bucket already exists");
            }
            var result = await _bucketsRepository.CreateBucket(bucketName);
            if (result == null)
            {
                return BadRequest();
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("")]
        public async Task<ActionResult<IEnumerable<BucketOverviewResponse>>> ListBuckets()
        {
            var result = await _bucketsRepository.ListBuckets();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("{bucketName}")]
        public async Task<IActionResult> DeleteBucket(string bucketName)
        {
            await _bucketsRepository.DeleteBucket(bucketName);
            return Ok();
        }
    }
}