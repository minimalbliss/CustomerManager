using FluentValidation;
using Moq;
using Microsoft.Extensions.Logging;
using Xunit;
using Assert = Xunit.Assert;
using LanguageExt.Common;
using CustomerManager.Api.Services;
using CustomerManager.Models.Models;
using CustomerManager.Api.Data;

namespace CustomerManager.Api.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _customerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILoggerFactory> _loggerFactoryMock;
        private readonly Mock<IValidator<Customer>> _validatorMock;

        public CustomerServiceTests()
        {
            _customerRepositoryMock = new Mock<ICustomerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository).Returns(_customerRepositoryMock.Object);
            _loggerFactoryMock = new Mock<ILoggerFactory>();
            _loggerFactoryMock.Setup(lf => lf.CreateLogger(It.IsAny<string>())).Returns(Mock.Of<ILogger>());
            _validatorMock = new Mock<IValidator<Customer>>();
        }

        [Fact]
        public async Task AddAsync_ValidCustomer_ReturnsTrue()
        {
            // Arrange
            Customer customer = new();
            _validatorMock.Setup(v => v.ValidateAsync(customer, CancellationToken.None)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.AddAsync(customer, CancellationToken.None)).ReturnsAsync(true);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Result<bool> result = await customerService.AddAsync(customer, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task AddAsync_InvalidCustomer_ReturnsFalse()
        {
            // Arrange
            Customer customer = new();
            List<FluentValidation.Results.ValidationFailure> validationFailures =
            [
                new FluentValidation.Results.ValidationFailure("Name", "Name is required"),
                new FluentValidation.Results.ValidationFailure("Email", "Email is required")
            ];
            FluentValidation.Results.ValidationResult validationResult = new(validationFailures);
            _validatorMock.Setup(v => v.ValidateAsync(customer, CancellationToken.None)).ReturnsAsync(validationResult);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Result<bool> result = await customerService.AddAsync(customer, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Name is required", result.ToString());
            Assert.Contains("Email is required", result.ToString());
        }

        [Fact]
        public async Task FindAsync_ValidCustomer_ReturnsCustomer()
        {
            // Arrange
            Customer customer = new();
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.FindAsync(customer, CancellationToken.None)).ReturnsAsync(customer);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Customer? result = await customerService.FindAsync(customer, CancellationToken.None);

            // Assert
            Assert.Equal(customer, result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsListOfCustomers()
        {
            // Arrange
            List<Customer> customers = [new Customer(), new Customer()];
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.GetAllAsync(CancellationToken.None)).ReturnsAsync(customers);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            IEnumerable<Customer> result = await customerService.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Equal(customers, result);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCustomer()
        {
            // Arrange
            int id = 1;
            Customer customer = new();
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync(customer);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Customer? result = await customerService.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Equal(customer, result);
        }

        [Fact]
        public async Task Update_ValidCustomer_ReturnsTrue()
        {
            // Arrange
            Customer customer = new();
            _validatorMock.Setup(v => v.ValidateAsync(customer, CancellationToken.None)).ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.Update(customer, CancellationToken.None));

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Result<bool> result = await customerService.UpdateAsync(customer, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Update_InvalidCustomer_ReturnsFalse()
        {
            // Arrange
            Customer customer = new();
            List<FluentValidation.Results.ValidationFailure> validationFailures =
            [
                        new FluentValidation.Results.ValidationFailure("Name", "Name is required"),
                        new FluentValidation.Results.ValidationFailure("Email", "Email is required")
                    ];
            FluentValidation.Results.ValidationResult validationResult = new(validationFailures);
            _validatorMock.Setup(v => v.ValidateAsync(customer, CancellationToken.None)).ReturnsAsync(validationResult);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Result<bool> result = await customerService.UpdateAsync(customer, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Name is required", result.ToString());
            Assert.Contains("Email is required", result.ToString());
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            Customer customer = new();
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync(customer);
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.Delete(customer, CancellationToken.None));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync(CancellationToken.None)).ReturnsAsync(1);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Result<bool> result = await customerService.DeleteAsync(id, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsFalse()
        {
            // Arrange
            int id = 1;
            _unitOfWorkMock.Setup(uow => uow.CustomerRepository.GetByIdAsync(id, CancellationToken.None)).ReturnsAsync((Customer?)null);

            CustomerService customerService = new(_unitOfWorkMock.Object, _validatorMock.Object, _loggerFactoryMock.Object);

            // Act
            Result<bool> result = await customerService.DeleteAsync(id, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Customer not found", result.ToString());
        }
    }
}
