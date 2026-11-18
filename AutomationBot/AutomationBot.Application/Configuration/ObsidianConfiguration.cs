namespace Application.Configuration;

/// <summary>
/// Конфигурация для запуска Obsidian
/// </summary>
public class ObsidianConfiguration
{
	public string ApplicationPath { get; set; }
	public Dictionary<string, string> Templates { get; set; }
}
