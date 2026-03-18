using System.ComponentModel.DataAnnotations;

namespace VShop.Web.Models;

public class CartHeaderViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "UserId is Required")]
    public string UserId { get; set; } = string.Empty;

    public string CouponCode { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }

    [Required(ErrorMessage = "Informe o nome")]
    [Display(Name = "Nome")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o sobrenome")]
    [Display(Name = "Sobrenome")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o telefone")]
    [Phone(ErrorMessage = "Telefone inválido")]
    [Display(Name = "Telefone")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o e-mail")]
    [EmailAddress(ErrorMessage = "E-mail inválido")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o número do cartão")]
    [CreditCard(ErrorMessage = "Número de cartão inválido")]
    [Display(Name = "Número do Cartão")]
    public string CardNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o nome impresso no cartão")]
    [Display(Name = "Nome no Cartão")]
    public string NameOnCard { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe o CVV")]
    [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV deve ter 3 ou 4 dígitos")]
    [Display(Name = "CVV")]
    public string CVV { get; set; } = string.Empty;

    [Required(ErrorMessage = "Informe a validade")]
    [RegularExpression(@"^(0[1-9]|1[0-2])\/(2[0-9]|[3-9][0-9])$",
        ErrorMessage = "Formato inválido. Use MM/AA")]
    [Display(Name = "Validade")]
    public string ExpiryMonthYear { get; set; } = string.Empty;

    public DateTime DateTime { get; set; }
    public int CartTotalItems { get; set; }
}
