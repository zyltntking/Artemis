using Artemis.Data.Core;
using Artemis.Service.Protos;
using Grpc.Core;
using Mapster;

namespace Artemis.App.Identity.Services
{
    /// <summary>
    /// 示例服务
    /// </summary>
    public class SampleService : Sample.SampleBase
    {
        /// <summary>
        /// 日志依赖
        /// </summary>
        private ILogger Logger { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loggerFactory"></param>
        public SampleService(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<SampleService>();
        }

        #region Overrides of SampleBase

        /// <summary>
        /// 测试服务器流式处理
        /// </summary>
        /// <param name="request">The request received from the client.</param>
        /// <param name="responseStream">Used for sending responses back to the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>A task indicating completion of the handler.</returns>
        public override async Task SayHelloStream(HelloRequest request, IServerStreamWriter<HelloResponse> responseStream, ServerCallContext context)
        {
            if (request.Count <= 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Count must be greater than zero."));
            }

            Logger.LogInformation($"Sending {request.Count} hellos to {request.Name}");

            for (var i = 0; i < request.Count; i++)
            {
                //if (context.CancellationToken.IsCancellationRequested)
                //{
                    
                //}

                var response = DataResult.Success(new HelloReply
                {
                    Message = $"Hello {request.Name} {i + 1}"
                }).Adapt<HelloResponse>();

                await responseStream.WriteAsync(response, context.CancellationToken);

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        /// <summary>
        /// 测试客户端流式处理
        /// </summary>
        /// <param name="requestStream">Used for reading requests from the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>The response to send back to the client (wrapped by a task).</returns>
        public override async Task<HelloResponse> StreamingFromClient(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            await foreach (var message in requestStream.ReadAllAsync(context.CancellationToken))
            {
                Logger.LogInformation($"Sending {message.Count} hellos to {message.Name}");
                //if (context.CancellationToken.IsCancellationRequested)
                //{

                //}

                // ...
            }
            return new HelloResponse();
        }

        /// <summary>
        /// 测试双向流式处理
        /// </summary>
        /// <param name="requestStream">Used for reading requests from the client.</param>
        /// <param name="responseStream">Used for sending responses back to the client.</param>
        /// <param name="context">The context of the server-side call handler being invoked.</param>
        /// <returns>A task indicating completion of the handler.</returns>
        public override async Task StreamingBothWays(IAsyncStreamReader<HelloRequest> requestStream, IServerStreamWriter<HelloResponse> responseStream, ServerCallContext context)
        {
            //await foreach (var message in requestStream.ReadAllAsync(context.CancellationToken))
            //{
            //    await responseStream.WriteAsync(new HelloResponse(), context.CancellationToken);
            //}

            // Read requests in a background task.
            var readTask = Task.Run(async () =>
            {
                await foreach (var message in requestStream.ReadAllAsync(context.CancellationToken))
                {
                    Logger.LogInformation($"Sending {message.Count} hellos to {message.Name}");
                    // Process request.
                }
            }, context.CancellationToken);

            // Send responses until the client signals that it is complete.
            while (!readTask.IsCompleted)
            {
                await responseStream.WriteAsync(new HelloResponse(), context.CancellationToken);
                await Task.Delay(TimeSpan.FromSeconds(1), context.CancellationToken);
            }
        }

        #endregion
    }
}
