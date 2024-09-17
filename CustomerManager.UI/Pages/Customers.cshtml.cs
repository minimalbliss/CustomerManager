using CustomerManager.Models.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace CustomerManger.UI.Pages
{
    public class CustomersModel(IHttpClientFactory httpClientFactory, ILogger<IndexModel> logger) : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ILogger<IndexModel> _logger = logger;

        [BindProperty]
        public Customer NewCustomer { get; set; }

        public List<Customer> Customers { get; set; } = [];

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("CustomerManager");
                HttpResponseMessage response = await httpClient.GetAsync("Customer");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customers = JsonConvert.DeserializeObject<List<Customer>?>(json) ?? Customers;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving customers.");
                RedirectToPage("Error");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                ModelState.RemoveUnrequiredCustomerKeys();
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                HttpClient httpClient = _httpClientFactory.CreateClient("CustomerManager");
                HttpResponseMessage response = await httpClient.PostAsJsonAsync("Customer", NewCustomer);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Customers");
                }
                else
                {

                    _logger.LogError("An error occurred while creating a new customer.");
                    return Page();
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new customer.");
                return RedirectToPage("Error");
            }
        }
    }
}
