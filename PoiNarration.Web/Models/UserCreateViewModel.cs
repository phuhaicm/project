using System.ComponentModel.DataAnnotations;

namespace PoiNarration.Web.Models;

public class UserCreateViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
    [StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Họ tên là bắt buộc")]
    [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
    public string FullName { get; set; } = "";

    [StringLength(100, ErrorMessage = "Mật khẩu tối đa 100 ký tự")]
    public string Password { get; set; } = "";

    [Required(ErrorMessage = "Vai trò là bắt buộc")]
    public string Role { get; set; } = "Owner";
}