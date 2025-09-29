using GeminiCustomer.Application.Common.Models.Addresses;
using GeminiCustomer.Domain.Common.Extensions;
using GeminiCustomer.Domain.Customers.Entities;
using Mapster;

namespace GeminiCustomer.Application.Common.Mappings;

public sealed class AddressMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressModel>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.CustomerId, src => src.CustomerId.Value)
            .Map(dest => dest.CountryCode, src => src.Country)
            .Map(dest => dest.CountryName, src => src.Country.GetDisplayName())
            .Map(dest => dest, src => src);

    }
}