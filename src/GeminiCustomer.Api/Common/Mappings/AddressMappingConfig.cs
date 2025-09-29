using GeminiCustomer.Application.Common.Models.Addresses;
using GeminiCustomer.Contracts;
using Mapster;

namespace GeminiCustomer.Api.Common.Mappings;

public class AddressMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddressModel, AddressResponse>()
            .Map(dest => dest, src => src);
    }
}