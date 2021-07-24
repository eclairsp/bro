using System;
using System.ComponentModel.DataAnnotations;

namespace bro
{
    class Task
    {
        public int TaskID { get; set; }
        [Required]
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Completed { get; set; }
        public DateTime Created { get; set; }
        public int ProjectID { get; set; }
        public Project Project { get; set; }
    }
}
