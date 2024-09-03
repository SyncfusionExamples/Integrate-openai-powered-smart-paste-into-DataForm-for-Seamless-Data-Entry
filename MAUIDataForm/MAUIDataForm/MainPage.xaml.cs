namespace MAUIDataForm
{
    using Newtonsoft.Json;

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        string clipboardText;
        private AzureOpenAIService azureAIService = new AzureOpenAIService();

        private async void OnSmartPasteButtonClicked(object sender, EventArgs e)
        {
            if (Clipboard.Default.HasText)
            {
                this.clipboardText = await Clipboard.Default.GetTextAsync();
            }

            if (string.IsNullOrEmpty(this.clipboardText))
            {
                await App.Current.MainPage.DisplayAlert("", "No text copied to clipboard. Please copy the text and try again", "OK");

                return;
            }

            string dataFormJsonData = JsonConvert.SerializeObject(this.feedbackForm!.DataObject);
            string prompt = $"Merge the copied data into the DataForm field content, ensuring that the copied text matches suitable field names. Here are the details:" +
            $"\n\nCopied data: {this.clipboardText}," +
            $"\nDataForm Field Name: {dataFormJsonData}," +
            $"\nProvide the resultant DataForm directly." +
            $"\n\nConditions to follow:" +
            $"\n1. Do not use the copied text directly as the field name; merge appropriately." +
            $"\n2. Ignore case sensitivity when comparing copied text and field names." +
            $"\n3. Final output must be Json format" +
            $"\n4. No need any explanation or comments in the output" +
            $"\n Please provide the valid JSON object without any additional formatting characters like backticks or newlines";
            string finalResponse = await this.azureAIService.GetResponseFromGPT(prompt);
            this.ProcessSmartPasteData(finalResponse);
        }

        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            if (this.feedbackForm != null && App.Current?.MainPage != null)
            {
                if (this.feedbackForm.Validate())
                {
                    await App.Current.MainPage.DisplayAlert("", "Feedback form submitted successfully", "OK");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("", "Please enter the required details", "OK");
                }
            }
        }

        private void ProcessSmartPasteData(string response)
        {
            //// Deserialize the JSON string to a Dictionary
            var openAIJsonData = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

            //// Create lists to hold field names and values
            var filedNames = new List<string>();
            var fieldValues = new List<string>();

            foreach (var data in openAIJsonData!)
            {
                filedNames.Add(data.Key);
                fieldValues.Add(data.Value?.ToString() ?? string.Empty);
            }

            if (this.feedbackForm.DataObject is FeedBackForm feedbackForm)
            {
                feedbackForm.Name = fieldValues[0];
                feedbackForm.Email = fieldValues[1];
                feedbackForm.ProductName = fieldValues[2];
                feedbackForm.ProductVersion = fieldValues[3];
                feedbackForm.Rating = (int)Math.Round(double.Parse(fieldValues[4]));
                feedbackForm.Comments = fieldValues[5];
            };

            for (int i = 0; i < filedNames.Count; i++)
            {
                this.feedbackForm!.UpdateEditor(filedNames[i]);
            }
        }
    }
}