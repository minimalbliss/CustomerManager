using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace CustomerManger.UI
{
    public static class ModelStateExtensions
    {
        public static void UpdateState(this ModelStateDictionary modelState, string json)
        {
            try
            {
                Dictionary<string, string[]>? errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(json);
                if (errors != null)
                {
                    foreach (KeyValuePair<string, string[]> error in errors)
                    {
                        foreach (string errorMessage in error.Value)
                        {
                            modelState.AddModelError(error.Key, errorMessage);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public static void RemoveUnrequiredCustomerKeys(this ModelStateDictionary modelState)
        {
            foreach (var key in modelState.Keys)
            {
                if (key.Contains(".Phone") || key.Contains(".PostCode") || key.Contains(".Country"))
                {
                    modelState[key]!.Errors.Clear();
                    modelState.Remove(key);
                }
            }
        }
    }
}


