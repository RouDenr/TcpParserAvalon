using ServerModel.XmlParser.Data;

namespace ServerModel.XmlParser.ClientModel;

public class TcpRequestHandler : IRequestHandler
{
	public void HandleResponse(ClientHandler client, IData dataReceived)
	{
		switch (dataReceived)
		{
			case SimpleTextData simpleTextData:
				TextHandler(client, simpleTextData);
				break;
			case XmlData xmlData:
				XmlHandler(client, xmlData);
				break;
			default:
				throw new Exception($"Unexpected data type: {dataReceived.GetType()}");
		}
	}

	public void HandleResponse(object? sender, IData dataReceived)
	{
		if (sender is not ClientHandler client)
			throw new Exception("Failed to cast sender to ClientHandler");
		HandleResponse(client, dataReceived);
	}

	private void XmlHandler(ClientHandler client, XmlData xmlData)
	{
		
		
	}

	
	
	private const string HelpRuleFormat = "help";
	private const string NameRuleFormat = "name: ";
	private const string ResendRuleFormat = "resend";
	private void TextHandler(ClientHandler client, SimpleTextData dataReceived)
	{
		var split = dataReceived.Text.Split(' ', 2);

		switch (split[0])
		{
			case HelpRuleFormat:
				HelpMessageHandler(client, dataReceived);
				break;
			case NameRuleFormat:
				NamеMessageHandler(client, dataReceived);
				break;
			case ResendRuleFormat:
				ResendMessageHandler(client, dataReceived);
				break;
			default:
				throw new Exception($"Unexpected text: {dataReceived.Text}");
		}
	}

	private void HelpMessageHandler(ClientHandler client, SimpleTextData dataReceived)
	{
		throw new NotImplementedException();
	}

	private void ResendMessageHandler(ClientHandler client, SimpleTextData dataReceived)
	{
		throw new NotImplementedException();
	}


	private void NamеMessageHandler(ClientHandler client, SimpleTextData nameMessage)
	{
		if (client.Name != string.Empty)
			return;
		// message should be in format: "name: <name>"
		if (!nameMessage.Text.StartsWith(NameRuleFormat))
			return;
		client.Name = nameMessage.Text[NameRuleFormat.Length..];
	}
	
	
}