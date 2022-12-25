using System.ComponentModel.DataAnnotations;

namespace iHub.API.Modals;

public class ProposalDto
{
    [Required]
    [MaxLength(3000)]
    public string summary { get; set; }
    [Required]
    public string description { get; set; }
    [Required]
    public string Address { get; set; }
    [Required]
    public string Company { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Phone { get; set; }
    [Required]
    public string Website { get; set; }
    [Required]
    public string Challange { get; set; }

    public IFormFile file { get; set; }
}
