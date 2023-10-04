
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace box_office.DataBase.Models;

public class Place : IBaseEntity
{
    public int Id { get; set; }
    public int HallId { get; set; }
    public string Name { get; set; }
    public virtual Hall Hall { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
}

public class PlaceSourceConfiguration : IEntityTypeConfiguration<Place>
{
    public void Configure(EntityTypeBuilder<Place> builder)
    {
        builder.ToTable("place");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name");

        builder.Property(x => x.HallId)
            .HasColumnName("hall_id");

        builder.HasOne(place => place.Hall)
            .WithMany(hall => hall.Places)
            .HasForeignKey(place => place.HallId);
    }
}
