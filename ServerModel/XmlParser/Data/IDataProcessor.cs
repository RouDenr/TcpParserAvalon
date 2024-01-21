namespace ServerModel.XmlParser.Data;

public interface IDataProcessor
{
	IEnumerable<IData> ProcessData { get; }

	void AddData(IData data);
	void RemoveData(IData data);

	void Init();
	void Update();
}