using AutomationBot.Commands;

namespace AutomationBot.Instructions;

/// <summary>
/// Базовый класс для инструкций
/// </summary>
public abstract class InstructionBase
{
	/// <summary>
	/// Команды для выполнения
	/// </summary>
	protected readonly Queue<ICommand> _commands = [];

	protected InstructionBase()
	{
		_commands = [];
	}

	public abstract Task Execute(CancellationToken cancellationToken);

	/// <summary>
	/// Метод для выполнения команд
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	protected async Task<bool> ExecuteCommands(CancellationToken cancellationToken)
	{
		var success = true;
		while (_commands.Count > 0 && success)
		{
			var command = _commands.Dequeue();
			success = await command.ExecuteAsync(cancellationToken);
		}

		return success;
	}
}
