namespace PostOffice.Common.Requests;
public class ParcelRequest
{

    [StringLength(10)]
    [Required]
    public string ParcelNumber { get; set; }

    [MaxLength(100)]
    [Required]
    public string RecipientName { get; set; }

    [StringLength(2)]
    [Required]
    public string DestinationCountry { get; set; }

    [RegularExpression(@"^\d+(\.\d{1,3})?$")]
    [Range(0, 9999999999999999.999)]
    public decimal Weight { get; set; }

    [RegularExpression(@"^\d+(\.\d{1,3})?$")]
    [Range(0, 9999999999999999.99)]
    public decimal Price { get; set; }

    public int BagId { get; set; }
}

public class ParcelUpdateRequest : ParcelRequest
{
    public int ParcelId { get; set; }
}