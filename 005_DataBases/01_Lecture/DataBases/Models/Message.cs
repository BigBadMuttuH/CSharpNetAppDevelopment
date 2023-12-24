using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBases.Models;

[Table("messages")]
public class Message
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("user_id")] public int UserId { get; set; }

    [Column("message")] public string? MessageContext { get; set; }

    // ленивая загрузка связей, один к одному
    [ForeignKey("UserId")] public virtual User User { get; set; }
}