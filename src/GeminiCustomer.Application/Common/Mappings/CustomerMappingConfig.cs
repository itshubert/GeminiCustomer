using GeminiCustomer.Application.Common.Models.Customers;
using GeminiCustomer.Domain.Customers;
using Mapster;

namespace GeminiCustomer.Application.Common.Mappings;

public sealed class CustomerMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Customer, CustomerModel>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.Addresses, src => src.Addresses.Adapt<List<Models.Addresses.AddressModel>>())
            .Map(dest => dest, src => src);
    }
}