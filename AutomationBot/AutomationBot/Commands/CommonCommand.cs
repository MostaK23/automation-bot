namespace AutomationBot.Commands;

/// <summary>
/// Обощенная команда, если нет необходимости создавать отдельную команду
/// </summary>
/// <param name="action">Действие для выполнения</param>
public class CommonCommand(Action action) : ICommand
{
	private readonly Action _action = action;

	public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
	{
		try
		{
			_action.Invoke();
		}
		catch (Exception)
		{
			return false;
		}

		return true;
	}
}
