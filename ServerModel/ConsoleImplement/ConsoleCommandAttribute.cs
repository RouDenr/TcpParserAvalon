namespace ServerModel.ConsoleImplement;

public delegate void ConsoleCommandDelegate();

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
sealed class ConsoleCommandAttribute(string name, string description, params string[] args) : Attribute
{
	public string Name { get; set; } = name;
	public string Description { get; set; } = description;
	public string[] Args { get; set; } = args;
}