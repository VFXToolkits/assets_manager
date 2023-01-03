
namespace assets_manager.Services.Storage;

public interface IKeyValueStorage
{
    string Get(string key, string defaultValue);
    void Set(string key, string value);
}
