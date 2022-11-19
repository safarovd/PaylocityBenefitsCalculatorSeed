using Api.BenefitsServices;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ApiTests.PaylocityBenefitsCalculatorApiTests.EmployeeControllerTestSuite
{
    public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;
        public EmployeeControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Get_WhenCalled_ReturnsOkResultAsync()
        {
            //_factory.Services.GetService
            //var okResult = await _controller.Get(0);
            //Assert.IsType<OkObjectResult>(okResult);
            var response = await _client.GetAsync("/100");
            System.Diagnostics.Debug.WriteLine(response.Content);
            System.Diagnostics.Debug.WriteLine("YEs");
            response.StatusCode.Equals(HttpStatusCode.OK);
        }

    }
}
