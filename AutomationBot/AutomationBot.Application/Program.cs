using Microsoft.Extensions.Configuration;
using Application.Instructions;
using Application.Configuration;
using AutomationBot.Helpers;

var config = new ConfigurationBuilder()
	.AddJsonFile("config.json")
	.Build();

var obsidianConfig = config.GetSection("instructions").GetSection("obsidian").Get<ObsidianConfiguration>()!;
var obsidianInstruction = new ObsidianInstruction(obsidianConfig, "login", "password");

using var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(20));
await obsidianInstruction.Execute(cts.Token);


//await Task.Delay(1000);
//InputHelper.InputSimulator.Keyboard.TextEntry("Hello world");