using System.ComponentModel.DataAnnotations;

namespace Application.DTO
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
    }
}