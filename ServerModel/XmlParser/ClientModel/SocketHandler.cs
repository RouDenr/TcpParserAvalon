using System.Net.Sockets;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public class SocketHandler : IConnectManage
{
	public bool IsConnected => Socket.Connected;
	
	public event EventHandler<IData>? DataReceivedEvent;
	public event EventHandler<SocketHandler>? DisconnectedEvent;
	
	protected TcpClient Socket { get; }
	
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
			finally
			{
				Dispose();
			}
		}
		
	}

	public async Task<IData?> ReadDataAsync()
	{
		try
		{
			var buffer = new byte[Socket.ReceiveBufferSize];
			var bytesRead = await Socket.GetStream()
				.ReadAsync(buffer.AsMemory(0, Socket.ReceiveBufferSize));
			return bytesRead == 0 ? null : AData.Deserialize(buffer);
		}
		catch (IOException)
		{
			// Client disconnected
			OnClientDisconnectedInvoke();
			return null;
		}
	}
	
	public async Task SendDataAsync(IData data)
	{
		try
		{
			byte[] buffer = data.Serialize();
			await Socket.GetStream().WriteAsync(buffer.AsMemory(0, buffer.Length));
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