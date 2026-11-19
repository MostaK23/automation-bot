namespace AutomationBot.Commands;

/// <summary>
/// Команда выполняется определенное количество раз, пока не будет выполнена успешно
/// </summary>
/// <param name="command">Команда для выполнения</param>
/// <param name="delay">Задержка между вызовами команды</param>
/// <param name="maxAttempts">Максимальное количество попыток</param>
public class RepeatUntilSuccessCommand(ICommand command, TimeSpan delay, int maxAttempts = 10) : ICommand
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
	private readonly TimeSpan _delay = delay;

	public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
	{
		for (int i = 0; i < _maxAttempts; i++)
		{
			var completed = await _command.ExecuteAsync(cancellationToken);
			if (completed)
			{
				return true;
			}

			await Task.Delay(_delay, cancellationToken);
		}

		return false;
	}
}
