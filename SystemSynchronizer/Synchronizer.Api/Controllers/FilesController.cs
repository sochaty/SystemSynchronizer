using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Synchronizer.Core.ApiCommunication.Files;
using Synchronizer.Core.Contracts;

namespace Synchronizer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;

        public FilesController(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
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
    }
}