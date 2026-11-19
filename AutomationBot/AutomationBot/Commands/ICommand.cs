namespace AutomationBot.Commands;

public interface ICommand
{
	Task<bool> ExecuteAsync(CancellationToken cancellationToken);
}
