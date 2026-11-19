using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Drawing;

namespace AutomationBot.Helpers;

/// <summary>
/// Класс для работы с координатами
/// </summary>
internal class CoordinatesHelper
{
	/// <summary>
	/// Метод для поиска координат заданного шаблона на изображении
	/// </summary>
	/// <param name="source">Изображение, где необходимо производить поиск</param>
	/// <param name="template">Шаблон, который необходимо найти</param>
	/// <returns>
	/// leftTopCorner - левая верхняя точка найденного шаблона <para/>
	/// center - центр найденного шаблона <para/>
	/// coefficient - коэффициент подобия найденного фрагмента на шаблон (от 0.85 - ок)
	/// </returns>
	public static (Point leftTopCorner, Point center, double coefficient) FindTemplateCoordinates(
		Image<Bgr, byte> source, Image<Bgr, byte> template)
	{
		var graySource = source.Convert<Gray, byte>();
		var grayTemplate = template.Convert<Gray, byte>();

		var result = graySource.MatchTemplate(grayTemplate, TemplateMatchingType.CcoeffNormed);
		result.MinMax(out _, out var coeffs, out _, out var points);

		var coeff = coeffs[0];
		var point = points[0];

		for (var i = 1; i < coeffs.Length; i++)
		{
			if (coeffs[i] > coeff)
			{
				coeff = coeffs[i];
				point = points[i];
			}
		}

		var center = new Point
		{
			X = point.X + grayTemplate.Width / 2,
			Y = point.Y + grayTemplate.Height / 2
		};

		return (point, center, coeff);
	}
}
