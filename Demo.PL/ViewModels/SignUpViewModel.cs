using System.ComponentModel.DataAnnotations;

namespace Demo.PL.ViewModels
{
	public class SignUpViewModel
	{
		[Required(ErrorMessage = "FName is Required")]
		public string FName { get; set; }

		[Required(ErrorMessage = "LName is Required")]
		public string LName { get; set; }
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Invalid Email")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Password is Required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required(ErrorMessage = "Confirm Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(Password), ErrorMessage = "Confirm Password doesn't match Password")]
		public string ConfirmPassword { get; set; }

		[Required(ErrorMessage = "Required To Agree")]
		public bool IsAgree { get; set; }
    }
}
