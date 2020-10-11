using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace WatchFunctionsTests
{
    public class WatchFunctionUnitTests
    {
        [Fact]
        public void TestWatchFunctionSuccess()
        {
            var httpContext = new DefaultHttpContext();
            var queryStringValue = "abc";
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                        (
                            new System.Collections.Generic.Dictionary<string, StringValues>()
                            {
                                { "model", queryStringValue }
                            }
                        )
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = WatchPortalFunction.WatchInfo.Run(request, logger);
            response.Wait();

            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            var result = (OkObjectResult)response.Result;
            dynamic watchinfo = new { Manufacturer = "Abc", CaseType = "Solid", Bezel = "Titanium", Dial = "Roman", CaseFinish = "Silver", Jewels = 15 };
            string watchInfo = $"Watch Details: {watchinfo.Manufacturer}, {watchinfo.CaseType}, {watchinfo.Bezel}, {watchinfo.Dial}, {watchinfo.CaseFinish}, {watchinfo.Jewels}";
            Assert.Equal(watchInfo, result.Value);

        }

        [Fact]
        public void TestWatchFunctionFailureNoQueryString()
        {
            var httpContext = new DefaultHttpContext();
            var request = new DefaultHttpRequest(new DefaultHttpContext());

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = WatchPortalFunction.WatchInfo.Run(request, logger);
            response.Wait();

            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);

            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal("Please provide a watch model in the query string", result.Value);


        }

        [Fact]
        public void TestWatchFunctionFailureNoModel()
        {
            var httpContext = new DefaultHttpContext();
            var queryStringValue = "abc";
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                        (
                            new System.Collections.Generic.Dictionary<string, StringValues>()
                            {
                                { "not-model", queryStringValue }
                            }
                        )
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = WatchPortalFunction.WatchInfo.Run(request, logger);
            response.Wait();

            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);

            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal("Please provide a watch model in the query string", result.Value);

        }
    }
}
