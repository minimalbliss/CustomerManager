using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManger.UI.Pages
{
    public class CustomerDeleteModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CustomerDeleteModel> _logger;

        public CustomerDeleteModel(IHttpClientFactory httpClientFactory, ILogger<CustomerDeleteModel> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("CustomerManager");
                HttpResponseMessage response = await client.DeleteAsync($"customer/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Customers");
                }
                else
                {
                    _logger.LogError($"Failed to delete customer with id: {id}. Status code: {response.StatusCode}");
                    ModelState.UpdateState(await response.Content.ReadAsStringAsync());
                    return RedirectToPage("Error");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting customer with id: {id}. Error: {ex.Message}");
                return RedirectToPage("Error");
            }
        }
    }
}
