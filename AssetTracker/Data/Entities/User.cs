using System.ComponentModel.DataAnnotations;

namespace AssetTracker.Data.Entities;

public class User
{
    [Key]   
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<Asset> AssignedAssets { get; set; }
}