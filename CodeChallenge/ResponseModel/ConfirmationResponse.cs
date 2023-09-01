namespace CodeChallenge.ResponseModel;

public class ConfirmationResponse
{
    public string ResponseMessage { get; set; }
    public DateTime AppointmentTime { get; set; }
    public Guid ProviderId { get; set; }
}
