namespace kind_of_bread.DTOs;

public class BreadTypeDTO
{
    public int Id { get; set; }
    public required string Type { get; set; }
}

public class BreadItemDTO
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public required string Name { get; set; }
    public required string Grain { get; set; }
    public bool Fermented { get; set; }
    public string? Viscosity { get; set; }
    public string? Texture { get; set; }
    public string? Shape { get; set; }
    public bool? Dressed { get; set; }
}

public class BreadAddonDTO
{
    public int Id { get; set; }
    public int BreadId { get; set; }
    public string? Addon { get; set; }
    public string? Type { get; set; }
}