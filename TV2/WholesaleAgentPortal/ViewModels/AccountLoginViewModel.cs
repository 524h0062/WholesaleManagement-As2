using System.ComponentModel.DataAnnotations;

namespace WholesaleAgentPortal.ViewModels;

public sealed class AccountLoginViewModel
{
    [Required, StringLength(80)]
    [Display(Name = "Username")]
    public string UserName { get; set; } = "";
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
    public string? ReturnUrl { get; set; }
}
