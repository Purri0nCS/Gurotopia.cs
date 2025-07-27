/*
    @copyright gurotopia (c) 2025-07-27
*/

ENet.Library.Initialize();

using ENet.Host server = new();
ENet.Address address = new() { Port = 17091 };

server.Create(address, 50);

Console.WriteLine("Server started on port " + address.Port);


while (!Console.KeyAvailable)
{
    bool polled = false;
    while (!polled)
    {
        if (server.CheckEvents(out ENet.Event netEvent) <= 0)
        {
            if (server.Service(15, out netEvent) <= 0)
                break;

            polled = true;
        }

        switch (netEvent.Type)
        {
            case ENet.EventType.Connect:
                Console.WriteLine($"Client connected - ID: {netEvent.Peer.ID}");
                break;

            case ENet.EventType.Disconnect:
                Console.WriteLine($"Client disconnected - ID: {netEvent.Peer.ID}");
                break;

            case ENet.EventType.Receive:
                Console.WriteLine($"Packet received from - ID: {netEvent.Peer.ID}, Data length: " + netEvent.Packet.Length);
                netEvent.Packet.Dispose();
                break;
        }
    }
}

ENet.Library.Deinitialize();