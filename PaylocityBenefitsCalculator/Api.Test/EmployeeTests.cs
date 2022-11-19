using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Api.Services;
using Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using AutoFixture;
using Api.Dtos.Employee;
using System.Linq;

namespace Api.Test
{
    [TestClass]
    public class EmployeeTests
    {
        private EmployeesController _employeesController;
        private TestContext _testContextInsatance;
        private Mock<IEmployeeService> _employeeService;
        private Fixture _fixture;
        // This variable allows us to write to test console.
        // Can be useful for debugging responses.
        public TestContext TestContext
        {
            get { return _testContextInsatance; }
            set { _testContextInsatance = value; }
        }
        public EmployeeTests() 
        {
            // Reason: We mock data because we do not care about the data we are testing in this scope
            //         but instead we care about if the code works as intended.
            // use fixture to quickly create test data for our Mock object
            _fixture = new Fixture();
            // Use Mock to mock out our EmployeeService
            _employeeService = new Mock<IEmployeeService>();
        }

        [TestMethod]
        public async Task Test1_POST_inserts_employee()
        {
            var employee = _fixture.Create<AddEmployeeDto>();
            _employeeService.Setup(srvc => srvc.AddEmployee(It.IsAny<AddEmployeeDto>())).Returns(employee);
            _employeesController = new EmployeesController(_employeeService.Object);

            var result = await _employeesController.AddEmployee(employee);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test2_UPDATE_updates_employee()
        {
            // Create our dummy data (expected input and output types)
            var employee = _fixture.Create<GetEmployeeDto>();
            var updateEmployeeDto = _fixture.Create<UpdateEmployeeDto>();
            // setup our mock service. Pass in "any" type of the correct parameter and expect the correct return type
            _employeeService.Setup(srvc => srvc.UpdateEmployee(0, It.IsAny<UpdateEmployeeDto>())).Returns(employee);
            // setup our controller locally with the given dependentService context
            _employeesController = new EmployeesController(_employeeService.Object);
            // call api and assert the results
            var result = await _employeesController.UpdateEmployee(0, updateEmployeeDto);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test3_GET_retrieves_all_employee() {
            var employeeList = _fixture.CreateMany<GetEmployeeDto>(3).ToList();
            _employeeService.Setup(srvc => srvc.GetAllEmployees()).Returns(employeeList);
            _employeesController = new EmployeesController(_employeeService.Object);

            var result = await _employeesController.GetAll();
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);

        }

        [TestMethod]
        public async Task Test4_GET_retrieves_employee()
        {
            var employee = _fixture.Create<GetEmployeeDto>();
            _employeeService.Setup(srvc => srvc.GetEmployee(0)).Returns(employee);
            _employeesController = new EmployeesController(_employeeService.Object);

            var result = await _employeesController.Get(0);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test4_GET_retrieves_employee_paycheck()
        {
            var paycheck = _fixture.Create<decimal>();
            _employeeService.Setup(srvc => srvc.GetEmployeePaycheck(0)).Returns(paycheck);
            _employeesController = new EmployeesController(_employeeService.Object);

            var result = await _employeesController.GetEmployeePaycheck(0);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

        [TestMethod]
        public async Task Test5_DELETE_removes_employee()
        {
            var employee = _fixture.Create<GetEmployeeDto>();
            _employeeService.Setup(srvc => srvc.DeleteEmployee(0)).Returns(employee);
            _employeesController = new EmployeesController(_employeeService.Object);

            var result = await _employeesController.DeleteEmployee(0);
            var obj = result.Result as ObjectResult;
            Assert.AreEqual(200, obj.StatusCode);
        }

    }
}