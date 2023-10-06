
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace box_office.DataBase.Models;

public class Ticket : IBaseEntity
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int PlaceId { get; set; }
    public int? UserId { get; set; }
    public virtual Place Place { get; set; }
    public virtual Session Session { get; set; }
    public virtual User User { get; set; }
}

public class TicketSourceConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("ticket");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id");

        builder.Property(x => x.SessionId)
            .HasColumnName("session_id");

        builder.Property(x => x.PlaceId)
            .HasColumnName("place_id");

        builder.Property(x => x.UserId)
            .HasColumnName("user_id");

        builder.HasOne(ticket => ticket.Place)
            .WithMany(place => place.Tickets)
            .HasForeignKey(ticket => ticket.PlaceId);

        builder.HasOne(ticket => ticket.Session)
            .WithMany(session => session.Tickets)
            .HasForeignKey(ticket => ticket.SessionId);

        builder.HasOne(ticket => ticket.User)
            .WithMany(user => user.Tickets)
            .HasForeignKey(ticket => ticket.UserId);
    }
}
