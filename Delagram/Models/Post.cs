using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Delagram.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public string ImageUrl { get; set; }

        [MaxLength(140)]
        public string Caption { get; set; }

        [Required]
        public DateTime Time { get; set; }

        // Filter - Foreign Key Relationship; creates a column refering to the Filter table
        public Filter ImageFilter { get; set; }
    }
}