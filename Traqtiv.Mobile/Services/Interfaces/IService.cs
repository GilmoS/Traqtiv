namespace Traqtiv.Mobile.Services.Interfaces;

// Base Interface for services that require initialization.
public interface IService
{
    Task InitializeAsync();
}