using GeminiCustomer.Application.Common.Models.Users;
using GeminiCustomer.Contracts;
using Mapster;

public sealed class UserMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserModel, UserResponse>();

    }
}