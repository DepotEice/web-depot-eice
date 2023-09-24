namespace Web.DepotEice.BLL.Models;

/// <summary>
/// User update model.
/// </summary>
public class UserUpdateModel
{
    /// <summary>
    /// Gets or sets the email of the user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the first name of the user.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the last name of the user.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's mobile phone number
    /// </summary>
    public string MobileNumber { get; set; } = string.Empty;

    /// <summary>
    /// User's phone number
    /// </summary>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// User's gender
    /// </summary>
    public string Gender { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the birth date of the user.
    /// </summary>
    public DateTime BirthDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is active.
    /// </summary>
    public bool IsActive { get; set; }
}
