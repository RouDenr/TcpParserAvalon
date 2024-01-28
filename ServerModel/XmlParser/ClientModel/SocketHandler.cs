using System.Net.Sockets;
using NLog;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public class SocketHandler : IConnectHandle
{
	public bool IsConnected => Socket.Connected;
	
	public event EventHandler<IData>? DataReceivedEvent;
	public event EventHandler<SocketHandler>? DisconnectedEvent;
	
	protected TcpClient Socket { get; set; }
	
	private static readonly Logger Log = LogManager.GetCurrentClassLogger();
	
	public SocketHandler(TcpClient socket)
	{
		Socket = socket;
	}
	
	public async Task ReadHandle()
	{
		await Task.Yield();
		
		while (true)
		{
			try
			{
				while (true)
				{
					IData? dataReceived = await ReadDataAsync();
					if (dataReceived == null)
						throw new Exception("Failed to read");
					OnClientSentDataInvoke(dataReceived);
				}
			}
			catch (IOException)
			{
				// Client disconnected
				Log.Info($"{this} disconnected");
				Dispose();
				OnClientDisconnectedInvoke();
			}
			catch (Exception e)
			{
				Log.Error(e.Message);
				throw;
			}
		}
		
	}

	public async Task<IData?> ReadDataAsync()
	{
		try
		{
			var buffer = new byte[Socket.ReceiveBufferSize];
			Log.Info($"Waiting for data from {Socket.Client.RemoteEndPoint}");
			var bytesRead = await Socket.GetStream()
				.ReadAsync(buffer.AsMemory(0, Socket.ReceiveBufferSize));
			return bytesRead == 0 ? null : AData.Deserialize(buffer);
		}
		catch (IOException)
		{
			throw;
		}
		catch (Exception e)
		{
			Log.Error(e.Message);
			throw;
		}
	}
	
	public async Task SendDataAsync(IData data)
	{
		try
		{
			if (!Socket.Connected)
				throw new Exception("Socket is not connected");
			byte[] buffer = data.Serialize();
			var stream = Socket.GetStream();
			if (stream == null)
				throw new Exception("Stream is null");
			
			await stream.WriteAsync(buffer.AsMemory(0, buffer.Length));
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			throw;
		}
	}


	public void Dispose()
	{
		Dispose(true);
	}

	private void Dispose(bool disposing)
	{
		if (!disposing) return;
		
		if (Socket.Connected)
			Socket.Close();
		Socket.Dispose();
	}

	private void OnClientSentDataInvoke(IData dataReceived)
	{
		DataReceivedEvent?.Invoke(this, dataReceived);
	}

	private void OnClientDisconnectedInvoke()
	{
		DisconnectedEvent?.Invoke(this, this);
	}
}