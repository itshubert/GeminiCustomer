using GeminiCustomer.Application.Common.Models.Customers;
using GeminiCustomer.Contracts;
using Mapster;

namespace GeminiCustomer.Api.Common.Mappings;

public sealed class CustomerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CustomerModel, CustomerResponse>()
            .Map(dest => dest.Addresses, src => src.Addresses.Adapt<IEnumerable<AddressResponse>>())
            .Map(dest => dest, src => src);
    }
}