namespace PostOffice.Repository.Entities;

public class Shipment
{
    public int ShipmentId { get; set; }

    [StringLength(10)]
    [Required]
    public string ShipmentNumber { get; set; }

    public Airport Airport { get; set; }

    [StringLength(6)]
    public string FlightNumber { get; set; }

    public DateTime FlightDate { get; set; }

    [Required]
    public Status Status { get; set; }

    public ICollection<Bag> Bags { get; set; }

    
}