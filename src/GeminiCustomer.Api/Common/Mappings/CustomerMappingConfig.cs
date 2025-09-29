using GeminiCustomer.Application.Common.Models.Addresses;
using GeminiCustomer.Application.Common.Models.Customers;
using GeminiCustomer.Application.Customers.Commands;
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

        config.NewConfig<CreateCustomerRequest, CreateCustomerCommand>()
            .Map(dest => dest, src => src);

        config.NewConfig<AddressModel, AddressResponse>()
            .Map(dest => dest.CountryCode, src => src.CountryCode.ToString())
            .Map(dest => dest.CountryName, src => src.CountryName)
            .Map(dest => dest, src => src);

        config.NewConfig<CreateAddressRequest, AddAddressCommand>()
            .Map(dest => dest, src => src);
    }
}