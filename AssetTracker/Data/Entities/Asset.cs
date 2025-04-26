using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssetTracker.Data.Entities;
public class Asset
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string SerialNumber { get; set; }
  
    public int? AssignedUserId { get; set; }

    [ForeignKey("AssignedUserId")]
    public virtual User AssignedUser { get; set; }
}