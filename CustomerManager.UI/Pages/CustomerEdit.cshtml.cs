using CustomerManager.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManger.UI.Pages
{
    public class CustomerEditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public Customer? ExistingCustomer { get; set; }

        public CustomerEditModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("CustomerManager");
                HttpResponseMessage response = await httpClient.GetAsync($"customer/{id}");

                if (response.IsSuccessStatusCode)
                {
                    ExistingCustomer = await response.Content.ReadFromJsonAsync<Customer?>();

                    if (ExistingCustomer == null)
                    {
                        _logger.LogError($"Customer with Id: {id} not found");
                        return NotFound();
                    }

                    return Page();
                }
                else
                {
                    _logger.LogError($"Error retrieving customer. Status code: {response.StatusCode}");

                    return StatusCode((int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the customer.");
                return StatusCode(500);
            }
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (ExistingCustomer == null)
                {
                    _logger.LogError($"No customer to update");
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    return Page();
                }

                HttpClient httpClient = _httpClientFactory.CreateClient("CustomerManager");
                HttpResponseMessage response = await httpClient.PutAsJsonAsync($"customer", ExistingCustomer);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Customers");
                }
                else
                {
                    ModelState.UpdateState(await response.Content.ReadAsStringAsync());
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer.");
                return StatusCode(500);
            }
        }
    }
}
