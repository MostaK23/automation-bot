using Application.Configuration;
using AutomationBot.Commands;
using AutomationBot.Helpers;
using AutomationBot.Instructions;

namespace Application.Instructions;

/// <summary>
/// Автоматизация запуска Obsidian (пример)
/// </summary>
/// <param name="configuration"></param>
/// <param name="login"></param>
/// <param name="password"></param>
public class ObsidianInstruction(ObsidianConfiguration configuration, string login, string password) : InstructionBase()
{
	/// <summary>
	/// Конфигурация для инструкции
	/// </summary>
	private readonly ObsidianConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
	/// <summary>
	/// Логин для входа
	/// </summary>
	private readonly string _login = login ?? throw new ArgumentNullException(nameof(login));
	/// <summary>
	/// Пароль для входа
	/// </summary>
	private readonly string _password = password ?? throw new ArgumentNullException(nameof(password));

	public override async Task Execute(CancellationToken cancellationToken)
	{
		try
		{
			_commands.Enqueue(new StartApplicationCommand(_configuration.ApplicationPath));
			_commands.Enqueue(new RepeatCommand(new ClickOnImageCommand(
				_configuration.Templates["settings"])));
			_commands.Enqueue(new RepeatCommand(new ClickOnImageCommand(
				_configuration.Templates["enter_rus"], _configuration.Templates["enter_eng"])));
			_commands.Enqueue(new TemplateCommand(() =>
			{
				Task.Delay(200, cancellationToken);
				InputHelper.InputSimulator.Keyboard.TextEntry(_login);
				InputHelper.InputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
				InputHelper.InputSimulator.Keyboard.TextEntry(_password);
				Task.Delay(200, cancellationToken);
				InputHelper.InputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
			}));
			
			await ExecuteCommands(cancellationToken);			
		}
		catch (Exception e)
		{
			Console.WriteLine(e.Message);
		}
	}
}
