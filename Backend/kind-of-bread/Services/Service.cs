using kind_of_bread.DTOs;
using kind_of_bread.Models;
using AutoMapper;
using System.Text.RegularExpressions;
using System.Globalization;

namespace kind_of_bread.Services;

public interface IBreadFactory
{
    BreadItemDTO Create(Bread bread);
}

public class LiquidBreadFactory(IMapper mapper) : IBreadFactory
{

    public BreadItemDTO Create(Bread bread)
    {
        if (bread is LiquidBread liquidBread)
        {
            return mapper.Map<BreadItemDTO>(liquidBread);
        }
        throw new ArgumentException("Invalid bread type for LiquidBreadFactory");
    }
}

public class SimpleSandwichFactory(IMapper mapper) : IBreadFactory
{

    public BreadItemDTO Create(Bread bread)
    {
        if (bread is SimpleSandwich simpleSandwich)
        {
            return mapper.Map<BreadItemDTO>(simpleSandwich);
        }
        throw new ArgumentException("Invalid bread type for SimpleSandwichFactory");
    }
}

public class DoubleSandwichFactory(IMapper mapper) : IBreadFactory
{
    public BreadItemDTO Create(Bread bread)
    {
        if (bread is DoubleSandwich doubleSandwich)
        {
            return mapper.Map<BreadItemDTO>(doubleSandwich);
        }
        throw new ArgumentException("Invalid bread type for DoubleSandwichFactory");
    }
}

public class MultiSandwichFactory(IMapper mapper) : IBreadFactory
{
    public BreadItemDTO Create(Bread bread)
    {
        if (bread is MultiSandwich multiSandwich)
        {
            return mapper.Map<BreadItemDTO>(multiSandwich);
        }
        throw new ArgumentException("Invalid bread type for MultiSandwichFactory");
    }
}

public class BoatlikeFactory(IMapper mapper) : IBreadFactory
{
    public BreadItemDTO Create(Bread bread)
    {
        if (bread is Boatlike boatlike)
        {
            return mapper.Map<BreadItemDTO>(boatlike);
        }
        throw new ArgumentException("Invalid bread type for BoatlikeFactory");
    }
}

public class ExtrudedFactory(IMapper mapper) : IBreadFactory
{
    public BreadItemDTO Create(Bread bread)
    {
        if (bread is Extruded extruded)
        {
            return mapper.Map<BreadItemDTO>(extruded);
        }
        throw new ArgumentException("Invalid bread type for ExtrudedFactory");
    }
}

public class BreadFactoryResolver(
    LiquidBreadFactory liquidBreadFactory,
    SimpleSandwichFactory simpleSandwichFactory,
    DoubleSandwichFactory doubleSandwichFactory,
    MultiSandwichFactory multiSandwichFactory,
    BoatlikeFactory boatlikeFactory,
    ExtrudedFactory extrudedFactory)
{
    private readonly LiquidBreadFactory _liquidBreadFactory = liquidBreadFactory;
    private readonly SimpleSandwichFactory _simpleSandwichFactory = simpleSandwichFactory;
    private readonly DoubleSandwichFactory _doubleSandwichFactory = doubleSandwichFactory;
    private readonly MultiSandwichFactory _multiSandwichFactory = multiSandwichFactory;
    private readonly BoatlikeFactory _boatlikeFactory = boatlikeFactory;
    private readonly ExtrudedFactory _extrudedFactory = extrudedFactory;

    public IBreadFactory Resolve(Bread bread) => bread switch
    {
        LiquidBread => _liquidBreadFactory,
        SimpleSandwich => _simpleSandwichFactory,
        DoubleSandwich => _doubleSandwichFactory,
        MultiSandwich => _multiSandwichFactory,
        Boatlike => _boatlikeFactory,
        Extruded => _extrudedFactory,
        _ => throw new ArgumentException("Unsupported bread type")
    };
}

public class BreadProfile : Profile
{
    public BreadProfile()
    {
        CreateMap<LiquidBread, BreadItemDTO>()
            .ForMember(dest => dest.Viscosity, opt => opt.MapFrom(src => src.Viscosity));
        CreateMap<SimpleSandwich, BreadItemDTO>()
            .ForMember(dest => dest.Texture, opt => opt.MapFrom(src => src.Texture))
            .ForMember(dest => dest.Dressed, opt => opt.MapFrom(src => src.Dressed));
        CreateMap<DoubleSandwich, BreadItemDTO>()
            .ForMember(dest => dest.Texture, opt => opt.MapFrom(src => src.Texture))
            .ForMember(dest => dest.Dressed, opt => opt.MapFrom(src => src.Dressed));
        CreateMap<MultiSandwich, BreadItemDTO>()
            .ForMember(dest => dest.Texture, opt => opt.MapFrom(src => src.Texture))
            .ForMember(dest => dest.Dressed, opt => opt.MapFrom(src => src.Dressed));
        CreateMap<Boatlike, BreadItemDTO>()
            .ForMember(dest => dest.Texture, opt => opt.MapFrom(src => src.Texture));
        CreateMap<Extruded, BreadItemDTO>()
            .ForMember(dest => dest.Texture, opt => opt.MapFrom(src => src.Texture))
            .ForMember(dest => dest.Shape, opt => opt.MapFrom(src => src.Shape));
    }
}

public static partial class StringHelper
{
    [GeneratedRegex(@"([a-z])([A-Z])|([A-Z])([A-Z][a-z])")]
    private static partial Regex StringSplitter();

    public static string FormatWords(string input)
    {
        string splitWords = StringSplitter().Replace(input, "$1$3 $2$4");

        TextInfo info = CultureInfo.CurrentCulture.TextInfo;
        splitWords = info.ToTitleCase(splitWords.ToLower());

        return splitWords;
    }
}