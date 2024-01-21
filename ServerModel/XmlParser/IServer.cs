namespace ServerModel.XmlParser;

public interface IServer
{
	void Start();

	void Stop();
	IEnumerable<IClient> Clients { get; }
}