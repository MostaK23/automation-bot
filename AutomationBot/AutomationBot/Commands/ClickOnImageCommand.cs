using Emgu.CV;
using AutomationBot.Helpers;
using AutomationBot.Extentions;
using AutomationBot.Constants;
using AutomationBot.Managers;

namespace AutomationBot.Commands;

/// <summary>
/// Команда для нажатия на первый совпадающий шаблон
/// </summary>
/// <param name="templateImagePaths">Массив путей к изображениям</param>
public class ClickOnImageCommand(params string[] templateImagePaths) : ICommand
{
	/// <summary>
	/// Массив путей к изображениям
	/// </summary>
	private readonly string[] _templateImagePaths = templateImagePaths ?? throw new ArgumentNullException();

	public async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
	{
		var source = ScreenManager.TakeScreenshot().ToImage();
		foreach (var imagePath in _templateImagePaths)
		{
			if (File.Exists(imagePath))
			{
				var templateImage = CvInvoke.Imread(imagePath).ToImage();
				var (_, center, coeff) = CoordinatesHelper.FindTemplateCoordinates(source, templateImage);

				if (coeff > AppConstants.MIN_THRESHOLD_COEFF)
				{
					await InputHelper.MoveSmoothlyAsync(center, cancellationToken: cancellationToken);
					InputHelper.InputSimulator.Mouse.LeftButtonClick();
					return true;
				}
			}
		}

		return false;
	}
}
