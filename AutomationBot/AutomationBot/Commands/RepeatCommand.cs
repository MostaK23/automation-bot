namespace AutomationBot.Commands;

public class RepeatCommand(ICommand command, int maxAttempts = 10, int millisecondsDelay = 500) : ICommand
{
	/// <summary>
	/// Команда для выполнения
	/// </summary>
	private readonly ICommand _command = command;
	/// <summary>
	/// Максимальное количество попыток
	/// </summary>
	private readonly int _maxAttempts = maxAttempts;
	/// <summary>
	/// Задержка между попытками в миллисекундах
	/// </summary>
	private readonly int _delay = millisecondsDelay;

	public async Task<bool> Execute(CancellationToken cancellationToken)
	{
		for (int i = 0; i < _maxAttempts; i++)
		{
			var completed = await _command.Execute(cancellationToken);
			if (completed)
			{
				return true;
			}

			await Task.Delay(_delay, cancellationToken);
		}

		return false;
	}
}
