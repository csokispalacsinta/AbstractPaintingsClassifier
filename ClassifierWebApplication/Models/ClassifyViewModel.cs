using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ClassifierWebApplication.Models
{
    public class ClassifyViewModel
    {
        public string? Style { get; set; }

        [Required]
        public float[]? Descriptors { get; set; }
    }
}
