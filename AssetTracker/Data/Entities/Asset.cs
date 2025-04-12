namespace AssetTracker.Data.Entities;
public class Asset
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string SerialNumber { get; set; }
    public int? AssignedUserId { get; set; }
    public User AssignedUser { get; set; }
}