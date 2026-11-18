using Emgu.CV;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Runtime.InteropServices;

namespace AutomationBot.Managers;

/// <summary>
/// Класс для работы с дисплеем
/// </summary>
internal class ScreenManager
{
	[DllImport("user32.dll")]
	private static extern int GetSystemMetrics(int nIndex);

	[DllImport("user32.dll")]
	private static extern bool GetCursorPos(out Point lpPoint);

	/// <summary>
	/// Номер ширина основного экрана в GetSystemMetrics
	/// </summary>
	private const int SM_CXSCREEN = 0;
	/// <summary>
	/// Номер высоты основного экрана в GetSystemMetrics
	/// </summary>
	private const int SM_CYSCREEN = 1;

	/// <summary>
	/// Константа для нормализации координат WindowsInput по оси X
	/// </summary>
	public const int MAX_ABSOLUTE_X = 65535;
	/// <summary>
	/// Константа для нормализации координат WindowsInput по оси Y
	/// </summary>
	public const int MAX_ABSOLUTE_Y = 65535;

	/// <summary>
	/// Ширина основного дисплея
	/// </summary>
	public static readonly int SCREEN_WIDTH = GetSystemMetrics(SM_CXSCREEN);
	/// <summary>
	/// Высота основного дисплея
	/// </summary>
	public static readonly int SCREEN_HEIGHT = GetSystemMetrics(SM_CYSCREEN);

	/// <summary>
	/// Метод делает скриншот основного дисплея
	/// </summary>
	/// <returns>Скриншот основного дисплея</returns>
	public static Mat TakeScreenshot(ColorConversion conversion = ColorConversion.Bgra2Bgr)
	{
		using var bitmap = new Bitmap(SCREEN_WIDTH, SCREEN_HEIGHT);
		using var graphics = Graphics.FromImage(bitmap);

		graphics.CopyFromScreen(new Point(0, 0), Point.Empty, bitmap.Size);
		var result = new Mat();
		CvInvoke.CvtColor(bitmap.ToMat(), result, conversion);
		
		return result;
	}

	/// <summary>
	/// Возвращает текущую позицию курсора
	/// </summary>
	/// <returns>Позиция курсора</returns>
	public static Point GetCoursorPosition()
	{
		GetCursorPos(out var position);
		return position;
	}
}
