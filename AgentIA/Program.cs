// Import packages
using AgentIA;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

// Create a kernel with Azure OpenAI chat completion
var builder = Kernel.CreateBuilder();
builder.AddAzureOpenAIChatCompletion(
                        deploymentName: "got-4o",
                        endpoint: "https://openaipocrh.openai.azure.com",
                        apiKey: "5fb3753bd5a24d71a13d20cec272e633");

// Add enterprise components
builder.Services.AddSingleton<IFunctionInvocationFilter, FileApproval>(); //Add approval need 
builder.Services.AddLogging(services => services.AddConsole().SetMinimumLevel(LogLevel.Trace)); //Add logs (remove if you want less text at each prompt)
// Build the kernel
Kernel kernel = builder.Build();
//Service d'achèvements de conversation (ChatCompletion)
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

// Add a plugin 
kernel.Plugins.AddFromType<FilePlugin>("Files");

// Enable planning
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};
//Tutorial 

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Make sure you specify the absolute path of directories");
Console.WriteLine("Here is a list of examples of commands");
Console.WriteLine("Get files name of directory");
Console.WriteLine("Get the number of files in a directory");
Console.WriteLine("Move or Copy files from one directory to another");
Console.WriteLine("get files in directory containing a certain content");
Console.WriteLine("search files by name");
Console.ResetColor();


// Create a history store the conversation
var history = new ChatHistory();

// Initiate a back-and-forth chat
string? userInput;

do
{
    // Collect user input
    Console.Write("User > ");
    userInput = Console.ReadLine();

    // Add user input
    history.AddUserMessage(userInput);

    // Get the response from the AI
    var result = await chatCompletionService.GetChatMessageContentAsync(
        history,
        executionSettings: openAIPromptExecutionSettings,
        kernel: kernel);

    // Print the results
    Console.WriteLine("Assistant > " + result);

    // Add the message from the agent to the chat history
    history.AddMessage(result.Role, result.Content ?? string.Empty);
    
} while (userInput is not null);



