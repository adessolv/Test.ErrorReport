using Microsoft.AspNetCore.Mvc.Testing;
using ErrorReport;
using ErrorReport.Data;
using Microsoft.EntityFrameworkCore;
using ErrorReport.Model;
using System.Net;

namespace Test.ErrorReport
{
    public class UnitTest1
    {
        [Fact]
        public async Task GetErrorReport_Test()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("https://localhost:7109/api/Reports");
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.Contains("New", stringResult);
        }

        [Fact]
        public async Task GetErrorReportById_Test()
        {
            var options = new DbContextOptionsBuilder<ErrorReportContext>()
                .UseInMemoryDatabase(databaseName: "ErrorReport")
                .Options;

            using (var context = new ErrorReportContext(options))
            {
                int errorReportId = 1;
                var testReport = new Report { Id = errorReportId, Name = "New" };
                context.Report.Add(testReport);
                context.SaveChanges();
            }

            using (var webAppFactory = new WebApplicationFactory<Program>())
            {
                var httpClient = webAppFactory.CreateDefaultClient();
                var response = await httpClient.GetAsync("https://localhost:7109/api/Reports/{errorReportId}");
                var stringResult = await response.Content.ReadAsStringAsync();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }

            using (var context = new ErrorReportContext(options))
            {
                context.Database.EnsureDeleted();
            }
        }
    }
}
