
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace box_office.DataBase.Models;

public class Hall : IBaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
    public virtual ICollection<Session> Sessions { get; set; }
    public virtual ICollection<Place> Places { get; set; }
}

public class HallSourceConfiguration : IEntityTypeConfiguration<Hall>
{
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.ToTable("hall");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name");

        builder.Property(x => x.Size)
            .HasColumnName("size");
    }
}
