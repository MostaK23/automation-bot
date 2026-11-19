using AutomationBot.Application.Configuration;
using AutomationBot.Application.Instructions;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
	.AddJsonFile("config.json")
	.Build();

var obsidianConfig = config.GetSection("instructions").GetSection("obsidian").Get<ObsidianConfiguration>()!;
var obsidianInstruction = new ObsidianInstruction(obsidianConfig, "login", "password");

using var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(20));
await obsidianInstruction.Execute(cts.Token);

//var wait = new WaitTemplateCommand(
//	TimeSpan.FromSeconds(10),
//	templateImagePaths: [obsidianConfig.Templates["settings"], obsidianConfig.Templates["enter_rus"]],
//	waitAll: true
//);

//var result = await wait.ExecuteAsync(cts.Token);
