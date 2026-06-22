using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using SimplyFly.API.Data;
using SimplyFly.API.DTOs;
using SimplyFly.API.Mappings;
using SimplyFly.API.Models;
using SimplyFly.API.Services.Implementations;

namespace SimplyFly.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private ApplicationDbContext _context;
        private UserService _userService;
        private IMapper _mapper;
        private IConfiguration _configuration;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            _mapper = mapperConfig.CreateMapper();

            var settings = new Dictionary<string, string?>
            {
                { "Jwt:Key", "ThisIsMySuperSecretKeyForSimplyFlyApplication2026" },
                { "Jwt:Issuer", "SimplyFlyAPI" },
                { "Jwt:Audience", "SimplyFlyUsers" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(settings)
                .Build();

            _userService = new UserService(_context, _configuration, _mapper);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void When_Register_ValidDetails_CreatesUserSuccessfully()
        {
            RegisterUserDto dto = new RegisterUserDto
            {
                FullName = "Arun Kumar",
                Email = "arun@test.com",
                Password = "Password123",
                Role = "Passenger"
            };

            ApiResponse<User> result = _userService.Register(dto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("User Registered Successfully"));
            Assert.That(_context.Users.Count(), Is.EqualTo(1));
            Assert.That(_context.Users.First().Email, Is.EqualTo("arun@test.com"));
            Assert.That(_context.Users.First().PasswordHash, Is.Not.EqualTo("Password123"));
        }

        [Test]
        public void When_Login_ValidCredentials_ReturnsToken()
        {
            RegisterUserDto registerDto = new RegisterUserDto
            {
                FullName = "Geetha",
                Email = "geetha@test.com",
                Password = "Password123",
                Role = "Passenger"
            };

            _userService.Register(registerDto);

            LoginUserDto loginDto = new LoginUserDto
            {
                Email = "geetha@test.com",
                Password = "Password123"
            };

            ApiResponse<object> result = _userService.Login(loginDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Message, Is.EqualTo("Login Successful"));
            Assert.That(result.Data, Is.Not.Null);
        }

        [Test]
        public void When_Login_InvalidEmail_ReturnsFailure()
        {
            LoginUserDto loginDto = new LoginUserDto
            {
                Email = "wrong@test.com",
                Password = "Password123"
            };

            ApiResponse<object> result = _userService.Login(loginDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Invalid Email or Password"));
        }
    }
}
