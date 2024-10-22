using Microsoft.AspNetCore.Mvc;
using kind_of_bread.Models;
using kind_of_bread.Repositories;
using kind_of_bread.Services;

namespace kind_of_bread.Controllers;

public partial class BreadController(BreadRepository repository) : ControllerBase
{

    [HttpGet("Bread/Get/ById/{id}")]
    public async Task<IActionResult> GetBreadById(int id)
    {
        List<Bread> bread = await repository.GetBreadById(id);
        return Ok(bread);
    }

    [HttpGet("Bread/Get/ByName/{name}")]
    public async Task<IActionResult> GetBreadByName(string name)
    {
        List<Bread> bread = await repository.GetBreadByName(StringHelper.FormatWords(name));
        return Ok(bread);
    }

    [HttpGet("Bread/Get/All/{type}")]
    public async Task<IActionResult> GetBreadByType(string type)
    {
        List<Bread> bread = await repository.GetAllOfType(type);
        return Ok(bread);
    }

    [HttpGet("Bread/Get/All")]
    public async Task<IActionResult> GetAllBread()
    {
        List<Bread> bread = await repository.GetAllBread();
        return Ok(bread);
    }

    [HttpPost("Bread/Create")]
    public async Task<IActionResult> CreateBread([FromBody] Bread bread)
    {
        await repository.CreateBread(bread);
        return CreatedAtAction(nameof(GetBreadById), new { id = bread.Id }, bread);
    }

    [HttpDelete("Bread/Delete/{id}")]
    public async Task<IActionResult> DeleteBread(int id)
    {
        try
        {
            await repository.DeleteBread(id);
            return Ok($"Bread with id: {id} deleted successfully!");
        }
        catch (Exception)
        {
            return StatusCode(500, $"An error occurred while deleting the bread with id: {id}.");
        }
    }

    [HttpPut("Bread/Update/{id}")]
    public async Task<IActionResult> UpdateBread(int id, [FromBody] Bread bread)
    {
        try
        {
            await repository.UpdateBread(id, bread);
            return Ok($"Bread with id: {id} updated successfully!");
        }
        catch (Exception)
        {
            return StatusCode(500, $"An error occurred while updating the bread with id: {id}.");
        }
    }
}