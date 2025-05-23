﻿using System.ComponentModel.DataAnnotations;

namespace SPInRepositoryPro.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        [Display(Name = "Department Name")]
        public int DepartmentId { get; set; }
    }
}
