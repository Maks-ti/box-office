

using box_office.DataBase.Models;

namespace box_office.Models;

public class Ticket
{
    public int Id { get; set; }
    public int SessionId { get; set; }
    public int PlaceId { get; set; }
    public bool IsSold { get; set; }
    public virtual Place Place { get; set; }
}

public class TicketIsSoldUpdateModel
{
    public int TicketId { get; set; }
    public bool IsSold { get; set; }
}