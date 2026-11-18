using Emgu.CV;
using AutomationBot.Helpers;
using AutomationBot.Extentions;
using AutomationBot.Constants;
using AutomationBot.Managers;

namespace AutomationBot.Commands;

/// <summary>
/// Перебирает изображение, пока не будет найдено одно из них
/// </summary>
/// <param name="templateImagePaths">Массив путей к изображениям</param>
public class ClickOnImageCommand(params string[] templateImagePaths) : ICommand
{
	private readonly string[] _templateImagePaths = templateImagePaths ?? throw new ArgumentNullException();

	public async Task<bool> Execute(CancellationToken cancellationToken)
	{
		var source = ScreenManager.TakeScreenshot().ToImage();
		foreach (var templateImagePath in _templateImagePaths)
		{
			if (File.Exists(templateImagePath))
			{
				var templateImage = CvInvoke.Imread(templateImagePath).ToImage();
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
