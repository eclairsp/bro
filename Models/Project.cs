using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bro
{
    class Project
    {
        public int ProjectID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Directory { get; set; }
        public DateTime Created { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}
