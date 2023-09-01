namespace CodeChallenge.Models
{
    public class ReservationResponse
    {
        public string ResponseMessage { get; set; }
        public string AppointmentTime { get; set; }
        public string ProviderName { get; set; }
        public Guid ProviderId { get; set; }
        public bool AppointmentScheduled { get; set; }
    }
}
