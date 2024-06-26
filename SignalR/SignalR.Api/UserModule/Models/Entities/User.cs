using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace SignalR.Api.UserModule.Models.Entities;

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

    public void SetFullName(string fullName)
    {
        FullName = fullName;
        FullNameNormalized = fullName.ToUpper();
    }
}

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

    }
}