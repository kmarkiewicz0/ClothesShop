﻿using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineShop.Core
{
    public class Product
    {
        public Product()
        {
            Photos = new List<Photo>();
        }
        [Required]
        public int Id { get; set; }
        [Required, StringLength(80)]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public string Description { get; set; }
        public ClothingSize Size { get; set; }
        public GenderType Gender { get; set; }
        public FabricType Fabric { get; set; }
        public ClothingType Type { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
