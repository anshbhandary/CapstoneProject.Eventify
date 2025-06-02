using System.ComponentModel.DataAnnotations;

namespace AdminAPI.Models.Dto
{
    public class AdminDto
    {
        public int id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
