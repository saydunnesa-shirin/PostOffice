namespace PostOffice.Common.Responses;
public class BagResponse
{
    public int BagId { get; set; }
    public string BagNumber { get; set; }
    public ContentType ContentType { get; set; }
    public decimal Weight { get; set; }
    public decimal Price { get; set; }
    public int ItemCount { get; set; }

    public int ShipmentId { get; set; }
    public ICollection<ParcelResponse> Parcels { get; set; }
}