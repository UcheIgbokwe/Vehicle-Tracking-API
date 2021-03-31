using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}