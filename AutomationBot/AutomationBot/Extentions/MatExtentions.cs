using Emgu.CV;
using Emgu.CV.Structure;

namespace AutomationBot.Extentions;

internal static class MatExtentions
{
	public static Image<Bgr, byte> ToImage(this Mat mat)
	{
		return new Image<Bgr, byte>(mat);
	}
}
