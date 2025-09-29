using GeminiCustomer.Application.Common.Models.Addresses;
using GeminiCustomer.Domain.Addresses;
using Mapster;

namespace GeminiCustomer.Application.Common.Mappings;

public sealed class AddressMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Address, AddressModel>()
            .Map(dest => dest.CustomerId, src => src.CustomerId.Value)
            .Map(dest => dest, src => src);

    }
}