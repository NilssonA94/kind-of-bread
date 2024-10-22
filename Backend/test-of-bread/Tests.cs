using AutoMapper;
using Microsoft.EntityFrameworkCore;
using kind_of_bread.Context;
using kind_of_bread.DTOs;
using kind_of_bread.Models;
using kind_of_bread.Repositories;
using kind_of_bread.Services;
using Xunit;

namespace test_of_bread;

public class Tests(BreadTestFixture fixture) : IClassFixture<BreadTestFixture>
{
    [Fact]
    public async Task CreateAndGetSimpleSandwichTest()
    {
        SimpleSandwich simpleSandwich = new("Test Sandwich", "Wheat", true, "Soft", false, ["Cheese", "Tomato"])
        {
            Id = 1
        };

        await fixture.context.BreadTypes.AddAsync(new BreadTypeDTO { Id = 1, Type = "SimpleSandwich" });
        await fixture.context.SaveChangesAsync();
        await fixture.repository.CreateBread(simpleSandwich);
        List<Bread> createdSandwich = await fixture.repository.GetBreadById(simpleSandwich.Id);
        SimpleSandwich? createdSimpleSandwich = createdSandwich[0] as SimpleSandwich;

        Assert.NotNull(createdSimpleSandwich);
        Assert.Equal(simpleSandwich.Name, createdSimpleSandwich.Name);
        Assert.Equal(simpleSandwich.Grain, createdSimpleSandwich.Grain);
        Assert.Equal(simpleSandwich.Fermented, createdSimpleSandwich.Fermented);
        Assert.Equal(simpleSandwich.Texture, createdSimpleSandwich.Texture);
        Assert.Equal(simpleSandwich.Dressed, createdSimpleSandwich.Dressed);
        Assert.Equal(simpleSandwich.Toppings, createdSimpleSandwich.Toppings);
    }
}

public class BreadTestFixture : IDisposable
{
    public readonly BreadContext context;
    public readonly BreadRepository repository;

    public BreadTestFixture()
    {
        context = new BreadContext(new DbContextOptionsBuilder<BreadContext>().UseInMemoryDatabase(databaseName: "TestDb").Options);

        var mapper = new MapperConfiguration(cfg => { cfg.AddProfile<BreadProfile>(); }).CreateMapper();

        repository = new BreadRepository(context, new BreadFactoryResolver(new LiquidBreadFactory(mapper), new SimpleSandwichFactory(mapper), new DoubleSandwichFactory(mapper), new MultiSandwichFactory(mapper), new BoatlikeFactory(mapper), new ExtrudedFactory(mapper)), mapper);

        context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}