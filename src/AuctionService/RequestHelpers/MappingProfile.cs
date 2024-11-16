using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();

        CreateMap<CreateAuctionDto, Auction>()
            .ForMember(destination => destination.Item, o => o.MapFrom(source => source));
        CreateMap<CreateAuctionDto, Item>();   /* CreateMap<CreateAuctionDto, Auction>():
        This tells AutoMapper how to map from CreateAuctionDto to Auction. 
        If you have a property like Item inside Auction, AutoMapper will need to know how to populate that property. 
        If Item is of type CreateAuctionDto, AutoMapper can simply assign it without needing further instructions.

        CreateMap<CreateAuctionDto, Item>():
        If Item is a different class (not CreateAuctionDto),
        then you need to tell AutoMapper explicitly how to map from CreateAuctionDto to Item. 
        This is because Item has its own structure that may or may not be identical to CreateAuctionDto, 
        and AutoMapper needs the mapping configuration to know how to convert the properties. */
    }
}
