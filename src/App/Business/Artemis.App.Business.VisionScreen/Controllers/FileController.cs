using Artemis.Data.Core;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Artemis.App.Business.VisionScreen.Controllers
{
    /// <summary>
    /// 文件控制器
    /// </summary>
    [Route("api/BusinessService/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        /// <summary>
        /// 允许的扩展名
        /// </summary>
        private readonly string[] _allowedExtensions = new[]
        {
            ".jpg", ".jpeg", ".png"
        };

        private readonly ILogger<FileController> _logger;

        private readonly IMongoClient _mongoClient;

        /// <summary>
        /// 文件控制器
        /// </summary>
        /// <param name="mongoClient"></param>
        /// <param name="logger"></param>
        public FileController(
            IMongoClient mongoClient, 
            ILogger<FileController> logger)
        {
            _logger = logger;
            _mongoClient = mongoClient;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <remarks>允许.jpg/.jpeg/.png</remarks>
        /// <returns></returns>
        [HttpPost]
        public async Task<DataResult<string>> UploadPicture(IFormFile file)
        {
            var db = _mongoClient.GetDatabase("FileStorage");

            var buckets = new GridFSBucket(db);

            var extension = Path.GetExtension(file.FileName);

            if (!_allowedExtensions.Contains(extension))
            {
                return DataResult.Fail<string>("不被允许的文件类型");
            }

            var id = await buckets.UploadFromStreamAsync(file.FileName, file.OpenReadStream(), new GridFSUploadOptions
            {
                Metadata = new BsonDocument
                {
                    {"ContentType", file.ContentType}
                }
            }, HttpContext.RequestAborted);

            return DataResult.Success(id.ToString());
        }

        /// <summary>
        /// 读取图片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<FileContentResult> ReadPicture([FromQuery]string id)
        {
            var db = _mongoClient.GetDatabase("FileStorage");

            var buckets = new GridFSBucket(db);

            var file = await buckets.DownloadAsBytesAsync(new ObjectId(id));

            var gridFileInfo = await buckets.FindAsync(new BsonDocument("_id", new ObjectId(id)), cancellationToken: HttpContext.RequestAborted);

            var metadata = await gridFileInfo.FirstOrDefaultAsync(HttpContext.RequestAborted);

            var contentType = metadata.Metadata["ContentType"].AsString;

            return new FileContentResult(file, contentType);
        }
    }
}
