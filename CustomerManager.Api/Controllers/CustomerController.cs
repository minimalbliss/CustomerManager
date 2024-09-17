using CustomerManager.Api.Services.Interfaces;
using CustomerManager.Models.Models;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManager.Api.Controllers
{
    /// <summary>
    /// The CustomerController class.
    /// Would have been nice to have the time to setup Antiforgery tokens and a proper authentication system.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        /// <summary>
        /// The CustomerController constructor.
        /// </summary>
        /// <param name="customerService">the customer service layer</param>
        /// <param name="loggerFactory">the logger</param>
        public CustomerController(ICustomerService customerService, ILoggerFactory loggerFactory)
        {
            _customerService = customerService;
            _logger = loggerFactory.CreateLogger<CustomerController>();
        }

        /// <summary>
        /// Gets all customers, or returns an empty collection if there are none.
        /// </summary>
        /// <returns>A collection of customers</returns>
        [ProducesResponseType(typeof(IEnumerable<Customer>), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(500)]
        [HttpGet(Name = "GetAllCustomers")]
        public async Task<IActionResult> GetAlAsyncl()
        {

            IEnumerable<Customer> customers = await _customerService.GetAllAsync(CancellationToken.None);
            return Ok(customers);

        }

        /// <summary>
        /// Gets a customer by ID, or returns a 404 if the customer is not found.
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>A Customer</returns>
        [HttpGet("{id}", Name = "GetCustomer")]
        [ProducesResponseType(typeof(Customer), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAsync(int id)
        {

            Customer? customer = await _customerService.GetByIdAsync(id, CancellationToken.None);
            if (customer != null)
            {
                return Ok(customer);
            }

            return NotFound();
        }

        /// <summary>
        /// Adds a customer to the system.
        /// </summary>
        /// <param name="item">The customer model</param>
        /// <returns>Success</returns>
        [HttpPost(Name = "AddCustomer")]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> AddAsync(Customer item)
        {
            Result<bool> result = await _customerService.AddAsync(item, CancellationToken.None);
            return result.ToActionResult();
        }

        /// <summary>
        /// Updates a customer in the system.
        /// </summary>
        /// <param name="item">The customer model</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpPut(Name = "UpdateCustomer")]
        public async Task<IActionResult> UpdateAsync(Customer item)
        {
            Result<bool> result = await _customerService.UpdateAsync(item, CancellationToken.None);
            return result.ToActionResult();
        }

        /// <summary>
        /// Deletes a customer from the system.
        /// </summary>
        /// <param name="id">The ID of the customer</param>
        /// <returns>NoContent</returns>
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Result<bool> result = await _customerService.DeleteAsync(id, CancellationToken.None);
            return result.ToActionResult();
        }
    }
}
