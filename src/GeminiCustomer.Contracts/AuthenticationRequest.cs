namespace GeminiCustomer.Contracts;

public sealed record AuthenticationRequest(
    string Username,
    string Password);