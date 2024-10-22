using System.Text.Json.Serialization;

namespace kind_of_bread.Models;

[JsonDerivedType(typeof(LiquidBread), "LiquidBread")]
[JsonDerivedType(typeof(SimpleSandwich), "SimpleSandwich")]
[JsonDerivedType(typeof(DoubleSandwich), "DoubleSandwich")]
[JsonDerivedType(typeof(MultiSandwich), "MultiSandwich")]
[JsonDerivedType(typeof(Boatlike), "Boatlike")]
[JsonDerivedType(typeof(Extruded), "Extruded")]
public abstract class Bread(string name, string grain, bool fermented)
{
    public int Id { get; set; }
    public string Name { get; set; } = name;
    public string Grain { get; set; } = grain;
    public bool Fermented { get; set; } = fermented;
}

public class LiquidBread(string name, string grain, bool fermented, string viscosity) : Bread(name, grain, fermented)
{
    public string Viscosity { get; set; } = viscosity;
}

public abstract class SolidBread(string name, string grain, bool fermented, string texture) : Bread(name, grain, fermented)
{
    public string Texture { get; set; } = texture;
}

public abstract class Sandwich(string name, string grain, bool fermented, string texture, bool? dressed) : SolidBread(name, grain, fermented, texture)
{
    public bool? Dressed { get; set; } = dressed;
}

public class SimpleSandwich(string name, string grain, bool fermented, string texture, bool? dressed, string?[] toppings) : Sandwich(name, grain, fermented, texture, dressed)
{
    public string?[] Toppings { get; set; } = toppings;
}

public class DoubleSandwich(string name, string grain, bool fermented, string texture, bool? dressed, string?[] fillings) : Sandwich(name, grain, fermented, texture, dressed)
{
    public string?[] Fillings { get; set; } = fillings;
}

public class MultiSandwich(string name, string grain, bool fermented, string texture, bool? dressed, string?[] toppings, string?[] fillings) : Sandwich(name, grain, fermented, texture, dressed)
{
    public string?[] Toppings { get; set; } = toppings;
    public string?[] Fillings { get; set; } = fillings;
}

public class Boatlike(string name, string grain, bool fermented, string texture, string?[] fillings) : SolidBread(name, grain, fermented, texture)
{
    public string?[] Fillings { get; set; } = fillings;
}

public class Extruded(string name, string grain, bool fermented, string texture, string shape) : SolidBread(name, grain, fermented, texture)
{
    public string Shape { get; set; } = shape;
}