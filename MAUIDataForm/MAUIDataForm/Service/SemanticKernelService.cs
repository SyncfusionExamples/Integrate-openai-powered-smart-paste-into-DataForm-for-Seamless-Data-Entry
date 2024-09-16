namespace MAUIDataForm
{
    using Microsoft.SemanticKernel;
    using Microsoft.SemanticKernel.ChatCompletion;
    using Microsoft.SemanticKernel.Connectors.OpenAI;

    internal class SemanticKernelService
    {
        const string endpoint = "https://{YOUR_END_POINT}.openai.azure.com";
        const string deploymentName = "GPT35Turbo";
        string key = "API key";

        IChatCompletionService chatCompletionService;
        Kernel kernel;

        internal SemanticKernelService()
        {

        }

        internal async Task<string> GetResponseFromGPT(string userPrompt)
        {
            var builder = Kernel.CreateBuilder().AddAzureOpenAIChatCompletion(deploymentName, endpoint, key);
            this.kernel = builder.Build();
            if (this.kernel != null)
            {
                var chatHistory = new ChatHistory();
                chatHistory.Clear();

                // Add the user's prompt as a user message to the conversation.
                chatHistory.AddSystemMessage("You are a predictive analytics assistant.");

                // Add the user's prompt as a user message to the conversation.
                chatHistory.AddUserMessage(userPrompt);

                // Get the chat completions from kernal.
                chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
                OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings();
                openAIPromptExecutionSettings.ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions;
                try
                {
                    var response = await chatCompletionService.GetChatMessageContentAsync(chatHistory, executionSettings: openAIPromptExecutionSettings, kernel: kernel);
                    return response.ToString();
                }
                catch
                {
                    return "";
                }
            }

            return "";
        }
    }
}