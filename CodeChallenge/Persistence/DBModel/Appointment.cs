using System.ComponentModel.DataAnnotations;

namespace CodeChallenge.Persistence.DBModel;

public class Appointment
{
    [Required]
    public Guid AppointmentId { get; set; }
    public Guid? ReservationHolderId { get; set; }
    public Guid ProviderId { get; set; }

    public DateTime AppointmentStartTime { get; set; }

    public bool IsReserved { get; set; }
    public bool IsConfirmed { get; set; }

    public DateTime? ReservationTimestamp { get; set; }

}
