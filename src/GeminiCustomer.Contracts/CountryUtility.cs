namespace GeminiCustomer.Contracts;

/// <summary>
/// Utility class for working with countries in the API.
/// Contains the same country codes and names as defined in the domain layer.
/// </summary>
public static class CountryUtility
{
    private static readonly Dictionary<string, string> Countries = new()
    {
        { "US", "United States" },
        { "CA", "Canada" },
        { "GB", "United Kingdom" },
        { "AU", "Australia" },
        { "DE", "Germany" },
        { "FR", "France" },
        { "IT", "Italy" },
        { "ES", "Spain" },
        { "NL", "Netherlands" },
        { "BE", "Belgium" },
        { "CH", "Switzerland" },
        { "AT", "Austria" },
        { "SE", "Sweden" },
        { "NO", "Norway" },
        { "DK", "Denmark" },
        { "FI", "Finland" },
        { "JP", "Japan" },
        { "KR", "South Korea" },
        { "CN", "China" },
        { "IN", "India" },
        { "BR", "Brazil" },
        { "MX", "Mexico" },
        { "AR", "Argentina" },
        { "ZA", "South Africa" },
        { "NZ", "New Zealand" },
        { "SG", "Singapore" },
        { "IE", "Ireland" },
        { "PT", "Portugal" },
        { "GR", "Greece" },
        { "PL", "Poland" },
        { "CZ", "Czech Republic" },
        { "HU", "Hungary" },
        { "TR", "Turkey" },
        { "IL", "Israel" },
        { "AE", "United Arab Emirates" },
        { "SA", "Saudi Arabia" },
        { "EG", "Egypt" },
        { "TH", "Thailand" },
        { "MY", "Malaysia" },
        { "PH", "Philippines" },
        { "ID", "Indonesia" },
        { "VN", "Vietnam" },
        { "CL", "Chile" },
        { "CO", "Colombia" },
        { "PE", "Peru" },
        { "UA", "Ukraine" },
        { "RU", "Russia" }
    };

    /// <summary>
    /// Gets all available countries with their codes and display names.
    /// </summary>
    /// <returns>A dictionary mapping country codes to display names.</returns>
    public static Dictionary<string, string> GetAllCountries() => new(Countries);

    /// <summary>
    /// Gets all available country codes.
    /// </summary>
    /// <returns>An array of country codes.</returns>
    public static string[] GetAllCountryCodes() => Countries.Keys.ToArray();

    /// <summary>
    /// Validates if a country code or name is valid.
    /// </summary>
    /// <param name="country">The country code or name to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool IsValidCountry(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
            return false;

        return Countries.ContainsKey(country.ToUpper()) ||
               Countries.Values.Contains(country, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the country name for a given country code.
    /// </summary>
    /// <param name="countryCode">The country code.</param>
    /// <returns>The country name if found, otherwise null.</returns>
    public static string? GetCountryName(string countryCode)
    {
        return Countries.TryGetValue(countryCode?.ToUpper() ?? "", out var name) ? name : null;
    }
}