namespace SignalR.SelfHosted.Users.Models;

public class User
{
    public User(int id, string fullName, string photoUrl)
    {
        Id = id;
        SetFullName(fullName);
        PhotoUrl = photoUrl;
    }

    public int Id { get; set; }

    public string FullName { get; private set; }

    public string FullNameNormalized { get; private set; }

    public string PhotoUrl { get; set; }

    public bool OnLine { get; set; }

    public void SetFullName(string fullName)
    {
        FullName = fullName;
        FullNameNormalized = fullName.ToUpper();
    }
}
