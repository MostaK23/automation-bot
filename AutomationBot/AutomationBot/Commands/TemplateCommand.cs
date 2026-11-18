namespace AutomationBot.Commands;

public class TemplateCommand(Action action) : ICommand
{
	private readonly Action _action = action;

	public async Task<bool> Execute(CancellationToken cancellationToken)
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
