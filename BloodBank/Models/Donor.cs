using System.ComponentModel.DataAnnotations;

namespace BloodBank.Models;

public partial class Donor
{
    public int DonorId { get; set; }

    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Blood group is required.")]
    [RegularExpression("^(A|B|AB|O)[+-]$", ErrorMessage = "Blood group must be one of A+, A-, B+, B-, AB+, AB-, O+, O-.")]
    [Display(Name = "Blood Group")]
    public string BloodGroup { get; set; } = null!;

    [Phone]
    [StringLength(20)]
    [Display(Name = "Contact No")]
    public string? ContactNo { get; set; }

    [StringLength(50)]
    public string? City { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Last Donation Date")]
    public DateOnly? LastDonationDate { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();
}
