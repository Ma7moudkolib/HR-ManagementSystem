using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Department
    {
        [Column("DepartmentId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage = "Maximum length for the Description is 100 characters.")]
        public string? Description { get; set; }
        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }
        public Company Company { get; set; }
        public ICollection<Employee>? Employees { get; set; }
    }
}
