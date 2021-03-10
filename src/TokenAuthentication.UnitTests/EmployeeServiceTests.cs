using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TokenAuthentication.Common.Automapper;
using TokenAuthentication.Common.Constants;
using TokenAuthentication.Dtos;
using TokenAuthentication.Entity.Entity;
using TokenAuthentication.Interfaces;
using TokenAuthentication.Services;

namespace TokenAuthentication.UnitTests
{
    [TestFixture]
    public class Tests
    {
        private static EmployeeDto employeeDto = new EmployeeDto()
        {
            Age = 20,
            DepartmentId = Guid.Parse("78b431e4-c8c2-4660-bbd2-67a6ca8ab7a5"),
            Departmentname = "Test",
            Email = "test@tes.com",
            EmployeeId = Guid.Parse("64302ae5-08b7-4dc9-a286-aebbd507d9bd"),
            EmployeeName = "Test User",
            Gender = "Female"
        };
        private EmployeeDto _employeeDto;
        private Mock<IUnitOfWork<TokenAuthenticationDbContext>> _unitOfWork;
        private MapperConfiguration _mapperConfiguration;
        private IMapper _mapper;
        private EmployeeService _employeeService;
        private IEnumerable<Employee> _getAll;
        private Employee _getById;
        private Mock<IRepository<Employee>> _employeeRepository;

        [SetUp]
        public void Setup()
        {
            _employeeDto = new EmployeeDto()
            {
                Age = 20,
                DepartmentId = Guid.Parse("78b431e4-c8c2-4660-bbd2-67a6ca8ab7a5"),
                Departmentname = "Test",
                Email = "test@tes.com",
                EmployeeId = Guid.Parse("64302ae5-08b7-4dc9-a286-aebbd507d9bd"),
                EmployeeName = "Test User",
                Gender = "Female"
            };
            _unitOfWork = new Mock<IUnitOfWork<TokenAuthenticationDbContext>>();
            _employeeRepository = new Mock<IRepository<Employee>>();
            _unitOfWork.Setup(p => p.GetRepository<Employee>()).Returns(_employeeRepository.Object);
            _mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile(new TokenAuthenticationProfile()); });
            _mapper = _mapperConfiguration.CreateMapper();
            _employeeService = new EmployeeService(_unitOfWork.Object, _mapper);
            _getAll = new List<Employee>()
            {
                _mapper.Map<Employee>(employeeDto)
            };
            _getById = _mapper.Map<Employee>(employeeDto);
        }

        [Test]
        public async Task Create_WhenModelIsInValid_ShouldReturnSuccessAsFalse()
        {
            //Arrange
            _employeeDto = null;
            var employee = _mapper.Map<Employee>(_employeeDto);
            //Act
            var result = await _employeeService.Create(_employeeDto);

            //Assert
            _employeeRepository.Verify(p => p.AddAsync(It.IsAny<Employee>()), Times.Never);
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.INVALID_MODEL);
        }

        [Test]
        public async Task Create_WhenModelIsValid_ShouldReturnSuccessAsTrue()
        {
            //Arrange
            _unitOfWork.Setup(p => p.SaveAndCommitAsync()).ReturnsAsync(1);

            //Act
            var result = await _employeeService.Create(_employeeDto);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be(Messages.EMPLOYEE_CREATE_SUCCESS);
            _employeeRepository.Verify(p => p.AddAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Test]
        public async Task Create_WhenFails_ShouldReturnSuccessAsFalse()
        {
            //Arrange
            _unitOfWork.Setup(p => p.SaveAndCommitAsync()).ReturnsAsync(0);

            //Act
            var result = await _employeeService.Create(_employeeDto);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.EMPLOYEE_CREATE_FAILURE);
            _employeeRepository.Verify(p => p.AddAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Test]
        [TestCaseSource("UpdateInvalidTestCases")]
        public async Task Update_WhenModelIsInvalid_ShouldReturnSuccessAsFalse(Guid guid, EmployeeDto employeeDto)
        {
            //Act
            var result = await _employeeService.Update(guid, employeeDto);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.INVALID_MODEL);
            _employeeRepository.Verify(p => p.Update(It.IsAny<Employee>()), Times.Never);
        }

        [Test]
        [TestCaseSource("UpdateTestCases")]
        public async Task Update_WhenModelIsValid_ShouldReturnSuccessAsTrue(Guid guid, EmployeeDto employeeDto)
        {
            //Arrange
            _unitOfWork.Setup(p => p.SaveAndCommitAsync()).ReturnsAsync(1);

            //Act
            var result = await _employeeService.Update(guid, employeeDto);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be(Messages.EMPLOYEE_UPDATE_SUCCESS);
            _employeeRepository.Verify(p => p.Update(It.IsAny<Employee>()), Times.Once);
        }

        [Test]
        [TestCaseSource("UpdateTestCases")]
        public async Task Update_WhenFails_ShouldReturnSuccessAsFalse(Guid guid, EmployeeDto employeeDto)
        {
            //Arrange
            _unitOfWork.Setup(p => p.SaveAndCommitAsync()).ReturnsAsync(0);

            //Act
            var result = await _employeeService.Update(guid, employeeDto);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.EMPLOYEE_UPDATE_FAILURE);
            _employeeRepository.Verify(p => p.Update(It.IsAny<Employee>()), Times.Once());
        }

        [Test]
        [TestCaseSource("DeleteInvalidTestCases")]
        public async Task Delete_WhenInputDataIsInvalid_ShouldReturnSuccessAsFalse(Guid guid)
        {
            //Act
            var result = await _employeeService.Delete(guid);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.INVALID_DATA);
            _employeeRepository.Verify(p => p.RemoveAsync(It.IsAny<Guid>()), Times.Never());
        }

        [Test]
        [TestCaseSource("DeleteTestCases")]
        public async Task Delete_WhenInputDataIsValid_ShouldReturnSuccessAsTrue(Guid guid)
        {
            //Arrange
            _unitOfWork.Setup(p => p.SaveAndCommitAsync()).ReturnsAsync(1);

            //Act
            var result = await _employeeService.Delete(guid);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be(Messages.EMPLOYEE_DELETE_SUCCESS);
            _employeeRepository.Verify(p => p.RemoveAsync(It.IsAny<Guid>()), Times.Once());
        }

        [Test]
        [TestCaseSource("DeleteTestCases")]
        public async Task Delete_WhenFails_ShouldReturnSuccessAsFalse(Guid guid)
        {
            //Arrange
            _unitOfWork.Setup(p => p.SaveAndCommitAsync()).ReturnsAsync(0);

            //Act
            var result = await _employeeService.Delete(guid);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Message.Should().Be(Messages.EMPLOYEE_DELETE_FAILURE);
            _employeeRepository.Verify(p => p.RemoveAsync(It.IsAny<Guid>()), Times.Once());
        }

        [Test]
        public async Task GetAll_WhenDataIsThere_ShouldReturnTheData()
        {
            //Arrange
            _employeeRepository.Setup(p => p.GetListAsync(
                It.IsAny<Expression<Func<Employee, bool>>>()
                , It.IsAny<Func<IQueryable<Employee>, IOrderedQueryable<Employee>>>()
                , It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>()))
                .ReturnsAsync(_getAll);

            //Act
            var result = await _employeeService.GetAll();

            //Assert
            result.Should().BeOfType<ResponseDto<ICollection<EmployeeDto>>>();
            result.Success.Should().BeTrue();
            result.Data.Count.Should().BeGreaterThan(0);
        }

        [Test]
        public async Task GetAll_WhenDataIsNotThere_ShouldReturnEmptyResult()
        {
            //Arrange
            _employeeRepository.Setup(p => p.GetListAsync(
                It.IsAny<Expression<Func<Employee, bool>>>()
                , It.IsAny<Func<IQueryable<Employee>, IOrderedQueryable<Employee>>>()
                , It.IsAny<Func<IQueryable<Employee>, IIncludableQueryable<Employee, object>>>()))
                .ReturnsAsync((IEnumerable<Employee>)null);

            //Act
            var result = await _employeeService.GetAll();

            //Assert
            result.Should().BeOfType<ResponseDto<ICollection<EmployeeDto>>>();
            result.Success.Should().BeTrue();
            result.Data.Count.Should().Be(0);
        }

        [Test]
        public async Task GetById_WhenDataIsThere_ShouldReturnTheData()
        {
            //Arrange
            _employeeRepository.Setup(p => p.FirstOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync(_getById);

            //Act
            var result = await _employeeService.GetById(_employeeDto.EmployeeId);

            //Assert
            result.Should().BeOfType<ResponseDto<EmployeeDto>>();
            result.Success.Should().BeTrue();
            result.Data.EmployeeId.Should().Be(_employeeDto.EmployeeId);

        }

        [Test]
        public async Task GetById_WhenDataIsNotThere_ShouldReturnTheEmptyResult()
        {
            //Arrange
            _employeeRepository.Setup(p => p.FirstOrDefaultAsync(It.IsAny<Expression<Func<Employee, bool>>>()))
                .ReturnsAsync((Employee)null);

            //Act
            var result = await _employeeService.GetById(_employeeDto.EmployeeId);

            //Assert
            result.Should().BeOfType<ResponseDto<EmployeeDto>>();
            result.Success.Should().BeTrue();
            result.Data.Should().BeNull();

        }

        #region TestCasesData
        public static IEnumerable<TestCaseData> UpdateInvalidTestCases
        {

            get
            {
                yield return new TestCaseData(Guid.Empty, employeeDto);
                yield return new TestCaseData(Guid.Empty, new EmployeeDto()
                {
                    Age = 20,
                    DepartmentId = Guid.Parse("78b431e4-c8c2-4660-bbd2-67a6ca8ab7a5"),
                    Departmentname = "Test",
                    Email = "test@tes.com",
                    EmployeeId = Guid.Empty,
                    EmployeeName = "Test User",
                    Gender = "Female"
                });
                yield return new TestCaseData(Guid.Empty, null);
            }
        }

        public static IEnumerable<TestCaseData> UpdateTestCases
        {
            get
            {
                yield return new TestCaseData(Guid.Parse("64302ae5-08b7-4dc9-a286-aebbd507d9bd"), employeeDto);
            }
        }

        public static IEnumerable<TestCaseData> DeleteInvalidTestCases
        {
            get
            {
                yield return new TestCaseData(Guid.Empty);
            }
        }

        public static IEnumerable<TestCaseData> DeleteTestCases
        {
            get
            {
                yield return new TestCaseData(employeeDto.EmployeeId);
            }
        }

        #endregion

    }
}