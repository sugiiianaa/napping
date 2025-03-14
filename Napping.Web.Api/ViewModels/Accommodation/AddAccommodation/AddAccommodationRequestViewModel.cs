namespace Napping.Web.Api.ViewModels.Accommodation.AddAccommodation
{
    public class AddAccommodationRequestViewModel
    {
        public required string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string Address { get; set; }
        public decimal Rating { get; set; }
    }
}
