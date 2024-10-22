using AutoMapper;
using Microsoft.EntityFrameworkCore;
using kind_of_bread.Context;
using kind_of_bread.DTOs;
using kind_of_bread.Models;
using kind_of_bread.Services;

namespace kind_of_bread.Repositories;

public class BreadRepository(BreadContext context, BreadFactoryResolver breadFactoryResolver, IMapper mapper)
{
    public async Task<List<Bread>> GetBreadById(int id)
    {
        BreadItemDTO breadItemDTO = await context.BreadItems.FirstAsync(b => b.Id == id);
        BreadTypeDTO breadTypeDTO = await context.BreadTypes.FirstAsync(bt => bt.Id == breadItemDTO.TypeId);

        return [await GetBread(breadTypeDTO.Type, breadItemDTO)];
    }

    public async Task<List<Bread>> GetBreadByName(string name)
    {
        BreadItemDTO breadItemDTO = await context.BreadItems.FirstAsync(b => b.Name == name);
        BreadTypeDTO breadTypeDTO = await context.BreadTypes.FirstAsync(bt => bt.Id == breadItemDTO.TypeId);

        return [await GetBread(breadTypeDTO.Type, breadItemDTO)];
    }

    public async Task<List<Bread>> GetAllOfType(string type)
    {
        var breadType = await context.BreadTypes.FirstAsync(bt => bt.Type == type);
        var breadItems = await context.BreadItems.Where(b => b.TypeId == breadType.Id).ToListAsync();
        List<Bread> breads = [];

        foreach (var bread in breadItems)
        {
            breads.Add(await GetBread(type, bread));
        }

        return breads;
    }

    public async Task<List<Bread>> GetAllBread()
    {
        List<BreadItemDTO> breadItems = await context.BreadItems.ToListAsync();
        List<BreadTypeDTO> breadTypes = await context.BreadTypes.ToListAsync();
        List<Bread> breads = [];

        foreach (var breadItem in breadItems)
        {
            string type = breadTypes.First(t => t.Id == breadItem.TypeId).Type;
            breads.Add(await GetBread(type, breadItem));
        }

        return mapper.Map<List<Bread>>(breads);
    }

    public async Task CreateBread(Bread bread)
    {
        BreadTypeDTO? breadType = await context.BreadTypes.FirstOrDefaultAsync(t => t.Type == bread.GetType().Name);

        if (breadType == null) { return; }

        var factory = breadFactoryResolver.Resolve(bread);

        BreadItemDTO breadDTO = factory.Create(bread);

        breadDTO.TypeId = breadType.Id;

        context.BreadItems.Add(breadDTO);
        await context.SaveChangesAsync();

        if (bread is SimpleSandwich simpleSandwich)
        {
            foreach (var topping in simpleSandwich.Toppings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = breadDTO.Id, Addon = topping, Type = "Topping" });
            }
        }
        else if (bread is DoubleSandwich doubleSandwich)
        {
            foreach (var filling in doubleSandwich.Fillings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = breadDTO.Id, Addon = filling, Type = "Filling" });
            }
        }
        else if (bread is MultiSandwich multiSandwich)
        {
            foreach (var topping in multiSandwich.Toppings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = breadDTO.Id, Addon = topping, Type = "Topping" });
            }
            foreach (var filling in multiSandwich.Fillings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = breadDTO.Id, Addon = filling, Type = "Filling" });
            }
        }
        else if (bread is Boatlike boatlike)
        {
            foreach (var filling in boatlike.Fillings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = breadDTO.Id, Addon = filling, Type = "Filling" });
            }
        }

        await context.SaveChangesAsync();
    }

    public async Task DeleteBread(int id)
    {
        var breadItem = await context.BreadItems.FindAsync(id) ?? throw new ArgumentException("Bread item with the provided ID not found.");
        var breadAddons = await context.BreadAddons.Where(a => a.BreadId == id).ToListAsync();

        context.BreadAddons.RemoveRange(breadAddons);
        context.BreadItems.Remove(breadItem);

        await context.SaveChangesAsync();
    }

    public async Task UpdateBread(int id, Bread newBread)
    {
        BreadItemDTO oldBread = await context.BreadItems.FindAsync(id) ?? throw new ArgumentException($"No bread with id: {id} found.");
        BreadTypeDTO breadTypeDTO = await context.BreadTypes.FindAsync(oldBread.TypeId) ?? throw new ArgumentException("Type not found.");

        oldBread.Name = newBread.Name;
        oldBread.Grain = newBread.Grain;
        oldBread.Fermented = newBread.Fermented;

        switch (breadTypeDTO.Type)
        {
            case "LiquidBread":
                if (newBread is LiquidBread liquidBread)
                {
                    oldBread.Viscosity = liquidBread.Viscosity;
                }
                break;
            case "SimpleSandwich":
                if (newBread is SimpleSandwich simpleSandwich)
                {
                    oldBread.Texture = simpleSandwich.Texture;
                    oldBread.Dressed = simpleSandwich.Dressed;
                    await UpdateBreadAddons(id, simpleSandwich);
                }
                break;
            case "DoubleSandwich":
                if (newBread is DoubleSandwich doubleSandwich)
                {
                    oldBread.Texture = doubleSandwich.Texture;
                    oldBread.Dressed = doubleSandwich.Dressed;
                    await UpdateBreadAddons(id, doubleSandwich);
                }
                break;
            case "MultiSandwich":
                if (newBread is MultiSandwich multiSandwich)
                {
                    oldBread.Texture = multiSandwich.Texture;
                    oldBread.Dressed = multiSandwich.Dressed;
                    await UpdateBreadAddons(id, multiSandwich);
                }
                break;
            case "Boatlike":
                if (newBread is Boatlike boatlike)
                {
                    await UpdateBreadAddons(id, boatlike);
                }
                break;
            case "Extruded":
                if (newBread is Extruded extruded)
                {
                    oldBread.Texture = extruded.Texture;
                    oldBread.Shape = extruded.Shape;
                }
                break;

            default:
                throw new ArgumentException("Unsupported type of bread.");
        }

        context.BreadItems.Update(oldBread);
        await context.SaveChangesAsync();
    }

    private async Task UpdateBreadAddons(int id, SolidBread bread)
    {
        List<BreadAddonDTO> existingAddons = await context.BreadAddons.Where(a => a.BreadId == id).ToListAsync();
        context.BreadAddons.RemoveRange(existingAddons);

        if (bread is SimpleSandwich simpleSandwich)
        {
            foreach (var topping in simpleSandwich.Toppings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = id, Addon = topping, Type = "Topping" });
            }
        }
        else if (bread is DoubleSandwich doubleSandwich)
        {
            foreach (var filling in doubleSandwich.Fillings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = id, Addon = filling, Type = "Filling" });
            }
        }
        else if (bread is MultiSandwich multiSandwich)
        {
            foreach (var topping in multiSandwich.Toppings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = id, Addon = topping, Type = "Topping" });
            }
            foreach (var filling in multiSandwich.Fillings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = id, Addon = filling, Type = "Filling" });
            }
        }
        else if (bread is Boatlike boatlike)
        {
            foreach (var filling in boatlike.Fillings)
            {
                context.BreadAddons.Add(new BreadAddonDTO { BreadId = id, Addon = filling, Type = "Filling" });
            }
        }

        await context.SaveChangesAsync();
    }

    private async Task<string?[]> GetAddons(int breadId, string type)
    {
        return await context.BreadAddons.Where(a => a.BreadId == breadId && a.Type == type).Select(a => a.Addon).ToArrayAsync();
    }

    public async Task<Bread> GetBread(string type, BreadItemDTO breadItemDTO)
    {
        Bread? fetchedBread = null;

        string?[] toppings;
        string?[] fillings;

        switch (type)
        {
            case "LiquidBread":
                if (breadItemDTO.Viscosity == null)
                {
                    throw new ArgumentException("Error retrieving viscosity.");
                }
                else
                {
                    fetchedBread = new LiquidBread(breadItemDTO.Name, breadItemDTO.Grain, breadItemDTO.Fermented, breadItemDTO.Viscosity);
                }
                break;
            case "SimpleSandwich":
                if (breadItemDTO.Texture == null)
                {
                    throw new ArgumentException("Error retrieving texture.");
                }
                else
                {
                    toppings = await GetAddons(breadItemDTO.Id, "Topping");
                    fetchedBread = new SimpleSandwich(breadItemDTO.Name, breadItemDTO.Grain, breadItemDTO.Fermented, breadItemDTO.Texture, breadItemDTO.Dressed, toppings);
                }
                break;
            case "DoubleSandwich":
                if (breadItemDTO.Texture == null)
                {
                    throw new ArgumentException("Error retrieving texture.");
                }
                else
                {
                    fillings = await GetAddons(breadItemDTO.Id, "Filling");
                    fetchedBread = new DoubleSandwich(breadItemDTO.Name, breadItemDTO.Grain, breadItemDTO.Fermented, breadItemDTO.Texture, breadItemDTO.Dressed, fillings);
                }
                break;
            case "MultiSandwich":
                if (breadItemDTO.Texture == null)
                {
                    throw new ArgumentException("Error retrieving texture.");
                }
                else
                {
                    toppings = await GetAddons(breadItemDTO.Id, "Topping");
                    fillings = await GetAddons(breadItemDTO.Id, "Filling");
                    fetchedBread = new MultiSandwich(breadItemDTO.Name, breadItemDTO.Grain, breadItemDTO.Fermented, breadItemDTO.Texture, breadItemDTO.Dressed, toppings, fillings);
                }
                break;
            case "Boatlike":
                if (breadItemDTO.Texture == null)
                {
                    throw new ArgumentException("Error retrieving texture.");
                }
                else
                {
                    fillings = await GetAddons(breadItemDTO.Id, "Filling");
                    fetchedBread = new Boatlike(breadItemDTO.Name, breadItemDTO.Grain, breadItemDTO.Fermented, breadItemDTO.Texture, fillings);
                }
                break;
            case "Extruded":
                if (breadItemDTO.Texture == null || breadItemDTO.Shape == null)
                {
                    throw new ArgumentException("Error retrieving texture and/or shape.");
                }
                else
                {
                    fetchedBread = new Extruded(breadItemDTO.Name, breadItemDTO.Grain, breadItemDTO.Fermented, breadItemDTO.Texture, breadItemDTO.Shape);
                }
                break;
        }

        if (fetchedBread == null)
        {
            throw new ArgumentException("Error retrieving bread.");
        }
        else
        {
            fetchedBread.Id = breadItemDTO.Id;
            return fetchedBread;
        }
    }
}