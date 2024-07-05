using Microsoft.Extensions.Configuration;
using System.Net;

IConfiguration appsettings = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var redirectEntries = new List<portredirector.RedirectEntry>();
appsettings.GetSection("redirects").Bind(redirectEntries);

var tasks = new List<Task>();

foreach (var redirect in redirectEntries)
{
    var localIP = IPAddress.Any;
    var localPort = redirect.port;
    var remoteIP = IPAddress.Parse(redirect.destiny.Split(":")[0]);
    var remotePort = int.Parse(redirect.destiny.Split(":")[1]);

    var redirector = new portredirector.TcpPortRedirector(localIP, localPort, remoteIP, remotePort);
    tasks.Add(redirector.StartAsync());

    Console.WriteLine($"{redirect.title} -> TCP redirector started on {localIP}:{localPort}, redirecting to {remoteIP}:{remotePort}");
}

await Task.WhenAll(tasks);