using Data;
using emeriTEA_back_end.Controllers;
using emeriTEA_back_end.IServices;
using emeriTEA_back_end.Services;
using Entities;
using Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace TestControllerAdministrador
{
    [TestClass]
    public class AdministradorControlleTests
    {
        private IConfiguration _configuration;
        private IAdministradorService _administradorService;
        private ServiceContext _serviceContext;
        private AdministradorControlle _administradorControlle;
        private TokenService _tokenService;

        public AdministradorControlleTests()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<ServiceContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _serviceContext = new ServiceContext(options);
            _administradorService = new AdministradorService(_serviceContext);
            //_administradorControlle = new AdministradorControlle(null, _administradorService, _tokenService, _serviceContext);
           
        }


        [TestMethod]
        public void insertAdmin()
        {
            // Arrange
            var administrador = new Administrador
            {
                Name_administrador = "Test Administrator",
                Email = "test@test.com",
                Password = "password"
            };
            _serviceContext.Administrador.Add(administrador);
            _serviceContext.SaveChanges();

            // Act
            var result = _administradorControlle.Post(administrador);

            // Assert
            if (result is StatusCodeResult statusCodeResult)
            {
                Assert.AreEqual(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
            }
            else if (result is OkObjectResult okObjectResult)
            {
                Assert.IsNotNull(okObjectResult.Value);
            }
            else if (result is ObjectResult objectResult)
            {
                Assert.IsNotNull(objectResult.Value);
            }
            else if (result is ContentResult contentResult)
            {
                Assert.IsNotNull(contentResult.Content);
            }
            else
            {
                Assert.Fail("El resultado no es de tipo StatusCodeResult, OkObjectResult, ObjectResult ni ContentResult");
            }
        }

        [TestMethod]
        public void Login()
        {
            // Arrange
            var administrador = new Administrador
            {
                Name_administrador = "Test Administrator",
                Email = "test@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password")
            };
            _serviceContext.Administrador.Add(administrador);
            _serviceContext.SaveChanges();

            var loginRequest = new LoginRequestModel
            {
                Email = "test@test.com",
                Password = "password"
            };

            // Act
            var result = _administradorControlle.Login(loginRequest);

            // Assert
            if (result is StatusCodeResult statusCodeResult)
            {
                Assert.AreEqual(StatusCodes.Status409Conflict, statusCodeResult.StatusCode);
            }
            else if (result is OkObjectResult okObjectResult)
            {
                Assert.IsNotNull(okObjectResult.Value);
            }
            else if (result is ObjectResult objectResult)
            {
                Assert.IsNotNull(objectResult.Value);
            }
            else if (result is ContentResult contentResult)
            {
                Assert.IsNotNull(contentResult.Content);
            }
            else
            {
                Assert.Fail("El resultado no es de tipo StatusCodeResult, OkObjectResult, ObjectResult ni ContentResult");
            }
        }

    }





}