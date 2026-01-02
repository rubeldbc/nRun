namespace nRun.Models;

/// <summary>
/// Represents an item in the bulk import list
/// </summary>
public class BulkImportItem
{
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public TkProfile? Profile { get; set; }

    public BulkImportItem() { }

    public BulkImportItem(string username)
    {
        Username = username.Trim().TrimStart('@');
    }

    public void UpdateFromProfile(TkProfile profile)
    {
        Profile = profile;
        Username = profile.Username;
        Nickname = profile.Nickname;
    }
}
