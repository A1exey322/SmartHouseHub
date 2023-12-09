using System.ComponentModel.DataAnnotations;

namespace SmartHouseHub.Models.Customer
{
    public class CustomerLoginModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
