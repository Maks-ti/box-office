
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace box_office.DataBase.Models;

public class Session
{
    public int Id { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int PlayId { get; set; }
    public int HallId { get; set; }
    public virtual Play Play { get; set; }
    public virtual Hall Hall { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; }
}

public class SessionSourceConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("session");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.DateFrom)
            .HasColumnName("date_from");

        builder.Property(x => x.DateTo)
            .HasColumnName("date_to");

        builder.Property(x => x.PlayId)
            .HasColumnName("play_id");

        builder.Property(x => x.HallId)
            .HasColumnName("hall_id");

        builder.HasOne(session => session.Play)
            .WithMany(play => play.Sessions)
            .HasForeignKey(session => session.PlayId);

        builder.HasOne(session => session.Hall)
            .WithMany(hall => hall.Sessions)
            .HasForeignKey(session => session.HallId);
    }
}
