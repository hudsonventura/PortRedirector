using System.Net;
using System.Net.Sockets;

namespace portredirector;

public class UdpPortRedirector
{
    private readonly IPAddress _localIPAddress;
    private readonly int _localPort;
    private readonly IPAddress _remoteIPAddress;
    private readonly int _remotePort;

    public UdpPortRedirector(IPAddress localIPAddress, int localPort, IPAddress remoteIPAddress, int remotePort)
    {
        _localIPAddress = localIPAddress;
        _localPort = localPort;
        _remoteIPAddress = remoteIPAddress;
        _remotePort = remotePort;
    }

    public async Task StartAsync()
    {
        using (var udpClient = new UdpClient(new IPEndPoint(_localIPAddress, _localPort)))
        {
            Console.WriteLine($"UDP redirector started on {_localIPAddress}:{_localPort}, redirecting to {_remoteIPAddress}:{_remotePort}");

            while (true)
            {
                var result = await udpClient.ReceiveAsync();
                await udpClient.SendAsync(result.Buffer, result.Buffer.Length, new IPEndPoint(_remoteIPAddress, _remotePort));
            }
        }
    }

}
