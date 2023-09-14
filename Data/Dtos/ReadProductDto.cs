using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsApi.Data.Dtos
{
    public class ReadProductDto
    {

        public string ProductName { get; set; } = default!;

        public string ProductDescription { get; set; } = default!;

        public string ProductCategory { get; set; } = default!;

        public float Price { get; set; }
        public DateTime HourToSearch { get; set; } = DateTime.Now;
    }
}