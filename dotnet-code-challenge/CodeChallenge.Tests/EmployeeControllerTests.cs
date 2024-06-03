
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class EmployeeControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateEmployee_Returns_Created()
        {
            // Arrange
            var employee = new Employee()
            {
                Department = "Complaintws",
                FirstName = "Debbie",
                LastName = "Downer",
                Position = "Receiver",
            };

            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/employee",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newEmployee = response.DeserializeContent<Employee>();
            Assert.IsNotNull(newEmployee.EmployeeId);
            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
            Assert.AreEqual(employee.Department, newEmployee.Department);
            Assert.AreEqual(employee.Position, newEmployee.Position);
        }

        [TestMethod]
        public void GetEmployeeById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFirstName = "John";
            var expectedLastName = "Lennon";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/employee/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var employee = response.DeserializeContent<Employee>();
            Assert.AreEqual(expectedFirstName, employee.FirstName);
            Assert.AreEqual(expectedLastName, employee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_Ok()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Department = "Engineering",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer VI",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var putRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var putResponse = putRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, putResponse.StatusCode);
            var newEmployee = putResponse.DeserializeContent<Employee>();

            Assert.AreEqual(employee.FirstName, newEmployee.FirstName);
            Assert.AreEqual(employee.LastName, newEmployee.LastName);
        }

        [TestMethod]
        public void UpdateEmployee_Returns_NotFound()
        {
            // Arrange
            var employee = new Employee()
            {
                EmployeeId = "Invalid_Id",
                Department = "Music",
                FirstName = "Sunny",
                LastName = "Bono",
                Position = "Singer/Song Writer",
            };
            var requestContent = new JsonSerialization().ToJson(employee);

            // Execute
            var postRequestTask = _httpClient.PutAsync($"api/employee/{employee.EmployeeId}",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Salary = 123456.78,
                EffectiveDate = DateTime.Parse("2020-01-01"),
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            var responseComp = response.DeserializeContent<Compensation>();

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(compensation.EmployeeId, responseComp.EmployeeId);
            Assert.AreEqual(compensation.Salary, responseComp.Salary);
            Assert.AreEqual(compensation.EffectiveDate, responseComp.EffectiveDate);
        }

        [TestMethod]
        public void GetCompensation_Returns_Ok()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Salary = 123456.78,
                EffectiveDate = DateTime.Parse("2020-01-01"),
            };

            // Execute
            var postRequestTask = _httpClient.GetAsync($"api/compensation/{compensation.EmployeeId}");
            var response = postRequestTask.Result;

            var responseComp = response.DeserializeContent<Compensation>();

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(compensation.EmployeeId, responseComp.EmployeeId);
            Assert.AreEqual(compensation.Salary, responseComp.Salary);
            Assert.AreEqual(compensation.EffectiveDate, responseComp.EffectiveDate);
            Assert.AreEqual("Pete", responseComp.Employee.FirstName);
        }

        [TestMethod]
        public void UpdateCompensation_Returns_Created()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                Salary = 999999.99,
                EffectiveDate = DateTime.Parse("2020-01-01"),
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            var responseComp = response.DeserializeContent<Compensation>();

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            Assert.AreEqual(compensation.EmployeeId, responseComp.EmployeeId);
            Assert.AreEqual(compensation.Salary, responseComp.Salary);
            Assert.AreEqual(compensation.EffectiveDate, responseComp.EffectiveDate);
        }

        [TestMethod]
        public void CreateCompensation_Returns_NotFound()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "BAD_ID",
                Salary = 999999.99,
                EffectiveDate = DateTime.Parse("2020-01-01"),
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync($"api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetCompensation_Returns_NotFound()
        {
            var compensation = new Compensation()
            {
                EmployeeId = "BAD_ID",
                Salary = 123456.78,
                EffectiveDate = DateTime.Parse("2020-01-01"),
            };

            // Execute
            var postRequestTask = _httpClient.GetAsync($"api/compensation/{compensation.EmployeeId}");
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void GetReportingStructure_Returns_Ok()
        {
            var pete = new Employee()
            {
                EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309",
                FirstName = "Pete",
                LastName = "Best",
                Position = "Developer II",
                Department = "Engineering"
            };

            // Execute
            var postRequestTask = _httpClient.GetAsync($"api/reporting/03aa1462-ffa9-4978-901b-7c001562cf6f");
            var response = postRequestTask.Result;

            var responseReportStucture = response.DeserializeContent<ReportingStructure>();
            var firstReport = responseReportStucture.Employee.DirectReports.First();
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual(pete.FirstName, firstReport.FirstName);
            Assert.AreEqual(pete.LastName, firstReport.LastName);
            Assert.AreEqual(pete.Position, firstReport.Position);
            Assert.AreEqual(pete.Department, firstReport.Department);
            Assert.AreEqual(pete.EmployeeId, firstReport.EmployeeId);
        }

        [TestMethod]
        public void GetReportingStructure_Returns_NotFound()
        {
            // Execute
            var postRequestTask = _httpClient.GetAsync($"api/reporting/BAD_ID");
            var response = postRequestTask.Result;
            
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
