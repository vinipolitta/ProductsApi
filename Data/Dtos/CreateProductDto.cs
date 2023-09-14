using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Data.Dtos
{
    public class CreateProductDto
    {

        [Required(ErrorMessage = "O Nome do produto e obrigatorio")]
        [StringLength(4, ErrorMessage = "Deve ter de no minimo 4 caracteres")]
        public string ProductName { get; set; } = default!;

        [Required(ErrorMessage = "A Descricao do produto e obrigatorio")]
        public string ProductDescription { get; set; } = default!;

        [Required(ErrorMessage = "A Categoria do produto e obrigatorio")]
        public string ProductCategory { get; set; } = default!;

        [Required(ErrorMessage = "O preco do produto e obrigatorio")]
        public float Price { get; set; }
    }
}