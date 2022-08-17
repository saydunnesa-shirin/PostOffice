namespace PostOffice.Common.Requests;

public class ShipmentRequest
{
    [StringLength(10)]
    [Required]
    public string ShipmentNumber { get; set; }

    public Airport Airport { get; set; }

    [StringLength(6)]
    [Required]
    public string FlightNumber { get; set; }

    public DateTime FlightDate { get; set; }
    public Status Status { get; set; }
}

public class ShipmentUpdateRequest : ShipmentRequest
{
    public int ShipmentId { get; set; }
}
