using System.ComponentModel;
using System.Diagnostics;

namespace AutomationBot.Commands;

public class StartApplicationCommand(string filePath, bool killOnCancell = false) : ICommand
{
	/// <summary>
	/// Полный путь к исполняемому файлу
	/// </summary>
	private readonly string _filePath = string.IsNullOrWhiteSpace(filePath)
			? throw new ArgumentException("Путь к приложению не может быть пустым.", nameof(filePath))
			: filePath;
	/// <summary>
	/// Принудительно закрывать ли приложение при отмене токена
	/// </summary>
	private readonly bool _killOnCancell = killOnCancell;

	public async Task<bool> Execute(CancellationToken cancellationToken)
	{
		if (!File.Exists(_filePath))
			throw new FileNotFoundException("Исполняемый файл не найден по указанному пути.", _filePath);

		// Проверка отмены еще до запуска тяжелого процесса
		cancellationToken.ThrowIfCancellationRequested();

		var startInfo = new ProcessStartInfo
		{
			FileName = _filePath,
		};

		try
		{
			using var process = new Process { StartInfo = startInfo,  };

			if (!process.Start())
			{
				throw new InvalidOperationException($"Не удалось инициализировать процесс для файла: {_filePath}");
			}

			return true;
		}
		catch (OperationCanceledException)
		{
			// Обработка отмены операции
			Console.WriteLine("Операция запуска/ожидания приложения была отменена.");

			if (_killOnCancell)
			{
				TryKillProcessByPath(_filePath);
			}

			throw; // Пробрасываем дальше, чтобы вызывающий код знал об отмене
		}
		catch (Win32Exception ex)
		{
			// Ошибки уровня ОС: файл поврежден, нет прав доступа (ErrorCode 5), отказ UAC (1223) и т.д.
			Console.WriteLine($"Ошибка Windows при запуске (Код {ex.NativeErrorCode}): {ex.Message}");
			throw;
		}
		catch (UnauthorizedAccessException ex)
		{
			Console.WriteLine($"Отказано в доступе к файлу: {ex.Message}");
			throw;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Непредвиденная ошибка при работе с процессом: {ex.Message}");
			throw;
		}
	}

	private static void TryKillProcessByPath(string filePath)
	{
		try
		{
			string processName = Path.GetFileNameWithoutExtension(filePath);
			foreach (var process in Process.GetProcessesByName(processName))
			{
				process.Kill(entireProcessTree: true);
			}
		}
		catch
		{
			// Игнорируем ошибки при попытке принудительного закрытия (процесс мог уже закрыться сам)
		}
	}
}
