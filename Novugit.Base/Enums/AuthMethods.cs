using System.ComponentModel.DataAnnotations;

namespace Novugit.Base.Enums;

public enum AuthMethods
{
    [Display(Name = "Personal Access Token")]
    Token,

    [Display(Name = "Username & Password")]
    UsernamePassword,
}