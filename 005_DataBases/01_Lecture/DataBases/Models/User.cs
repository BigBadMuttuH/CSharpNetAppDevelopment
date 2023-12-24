using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataBases.Models;

[Table("users")]
public class User
{
    [Key] [Column("id")] public int Id { get; set; }

    [Column("name")] public string Name { get; set; }

    // один ко многим
    public virtual ICollection<Message> Messages { get; set; }
}