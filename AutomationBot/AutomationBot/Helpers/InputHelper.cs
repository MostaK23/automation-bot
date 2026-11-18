using AutomationBot.Managers;
using System.Drawing;
using WindowsInput;

namespace AutomationBot.Helpers;

public class InputHelper
{
	public static readonly InputSimulator InputSimulator = new();

	public static async Task MoveSmoothlyAsync(Point targetPoint,
		int durationMs = 200,
		int steps = 50,
		CancellationToken cancellationToken = default)
	{
		// Получаем текущую позицию курсора
		var startPoint = ScreenManager.GetCoursorPosition();

		int delayPerStep = Math.Max(1, durationMs / steps);

		for (int i = 1; i <= steps; i++)
		{
			cancellationToken.ThrowIfCancellationRequested();

			// Коэффициент прогресса от 0.0 до 1.0
			double progress = (double)i / steps;

			// Необязательно: плавное замедление в конце (Ease-Out), чтобы движение выглядело естественнее
			double easedProgress = Easing(progress);

			// Вычисляем текущие пиксели
			double currentX = startPoint.X + (targetPoint.X- startPoint.X) * easedProgress;
			double currentY = startPoint.Y + (targetPoint.Y - startPoint.Y) * easedProgress;

			// Переводим пиксели в систему координат WindowsInput
			double normalizedX = (currentX * ScreenManager.MAX_ABSOLUTE_X) / ScreenManager.SCREEN_WIDTH;
			double normalizedY = (currentY * ScreenManager.MAX_ABSOLUTE_Y) / ScreenManager.SCREEN_HEIGHT;

			InputSimulator.Mouse.MoveMouseTo(normalizedX, normalizedY);

			await Task.Delay(delayPerStep, cancellationToken);
		}
	}

	private static double Easing(double progress)
	{
		return 1 - Math.Pow(1 - progress, 3);
	}
}
