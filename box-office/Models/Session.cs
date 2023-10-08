

namespace box_office.Models;

public class Session
{
    public int Id { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int PlayId { get; set; }
    public int HallId { get; set; }
    public Play Play { get; set; }
    public Hall Hall { get; set; }
}

public class SessionCreateModel
{
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
    public int PlayId { get; set; }
    public int HallId { get; set; }
}

public class SessionUpdateModel
{
    public int Id { get; set; }
    public DateTime DateFrom { get; set; }
    public DateTime DateTo { get; set; }
}
