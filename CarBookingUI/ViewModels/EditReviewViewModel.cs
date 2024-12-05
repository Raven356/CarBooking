namespace CarBookingUI.ViewModels
{
    public class EditReviewViewModel : BaseViewModel
    {
        private int carId;
        private int reviewId;
        private string text;
        private int userId;
        private int rating;

        public int ReviewId
        {
            get => reviewId;
            set
            {
                reviewId = value;
                OnPropertyChanged();
            }
        }

        public int OrderId
        {
            get => carId;
            set { carId = value; OnPropertyChanged(); }
        }

        public string Text
        {
            get => text;
            set { text = value; OnPropertyChanged(); }
        }

        public int UserId
        {
            get => userId;
            set { userId = value; OnPropertyChanged(); }
        }

        public int Rating
        {
            get => rating;
            set
            {
                if (rating != value)
                {
                    rating = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StarImages));
                }
            }
        }

        public EditReviewViewModel(int reviewId)
        {
            this.reviewId = reviewId;
        }

        // Star images based on the rating
        public string[] StarImages => new string[5]
            .Select((_, index) => index < Rating ? "star_filled.png" : "star_empty.png")
            .ToArray();
    }
}
