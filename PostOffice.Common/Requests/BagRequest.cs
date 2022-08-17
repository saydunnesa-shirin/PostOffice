namespace PostOffice.Common.Requests;
public class BagRequest
{
    
    [MaxLength(15)]
    [Required]
    public string BagNumber { get; set; }
    public ContentType ContentType { get; set; }
    public decimal Weight { get; set; }
    public decimal Price { get; set; }
    public int ItemCount { get; set; }
    public int ShipmentId { get; set; }
}

public class BagUpdateRequest : BagRequest
{
    public int BagId { get; set; }
}