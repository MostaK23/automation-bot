using AutomationBot.Commands;

namespace AutomationBot.Instructions;

public abstract class InstructionBase
{
	protected readonly Queue<ICommand> _commands = [];

	protected InstructionBase()
	{
		_commands = [];
	}

	public abstract Task Execute(CancellationToken cancellationToken);

	protected async Task<bool> ExecuteCommands(CancellationToken cancellationToken)
	{
		var success = true;
		while (_commands.Count > 0 && success)
		{
			var command = _commands.Dequeue();
			success = await command.Execute(cancellationToken);
		}

		return success;
	}
}
