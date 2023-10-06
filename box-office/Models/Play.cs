


namespace box_office.Models;

public class Play
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte[]? PictureData { get; set; }
    public string? PictureExtension { get; set; }
}
