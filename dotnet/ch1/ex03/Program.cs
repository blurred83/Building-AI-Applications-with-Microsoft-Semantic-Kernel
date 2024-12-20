﻿using Microsoft.SemanticKernel;

var (apiKey, orgId, _, _, _) = Settings.LoadSettings();

Kernel kernel = Kernel.CreateBuilder()
                        .AddOpenAIChatCompletion("gpt-3.5-turbo", apiKey, orgId, serviceId: "gpt3")
                        .AddOpenAIChatCompletion("gpt-4", apiKey, orgId, serviceId: "gpt4")
                        .Build();

string prompt = "Finish the following knock-knock joke. Knock, knock. Who's there? {{$input}}, {{$input}} who?";
KernelFunction jokeFunction = kernel.CreateFunctionFromPrompt(prompt);

var showManagerPlugin = kernel.ImportPluginFromObject(new Plugins.ShowManager());

var result = await kernel.InvokeAsync(showManagerPlugin["RandomTheme"]);
Console.WriteLine("I will tell a joke about " + result);

var arguments = new KernelArguments() { ["input"] = result };

var joke = await kernel.InvokeAsync(jokeFunction, arguments);
Console.WriteLine(joke);
