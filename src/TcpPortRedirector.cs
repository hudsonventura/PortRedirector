using System.Net;
using System.Net.Sockets;

namespace portredirector;

public class TcpPortRedirector
{
    private readonly IPAddress _localIPAddress;
    private readonly int _localPort;
    private readonly IPAddress _remoteIPAddress;
    private readonly int _remotePort;

    public TcpPortRedirector(IPAddress localIPAddress, int localPort, IPAddress remoteIPAddress, int remotePort)
    {
        _localIPAddress = localIPAddress;
        _localPort = localPort;
        _remoteIPAddress = remoteIPAddress;
        _remotePort = remotePort;
    }

    public async Task StartAsync()
    {
        var listener = new TcpListener(_localIPAddress, _localPort);
        listener.Start();
        //Console.WriteLine($"TCP redirector started on {_localIPAddress}:{_localPort}, redirecting to {_remoteIPAddress}:{_remotePort}");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private async Task HandleClientAsync(TcpClient client)
    {
        using (client)
        using (var remoteClient = new TcpClient())
        {
            await remoteClient.ConnectAsync(_remoteIPAddress, _remotePort);

            var clientStream = client.GetStream();
            var remoteStream = remoteClient.GetStream();

            var clientToRemoteTask = clientStream.CopyToAsync(remoteStream);
            var remoteToClientTask = remoteStream.CopyToAsync(clientStream);

            await Task.WhenAny(clientToRemoteTask, remoteToClientTask);
        }
    }
}
