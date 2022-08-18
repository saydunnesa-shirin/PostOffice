namespace PostOffice.Common.Responses;

public class ShipmentResponse
{
    public int ShipmentId { get; set; }
    public string ShipmentNumber { get; set; }
    public Airport Airport { get; set; }
    public string FlightNumber { get; set; }
    public DateTime FlightDate { get; set; }
    public ICollection<BagResponse> Bags { get; set; }
    public Status Status { get; set; } = Status.Initial;
}