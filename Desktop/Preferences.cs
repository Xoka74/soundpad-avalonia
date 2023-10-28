using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Desktop;

public class Preferences
{
    private IConfigurationRoot root;

    public Preferences()
    {
        if (!File.Exists("appsettings.json"))
            File.Create("appsettings.json");
        
        root = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
    }

    public string? this[string key]
    {
        get { return root[key]; }
        set
        {
            root[key] = value;
        }
    }
}