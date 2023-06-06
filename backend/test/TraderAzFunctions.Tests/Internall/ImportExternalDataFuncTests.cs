using FakeItEasy;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging; 

namespace TraderAzFunctions.Tests.Internall
{
    [TestFixture]
    public class ImportExternalDataFuncTests
    {
        [Test]
        public async Task VerifyImport()
        {
            var importFunc = new ImportExternalDataFunc();
            var mockedBinder = A.Fake<IBinder>();
            var mockedLogger = A.Fake<ILogger>();

            await importFunc.ImportData(mockedBinder, mockedLogger);
       
        
        }
    }
}
