namespace AutomationBot.Application.Configuration;

/// <summary>
/// Конфигурация для запуска Obsidian
/// </summary>
public class ObsidianConfiguration
{
	/// <summary>
	/// Путь к исполняемому файлу
	/// </summary>
	public string ApplicationPath { get; set; }
	/// <summary>
	/// Необходимые шаблоны (пути к файлам)
	/// </summary>
	public Dictionary<string, string> Templates { get; set; }
}
