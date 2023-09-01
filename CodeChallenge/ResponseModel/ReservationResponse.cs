﻿namespace CodeChallenge.ResponseModel;

public class ReservationResponse
{
    public string ResponseMessage { get; set; }
    public DateTime AppointmentTime { get; set; }
    public Guid ProviderId { get; set; }
    public bool AppointmentScheduled { get; set; }
}
