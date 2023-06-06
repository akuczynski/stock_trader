using FakeItEasy;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Shouldly;
using System.Configuration;
using TraderAzFunctions.Entities;

namespace TraderAzFunctions.Tests.Internall
{
    [TestFixture]
    public class ImportExternalDataFuncTests
    {
        private readonly string? _uri = ConfigurationManager.AppSettings["TradingServiceURI"];

        [Test]
        public async Task VerifyImport()
        {
            Environment.SetEnvironmentVariable("ImportURI", _uri);

            var importFunc = new ImportExternalDataFunc();
            var mockedBinder = A.Fake<IBinder>();
            var mockedLogger = A.Fake<ILogger>();
            ImportLog importLog;

            Assert.DoesNotThrowAsync(async () => {
                importLog = await importFunc.ImportData(mockedBinder, mockedLogger);
                importLog.ShouldNotBeNull();   
                importLog.IsSucceded.ShouldBeTrue();
            });            
        }
    }
}
