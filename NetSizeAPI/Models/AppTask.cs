using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetSizeAPI.Models
{
    public class AppTask
    {
        public int ID { get; set; }

        public DateTime? CreatedAt { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public bool Done { get; set; }
    }
}
