﻿using System.ComponentModel.DataAnnotations;

namespace WebApiEfCodeFirst.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
    }
}
