using Microsoft.AspNetCore.Mvc.Testing;
using ErrorReport;

namespace Test.ErrorReport
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var response = await httpClient.GetAsync("https://localhost:7109/api/Reports");
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.Contains("New", stringResult);
        }
    }
}