using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CustomerManager.Models.Models
{
    public class Customer
    {
        public Customer()
        {
            
        }

        [Required]
        [Key]
        public int Id { get; set; } = 0;

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; } = string.Empty;

        [DisplayName("Post Code")]
        public string? PostCode { get; set; } = string.Empty;

        [DisplayName("Country Code")]
        public string? Country { get; set; } = string.Empty;

        public Customer Copy(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Phone = customer.Phone;
            PostCode = customer.PostCode;
            Country = customer.Country;
            return this;
        }
    }
}
