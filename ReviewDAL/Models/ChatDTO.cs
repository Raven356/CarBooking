using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewDAL.Models
{
    [Table("Chat")]
    public class ChatDTO
    {
        public int Id { get; set; }

        public Guid ChatGuid { get; set; }

        public bool IsActive { get; set; }
    }
}
