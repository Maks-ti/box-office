

namespace box_office.Models;

public class Hall
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
}

public class HallCreateModel
{
    public string Name { get; set; }
    public int Size { get; set; }
}
