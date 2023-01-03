using assets_manager.Helpers;
using Microsoft.Data.Sqlite;
using System.IO;


namespace assets_manager.Services.Storage;

public class KeyValueStorage : IKeyValueStorage
{
    private SqLiteHelper sqlite_helper;

    public KeyValueStorage() {

        sqlite_helper = new SqLiteHelper();
        InitDatabase();
    }
    public string Get(string key, string defaultValue = "")
    {
        SqliteDataReader result = sqlite_helper.ExecuteQuery($"SELECT value from preferences where key = \"{key}\"");
        while (result.Read()) {
            if (result.GetString(result.GetOrdinal("key")) == key) { 
                return result.GetString(result.GetOrdinal("value"));
            }
        }
            return defaultValue;
    }

    public void Set(string key, string value)
    {
        if (this.Get(key) == "") {
            sqlite_helper.InsertValues("preferences", new string[] { key, value });
        }
        else
        {
            sqlite_helper.UpdateValues("preferences", new string[] { "key" }, new string[] { value }, "key", key, "=");
        }
    }

    private void InitDatabase() {
        string[] pre_field = {"id", "key", "value" };
        string[] pre_type = { "INTEGER PRIMARY KEY AUTO_INCREMENT", "TEXT", "TEXT" };
        if (!File.Exists(Config.AppPathConst.AppDataDirectory + "/assets_config.db")) {
            sqlite_helper.CreateTable("preferences", pre_field, pre_type);
        }

    }

    public void Colse() {

        sqlite_helper.CloseConnection();
    }
}

