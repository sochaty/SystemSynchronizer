using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Synchronizer.Core.ApiCommunication.Files;
using Synchronizer.Core.Contracts;

namespace Synchronizer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;
        private readonly IConfiguration _configuration;

        public FilesController(IFilesRepository filesRepository,IConfiguration configuration)
        {
            _filesRepository = filesRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("{bucketName}")]
        public async Task<ActionResult<AddFileResponse>> AddFiles(string bucketName, IList<IFormFile> formFiles)
        {
            if (formFiles == null)
            {
                return BadRequest("Request contains no file(s) to upload");
            }
            var response = await _filesRepository.UploadFiles(bucketName, formFiles);
            if (response == null)
            {
                return BadRequest();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("{bucketName}")]
        public async Task<ActionResult<IEnumerable<FileOverviewResponse>>> ListFiles(string bucketName)
        {
            var response = await _filesRepository.ListFiles(bucketName);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("{bucketName}/download/{fileName}")]
        public async Task<IActionResult> DownloadFile(string bucketName,string fileName)
        {
            var path = _configuration["downloadPath"];
            await _filesRepository.DownloadFile(bucketName, fileName, path);
            return Ok();
        }
        [HttpDelete]
        [Route("{bucketName}/{fileName}")]
        public async Task<ActionResult<DeleteFileResponse>> DeleteFile(string bucketName, string fileName)
        {
            var response = await _filesRepository.DeleteFile(bucketName, fileName);
            return Ok(response);
        }
    }
}