using System;

public class Metadata
{
    public string FileName { get; set; }
    public int Version { get; set; }
    public DateTime LastSaved { get; set; }
    public DateTime Creation { get; set; }

    public Metadata() { }

    public Metadata CreateNewStaticFile(string fileName, string extension)
    {
        FileName = $"{fileName}{extension}";
        Version = 1;
        LastSaved = DateTime.Now;
        Creation = DateTime.Now;

        return this;
    }
    
    public Metadata CreateNewDynamicFile(string extension)
    {
        FileName = $"{Guid.NewGuid()}{extension}";
        Version = 1;
        LastSaved = DateTime.Now;
        Creation = DateTime.Now;

        return this;
    }
}