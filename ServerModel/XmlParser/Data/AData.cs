using System.Runtime.Serialization.Formatters.Binary;
using ServerModel.Log;

namespace ServerModel.XmlParser.Data;

[Serializable]
public abstract class AData : IData
{
	public abstract IData Clone();

	public byte[] Serialize()
	{
		using MemoryStream ms = new();
#pragma warning disable SYSLIB0011
		BinaryFormatter formatter = new();
		formatter.Serialize(ms, this);
		return ms.ToArray();
	}
	
	public static IData Deserialize(byte[] data)
	{
		
		using MemoryStream ms = new(data);
		
#pragma warning disable SYSLIB0011
		BinaryFormatter formatter = new();
		var dataObject = formatter.Deserialize(ms);
		
		if (dataObject == null)
			throw new Exception("Failed to deserialize data");
		if (dataObject is not IData deserializeData)
			throw new Exception($"Failed to cast data to {nameof(XmlData)} type: {dataObject.GetType()}");
		
		return deserializeData;
	}
}