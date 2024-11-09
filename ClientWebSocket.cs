using System.Net.WebSockets;
using System.Text;

if (args.Length == 0)
{
    Console.WriteLine("Channel ID not provided.");
    return;
}

string channelId = args[0];
string uriString = $"ws://host.docker.internal:8000/ws/{channelId}";
using var client = new ClientWebSocket();

try
{
    // Connect to the WebSocket server with the provided channel ID
    await client.ConnectAsync(new Uri(uriString), CancellationToken.None);
    Console.WriteLine($"Connected to channel {channelId}");

    // Send an initial message to the server
    var message = "Hello from client!";
    var buffer = Encoding.UTF8.GetBytes(message);
    await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
    Console.WriteLine("Initial message sent");

    // Enter a loop to continuously receive messages
    var receiveBuffer = new byte[1024];
    while (client.State == WebSocketState.Open)
    {
        try
        {
            var receiveResult = await client.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

            // If the WebSocket is closed by the server, exit the loop
            if (receiveResult.MessageType == WebSocketMessageType.Close)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closed connection", CancellationToken.None);
                Console.WriteLine("Server closed the connection");
                break;
            }

            var receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);
            Console.WriteLine($"Received message: {receivedMessage}");
        }
        catch (WebSocketException ex)
        {
            Console.WriteLine($"WebSocket error: {ex.Message}");
            break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Connection error: {ex.Message}");
}
finally
{
    client.Dispose();
    Console.WriteLine("WebSocket closed");
}
