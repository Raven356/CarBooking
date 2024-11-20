using ReviewDAL.Models;

namespace ReviewBLL.Models
{
    public class MessageBLL
    {
        public int Id { get; set; }

        public required string Message { get; set; }

        public int UserId { get; set; }

        public required ChatDTO Chat { get; set; }
    }
}
