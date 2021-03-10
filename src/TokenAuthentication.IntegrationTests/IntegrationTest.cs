using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TokenAuthentication.API;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Dtos;
using TokenAuthentication.Dtos.Authentication;
using TokenAuthentication.Entity.Authentication;
using TokenAuthentication.Entity.Entity;

namespace TokenAuthentication.IntegrationTests
{
    public class IntegrationTest
    {
        private WebApplicationFactory<Startup> _appFactory;
        protected HttpClient _client;

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


        public IntegrationTest()
        {

            _appFactory = new WebApplicationFactory<Startup>()
                                .WithWebHostBuilder(config =>
                                {
                                    config.ConfigureServices(services =>
                                    {
                                        var descriptor = services
                                                    .FirstOrDefault(p => p.ServiceType == typeof(DbContextOptions<TokenAuthenticationDbContext>));
                                        if (descriptor != null)
                                            services.Remove(descriptor);

                                        services.AddDbContext<TokenAuthenticationDbContext>((options, context) =>
                                        {

                                            context.UseInMemoryDatabase("TestDb");
                                        });
                                        var sp = services.BuildServiceProvider();
                                        using (var scope = sp.CreateScope())
                                        {
                                            var serviceProvider = scope.ServiceProvider;
                                            var db = serviceProvider.GetRequiredService<TokenAuthenticationDbContext>();
                                            db.Database.EnsureCreated();
                                            try
                                            {
                                                Utilities.InitializeDbForTests(db);
                                            }
                                            catch (Exception)
                                            {
                                                //throw;
                                            }
                                        }
                                    });
                                });
            _client = _appFactory.CreateClient();
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
