using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CaixaAPI.Model.User;
using System.ComponentModel.DataAnnotations;
[Table("Users")]
[Index(nameof(Email), IsUnique =  true)]
public class User (int ID, string Name, string Email, string Password)
{
    [Key]
    [Required]
    [Column("ID")]
    public int ID { get; set; } = ID;
    [Required]
    [MaxLength(40)]
    
    [Column("Name")]
    public string Name { get; set; } = Name;
    [EmailAddress]
    [Required]
    [MaxLength(50)]
    
    [Column("Email")]
    public string Email { get; set; } = Email;
    [Required]
    
    [Column("Password")]
    public string Password { get; set; } = Password;
}