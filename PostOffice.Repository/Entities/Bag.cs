namespace PostOffice.Repository.Entities;
public class Bag
{
    public int BagId { get; set; }
    
    [MaxLength(15)]
    [Required]
    public string BagNumber { get; set; }

    public ContentType ContentType { get; set; }
    public decimal Weight { get; set; }
    public decimal Price { get; set; }
    public int ItemCount { get; set; }
    public int? ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }
    public ICollection<Parcel> Parcels { get; set; }
}