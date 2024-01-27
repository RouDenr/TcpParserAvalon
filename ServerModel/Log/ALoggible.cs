using NLog;

namespace ServerModel.Log;

public abstract class ALoggable {
	protected static readonly Logger Log = LogManager.GetCurrentClassLogger();
	
	protected void LogException(Exception e)
	{
		Log.Error(e, $"Exception: {e.Message}");
	}
}