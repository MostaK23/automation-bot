using AutomationBot.Constants;
using AutomationBot.Extentions;
using AutomationBot.Helpers;
using AutomationBot.Managers;
using Emgu.CV;

namespace AutomationBot.Commands;

/// <summary>
/// Команда выполняется, пока не будет обнаружен определенный шаблон
/// </summary>
/// <param name="totalDuration">Максимальное время ожидания</param>
/// <param name="waitAll">Нужно ли ожидать появления всех шаблонов</param>
/// <param name="templateImagePaths">Массив путей к изображениям</param>
public class WaitTemplateCommand(TimeSpan totalDuration, bool waitAll = false, params string[] templateImagePaths) : ICommand
{
	/// <summary>
	/// Массив путей к изображениям
	/// </summary>
	private readonly string[] _templateImagePaths = templateImagePaths;
	/// <summary>
	/// Максимальное время ожидания в миллисекундах
	/// </summary>
	private readonly TimeSpan _totalDuration = totalDuration;
	/// <summary>
	/// Время между проверками
	/// </summary>
	private readonly TimeSpan _delayBetweenComparing = TimeSpan.FromMilliseconds(500);
	/// <summary>
	/// Нужно ли ожидать появления всех шаблонов
	/// </summary>
	private bool _waitAll = waitAll;

	public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
	{
		var startTime = DateTime.UtcNow;
		var endTime = startTime + _totalDuration;

		while (DateTime.UtcNow < endTime && !cancellationToken.IsCancellationRequested)
		{
			var source = ScreenManager.TakeScreenshot().ToImage();
			var successCount = 0;

			foreach (var imagePath in _templateImagePaths)
			{
				if (File.Exists(imagePath))
				{
					var templateImage = CvInvoke.Imread(imagePath).ToImage();
					var (_, center, coeff) = CoordinatesHelper.FindTemplateCoordinates(source, templateImage);

					if (coeff > AppConstants.MIN_THRESHOLD_COEFF)
					{
						successCount++;
					}

					if (successCount > 0 && !_waitAll)
					{
						return true;
					}
				}
			}

			if (_waitAll && successCount == _templateImagePaths.Length)
			{
				return true;
			}

			var remainingTime = endTime - DateTime.UtcNow;
			if (remainingTime > _delayBetweenComparing)
			{
				await Task.Delay(_delayBetweenComparing, cancellationToken);
			}
			else
			{
				return false;
			}
		}

		return false;
	}
}
