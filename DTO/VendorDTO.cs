using System.ComponentModel.DataAnnotations;

namespace E_commerce_system.Data.DTO
{
    public class CreateVendorDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public string Description { get; set; }
    }

    public class VendorRatingDTO
    {
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [Required]
        public string Comment { get; set; }
    }
}