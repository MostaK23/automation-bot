namespace AutomationBot.Commands;

public interface ICommand
{
	Task<bool> Execute(CancellationToken cancellationToken);
}
