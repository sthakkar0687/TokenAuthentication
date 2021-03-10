using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TokenAuthentication.API;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;

namespace TokenAuthentication.IntegrationTests
{
    [TestFixture]
    public class EmployeeControllerTestsMethodOne
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private HttpClient _client;
        private LoginModel _adminWithAgeSupportLoginModel = new LoginModel()
        {
            UserName = "sagarvthakkar.12@gmail.com",
            Password = "Pass@123"
        };
        private LoginModel _userWithAgeSupportLoginModel = new LoginModel()
        {
            UserName = "hiralthakkar@gmail.com",
            Password = "Pass@123"
        };

        private LoginModel _adminWithAgeRestrictionLoginModel = new LoginModel()
        {
            UserName = "yogeshpatel@gmail.com",
            Password = "Pass@123"
        };
        private LoginModel _userWithAgeRestrictionLoginModel = new LoginModel()
        {
            UserName = "bansarishah@gmail.com",
            Password = "Pass@123"
        };
        public EmployeeControllerTestsMethodOne()
        {
            _factory = new CustomWebApplicationFactory<Startup>();
            _client = _factory.CreateClient();
        }

        [Test]
        public async Task GetAll_WhenNotAuthorized_ReturnsUnAuthorizedResult()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.NoRole);
            var result = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var response = await result.Content.ReadAsAsync<ResponseDto<IEnumerable<EmployeeDto>>>();
            response.Should().BeNull();
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Test]
        public async Task GetAll_WhenLoggedInAsAdminWithAgeSupport_ReturnsOkResult()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.AdminWithAgeSupport);
            var result = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var response = await result.Content.ReadAsAsync<ResponseDto<ICollection<EmployeeDto>>>();
            response.Should().NotBeNull();
            response.Data.Count.Should().BeGreaterThan(0);
            response.Should().BeOfType<ResponseDto<ICollection<EmployeeDto>>>();
            result.EnsureSuccessStatusCode();
        }

        [Test]
        public async Task GetAll_WhenLoggedInAsAdminWithAgeRestriction_ReturnsFoirbidden()
        {
            await AddTokenInAuthorizationHeaderAsync(RoleType.AdminWithAgeRestriction);
            var result = await _client.GetAsync(ApiRoutes.Employees.GetAll);
            var response = await result.Content.ReadAsAsync<ResponseDto<ICollection<EmployeeDto>>>();
            response.Should().BeNull();            
            result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        public async Task AddTokenInAuthorizationHeaderAsync(RoleType roleType)
        {
            LoginModel loginModel = null;
            switch (roleType)
            {
                case RoleType.AdminWithAgeSupport:
                    loginModel = _adminWithAgeSupportLoginModel;
                    break;
                case RoleType.UserWithAgeSupport:
                    loginModel = _userWithAgeSupportLoginModel;
                    break;
                case RoleType.AdminWithAgeRestriction:
                    loginModel = _adminWithAgeRestrictionLoginModel;
                    break;
                case RoleType.UserWithAgeRestriction:
                    loginModel = _userWithAgeRestrictionLoginModel;
                    break;
                default:
                    break;
            }
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetTokenAsync(loginModel));
        }

        private async Task<string> GetTokenAsync(LoginModel loginModel)
        {
            if (loginModel == null)
                return string.Empty;
            var requestContent = new StringContent(JsonConvert.SerializeObject(loginModel));
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await _client.PostAsync(ApiRoutes.Authentication.Login, requestContent);
            var midResult = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto<LoginResponseDto>>(midResult);
            return response?.Data?.Token ?? "";
        }
    }
}
