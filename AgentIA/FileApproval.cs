using Microsoft.SemanticKernel;

namespace AgentIA
{
    public class FileApproval() : IFunctionInvocationFilter
    {
        public async Task OnFunctionInvocationAsync(FunctionInvocationContext context, Func<FunctionInvocationContext, Task> next)
        {
            if (context.Function.PluginName == "Files")
            {
                Console.WriteLine("System > The agent wants to create an approval to gain access to your files, do you want to proceed? (y/n)");
                string shouldProceed = Console.ReadLine()!;
                if (shouldProceed != "y")
                {
                    context.Result = new FunctionResult(context.Result, "The order creation was not approved by the user");
                    return;
                }
            }
            await next(context);
        }
    }
}
