namespace MAUIDataForm
{
    public class FeedbackViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackViewModel " /> class.
        /// </summary>
        public FeedbackViewModel()
        {
            this.ViewModel = new FeedBackForm();
        }

        /// <summary>
        /// Gets or sets the feedback form model.
        /// </summary>
        public FeedBackForm ViewModel { get; set; }
    }
}