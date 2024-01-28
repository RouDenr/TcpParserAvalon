using System.Net.Sockets;
using System.Runtime.Serialization;
using NLog;
using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public class SocketHandler : IConnectHandle
{
	public bool IsConnected => Socket.Connected;
	
	public event EventHandler<IData>? DataReceivedEvent;
	public event EventHandler<IData>? DataSentInvoke;
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
				Log.Error(e);
			}
		}
		
	}

	public async Task<IData?> ReadDataAsync()
	{
		try
		{
			Log.Info($"Waiting for data from {Socket.Client.RemoteEndPoint}");
			var data = await ReadAllBytes();
			Log.Info($"Received data from {Socket.Client.RemoteEndPoint} size: {data?.Serialize().Length}");
			return data;
		}
		catch (IOException)
		{
			throw;
		}
		catch (Exception e)
		{
			Log.Error(e);
			throw;
		}
	}

	private async Task<IData?> ReadAllBytes()
	{
		MemoryStream memoryStream = new ();
		NetworkStream stream = Socket.GetStream();
		byte[] buffer = new byte[Socket.ReceiveBufferSize];
		IData? data = null;
		
		// TODO: Можно просто сначала отправить размер, а потом уже данные!!! Я ДОЛБАЕБ
		while (data == null)
		{
			var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
			memoryStream.Write(buffer, 0, bytesRead);
			data = TryDeserializeData(memoryStream.ToArray());
		}


		return data;
	}

	private IData? TryDeserializeData(byte[] buffer)
	{
		try
		{
			return AData.Deserialize(buffer);
		}
		catch (SerializationException e)
		{
			Log.Info(e.Message);
			return null;
		}
		catch (Exception e)
		{
			Log.Error(e);
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
			
			Log.Info($"Sending data to {Socket.Client.RemoteEndPoint} size: {buffer.Length}");
			await stream.WriteAsync(buffer.AsMemory(0, buffer.Length));
			OnDataSentInvoke(data);
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
	private void OnDataSentInvoke(IData data)
	{
		DataSentInvoke?.Invoke(this, data);
	}
}