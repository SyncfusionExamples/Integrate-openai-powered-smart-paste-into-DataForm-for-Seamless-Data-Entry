namespace MAUIDataForm
{
    using System.ComponentModel.DataAnnotations;

    public class FeedBackForm
    {
        public FeedBackForm()
        {
            this.Name = string.Empty;
            this.Email = string.Empty;
            this.ProductName = string.Empty;
            this.ProductVersion = string.Empty;
            this.Rating = 0;
            this.Comments = string.Empty;
        }

        [Display(Name = "Name", Prompt = "Enter your name")]
        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; }

        [Display(Name = "Email", Prompt = "Enter your email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Display(Name = "Product Name", Prompt = "Example: Scheduler")]
        public string ProductName { get; set; }

        [Display(Name = "Product Version", Prompt = "Example: 26.2.8")]
        public string ProductVersion { get; set; }

        [Display(Name = "Rating", Prompt = "Rating between 1-5")]
        [Range(1, 5, ErrorMessage = "Rating should be between 1 and 5")]
        public int Rating { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ShortName = "Describe your feedback in detail", Name = "Comments")]
        public string Comments { get; set; }
    }
}