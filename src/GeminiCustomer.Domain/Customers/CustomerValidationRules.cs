namespace GeminiCustomer.Domain.Customers;

public static class CustomerValidationRules
{
    public static class Names
    {
        public const int MaxLength = 50;
        public const int MinLength = 1;
    }

    public static class Email
    {
        public const int MaxLength = 100;
    }

    public static class Address
    {
        public const int AddressLineMaxLength = 100;
        public const int CityMaxLength = 50;
        public const int StateMaxLength = 50;
        public const int PostCodeMaxLength = 20;
    }
}