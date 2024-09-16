namespace WPF;

internal partial class UserViewModel : IDataErrorInfo
{
    private Dictionary<string, string> _Errors = [];

    public string this[string propertyName]
    {
        get
        {
            var error = propertyName switch
            {
                nameof(FirstName) when string.IsNullOrEmpty(FirstName) => "First name is required.",
                nameof(FirstName) when FirstName.Length < 2 => "First name must be at least 2 characters long.",
                nameof(LastName) when string.IsNullOrEmpty(LastName) => "Last name is required.",
                nameof(LastName) when LastName.Length < 2 => "Last name must be at least 2 characters long.",
                nameof(Address) when string.IsNullOrEmpty(Address) => "Address is required.",
                nameof(Address) when Address.Length < 5 => "Address must be at least 5 characters long.",
                nameof(MobileNumber) when string.IsNullOrEmpty(MobileNumber) => "Mobile Number is required.",
                nameof(MobileNumber) when MobileNumber.Length != 10 => "Mobile Number must be 10 characters long.",
                nameof(Email) when string.IsNullOrEmpty(Email) => "Email is required.",
                nameof(Email) when !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$") => "Invalid email format.",
                nameof(DateOfBirth) when !DateOfBirth.HasValue => "Date of birth is required.",
                nameof(DateOfBirth) when DateOfBirth >= DateTime.Now.Date => "Date of birth must be in the past.",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(error))
            {
                _Errors.Remove(propertyName);
            }
            else
            {
                _Errors[propertyName] = error;
            }

            return error;
        }
    }

    public string Error => _Errors.Any() ? "Please check input data." : string.Empty;
}