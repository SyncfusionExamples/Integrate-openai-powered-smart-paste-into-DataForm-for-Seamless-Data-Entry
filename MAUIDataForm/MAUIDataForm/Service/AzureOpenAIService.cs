namespace MAUIDataForm
{
    using Azure;
    using Azure.AI.OpenAI;

    internal class AzureOpenAIService
    {
        const string endpoint = "https://{YOUR_END_POINT}.openai.azure.com";
        const string deploymentName = "GPT35Turbo";
        string key = "API key";

        OpenAIClient? client;
        ChatCompletionsOptions? chatCompletions;

        internal AzureOpenAIService()
        {

        }

        internal async Task<string> GetResponseFromGPT(string userPrompt)
        {
            this.chatCompletions = new ChatCompletionsOptions
            {
                DeploymentName = deploymentName,
                Temperature = (float)0.5,
                MaxTokens = 800,
                NucleusSamplingFactor = (float)0.95,
                FrequencyPenalty = 0,
                PresencePenalty = 0,
            };

            this.client = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(key));
            if (this.client != null)
            {
                // Add the user's prompt as a user message to the conversation.
                this.chatCompletions?.Messages.Add(new ChatRequestSystemMessage("You are a predictive analytics assistant."));

                // Add the user's prompt as a user message to the conversation.
                this.chatCompletions?.Messages.Add(new ChatRequestUserMessage(userPrompt));
                try
                {
                    // Send the chat completion request to the OpenAI API and await the response.
                    var response = await this.client.GetChatCompletionsAsync(this.chatCompletions);

                    // Return the content of the first choice in the response, which contains the AI's answer.
                    return response.Value.Choices[0].Message.Content;
                }
                catch
                {
                    // If an exception occurs (e.g., network issues, API errors), return an empty string.
                    return "";
                }
            }

            return "";
        }
    }
}