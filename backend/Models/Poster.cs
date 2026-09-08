namespace backend.Models;

public class Poster
{
    public int Id{get; set;}

    public string FileName{get; set;} = "";

    public string ContentType{get; set;} = "";

    public byte[] Data{get; set;} = Array.Empty<byte>();
}

