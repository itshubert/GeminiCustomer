using System.ComponentModel.DataAnnotations;
using System.Reflection;
using GeminiCustomer.Domain.Common.Enums;

namespace GeminiCustomer.Domain.Common.Extensions;

public static class CountryCodeExtensions
{
    /// <summary>
    /// Gets the display name of a CountryCode enum value using the Display attribute.
    /// </summary>
    /// <param name="countryCode">The country code enum value.</param>
    /// <returns>The display name if available, otherwise the enum name.</returns>
    public static string GetDisplayName(this CountryCode countryCode)
    {
        var displayAttribute = countryCode.GetType()
            .GetMember(countryCode.ToString())
            .FirstOrDefault()?
            .GetCustomAttribute<DisplayAttribute>();

        return displayAttribute?.Name ?? countryCode.ToString();
    }

    /// <summary>
    /// Gets all available country codes with their display names.
    /// </summary>
    /// <returns>A dictionary mapping country codes to their display names.</returns>
    public static Dictionary<CountryCode, string> GetAllCountriesWithDisplayNames()
    {
        return Enum.GetValues<CountryCode>()
            .ToDictionary(code => code, code => code.GetDisplayName());
    }

    /// <summary>
    /// Tries to parse a country name or code to a CountryCode enum.
    /// </summary>
    /// <param name="value">The country name or code to parse.</param>
    /// <param name="countryCode">The parsed country code if successful.</param>
    /// <returns>True if parsing was successful, false otherwise.</returns>
    public static bool TryParseCountry(string value, out CountryCode countryCode)
    {
        countryCode = default;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        // Try to parse as enum value first (for codes like "US", "CA")
        if (Enum.TryParse<CountryCode>(value, true, out countryCode))
            return true;

        // Try to match by display name (for full names like "United States")
        var allCountries = GetAllCountriesWithDisplayNames();
        var matchingCountry = allCountries.FirstOrDefault(kvp =>
            string.Equals(kvp.Value, value, StringComparison.OrdinalIgnoreCase));

        if (!matchingCountry.Equals(default(KeyValuePair<CountryCode, string>)))
        {
            countryCode = matchingCountry.Key;
            return true;
        }

        return false;
    }
}