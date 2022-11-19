using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoFixture;
using System.Linq;
using Api.Services.DependentService;
using Api.Dtos.Dependent;

namespace Api.Test
{
    [TestClass]
    public class DependentTests
    {
        private DependentsController _dependentsController;
        private TestContext _testContext;
        private Mock<IDependentService> _dependentService;
        private Fixture _fixture;
        // This variable allows us to write to test console.
        // Can be useful for debugging responses.
        public TestContext TestContext
        {
            get { return _testContext; }
            set { _testContext = value; }
        }
        public DependentTests()
        {
            // use fixture to quickly create test data for our Mock object
            _fixture = new Fixture();
            // Use Mock to mock out our EmployeeService
            _dependentService = new Mock<IDependentService>();
        }

        [TestMethod]
        public async Task Test1_POST_inserts_dependent()
        {
            // Create our dummy data
            var addDependent = _fixture.Create<AddDependentWithEmployeeIdDto>();
            // setup our mock service. Pass in "any" type of the correct parameter and expect the correct return type
            _dependentService.Setup(srvc => srvc.AddDependent(It.IsAny<AddDependentWithEmployeeIdDto>())).Returns(addDependent);
            // setup our controller locally with the given dependentService context
            _dependentsController = new DependentsController(_dependentService.Object);
            // call api and assert the results
            var result = await _dependentsController.AddDependent(addDependent);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test2_GET_retrieves_all_dependents()
        {
            var dependentsList = _fixture.CreateMany<GetDependentDto>(3).ToList();
            _dependentService.Setup(srvc => srvc.GetAllDependents()).Returns(dependentsList);
            _dependentsController = new DependentsController(_dependentService.Object);

            var result = await _dependentsController.GetAll();
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test3_GET_retrieves_dependent()
        {
            var dependent = _fixture.Create<GetDependentDto>();
            _dependentService.Setup(srvc => srvc.GetDependent(0)).Returns(dependent);
            _dependentsController = new DependentsController(_dependentService.Object);

            var result = await _dependentsController.Get(0);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test4_PUT_updates_dependent()
        {
            var dependent = _fixture.Create<GetDependentDto>();
            var updateDependent = _fixture.Create<UpdateDependentDto>();
            _dependentService.Setup(srvc => srvc.UpdateDependent(0, updateDependent)).Returns(dependent);
            _dependentsController = new DependentsController(_dependentService.Object);

            var result = await _dependentsController.UpdateDependent(0, updateDependent);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test4_DELETE_removes_dependent()
        {
            var dependent = _fixture.Create<GetDependentDto>();
            _dependentService.Setup(srvc => srvc.DeleteDependent(0)).Returns(dependent);
            _dependentsController = new DependentsController(_dependentService.Object);

            var result = await _dependentsController.DeleteDependent(0);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }
    }
}
