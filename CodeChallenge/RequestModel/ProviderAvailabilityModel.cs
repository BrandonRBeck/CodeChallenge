namespace CodeChallenge.RequestModel;

public class ProviderAvailabilityModel
{
    public Guid ProviderId { get; set; }
    public DateTime AvailabilityStartTime { get; set; }
    public DateTime AvailabilityEndTime { get; set; }

}
