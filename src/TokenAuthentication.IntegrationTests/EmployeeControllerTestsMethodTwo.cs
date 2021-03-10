using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Dtos;

namespace TokenAuthentication.IntegrationTests
{
    public class EmployeeControllerTestsMethodTwo : IntegrationTest
    {
        [Test]
        public async Task GetAllEmployees_UnAuthorized_ShouldReturnUnAuthorizedResult()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.NoRole);
            var response = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var employees = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            employees.Should().BeEmpty();

        }

        [Test]
        public async Task GetAllEmployees_AdminWithAgeSupport_ShouldReturnOkResult()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.AdminWithAgeSupport);
            var response = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var employees = JsonConvert.DeserializeObject<ResponseDto<ICollection<EmployeeDto>>>(await response.Content.ReadAsStringAsync());
            employees.Data.Count.Should().BeGreaterThan(0);
            employees.Should().BeOfType<ResponseDto<ICollection<EmployeeDto>>>();
            response.EnsureSuccessStatusCode();

        }

        [Test]
        public async Task GetAllEmployees_UserWithAgeSupport_ShouldReturnOkResult()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.UserWithAgeSupport);
            var response = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var employees = JsonConvert.DeserializeObject<ResponseDto<ICollection<EmployeeDto>>>(await response.Content.ReadAsStringAsync());
            employees.Data.Count.Should().BeGreaterThan(0);
            employees.Should().BeOfType<ResponseDto<ICollection<EmployeeDto>>>();
            response.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetAllEmployees_AdminWithAgeRestriction_ShouldReturnForbidden()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.AdminWithAgeRestriction);
            var response = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var employees = JsonConvert.DeserializeObject<ResponseDto<ICollection<EmployeeDto>>>(await response.Content.ReadAsStringAsync());
            //var employees = await response.Content.ReadAsStringAsync();
            employees.Should().BeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);

        }

        [Test]
        public async Task GetAllEmployees_UserWithAgeRestriction_ShouldReturnForbidden()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.UserWithAgeRestriction);
            var response = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var employees = await response.Content.ReadAsStringAsync();
            employees.Should().BeEmpty();
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
    }
}