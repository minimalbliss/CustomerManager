using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CustomerManger.UI.Pages
{
    public class IndexModel(ILogger<IndexModel> logger) : PageModel
    {
        private readonly ILogger<IndexModel> logger = logger;

        public void OnGet()
        {

        }
    }
}
