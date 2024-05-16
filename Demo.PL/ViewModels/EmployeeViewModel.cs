using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is Required!")]
        [MaxLength(50, ErrorMessage = "Max Length is 50")]
        [MinLength(4, ErrorMessage = "Max Length is 4")]
        public string Name { get; set; }
        [Range(21, 60)]
        public int? Age { get; set; }
        [RegularExpression(@"^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$"
           , ErrorMessage = "Address must be like 123-street-city-country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }

        public DateTime HireDate { get; set; }
        //[ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        //[InverseProperty(nameof(Models.Department.Employees))]
        public Department Department { get; set; }  //navigation property ==> [one]
    }
}
