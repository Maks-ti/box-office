
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace box_office.DataBase.Models;

public class Play : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[] PictureData { get; set; }
    public string PictureExtension { get; set; }
    public virtual ICollection<Session> Sessions { get; set; }
}

public class PlaySourceConfiguration : IEntityTypeConfiguration<Play>
{
    public void Configure(EntityTypeBuilder<Play> builder)
    {
        builder.ToTable("play");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name");

        builder.Property(x => x.Description)
            .HasColumnName("description");

        builder.Property(x => x.PictureData)
            .HasColumnName("picture_data");

        builder.Property(x => x.PictureExtension)
            .HasColumnName("picture_extension");
    }
}
