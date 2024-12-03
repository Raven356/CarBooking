using CarBookingUI.Models;
using System.Collections.ObjectModel;

namespace CarBookingUI.ViewModels
{
    public class ReviewsPageViewModel : BaseViewModel
    {
        private ObservableCollection<Review> reviews;

        public ObservableCollection<Review> Reviews { get { return reviews; } set { reviews = value; OnPropertyChanged(); } }
    }
}
