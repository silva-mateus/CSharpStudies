using System.ComponentModel.DataAnnotations;

namespace VShop.Web.Models;

public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [Display(Name = "Nome")]
    public string? Name { get; set; }

    [Required(ErrorMessage = "O preço é obrigatório")]
    [Display(Name = "Preço")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "A descrição é obrigatória")]
    [Display(Name = "Descrição")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "O estoque é obrigatório")]
    [Display(Name = "Estoque")]
    public long Stock { get; set; }

    [Required(ErrorMessage = "A URL da imagem é obrigatória")]
    [Display(Name = "Imagem")]
    public string? ImageURL { get; set; }

    [Display(Name = "Categoria")]
    public string? CategoryName { get; set; }

    [Display(Name = "Categoria")]
    public int CategoryId { get; set; }
}
