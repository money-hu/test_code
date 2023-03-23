using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace coredemo2._2.Models
{
    [Table("Employee_Master")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "The {0} must be atleast {2} characters long.", MinimumLength = 6)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The {0} can not be blank.")]
        [DataType(DataType.EmailAddress)]
        [StringLength(50, ErrorMessage = "The {0} must be atleast {2} characters long.", MinimumLength = 12)]
        public string Email { get; set; }

        [Required(ErrorMessage = "The {0} can not be blank.")]
        [StringLength(1000, ErrorMessage = "The {0} must be atleast {2} characters long.", MinimumLength = 20)]
        public string Description { get; set; }
    }

    public class EmployeeContext : DbContext
    {
        public DbSet<Employee> listEmployee { get; set; }
    }
}
