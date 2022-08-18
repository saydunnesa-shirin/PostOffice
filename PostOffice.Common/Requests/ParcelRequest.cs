namespace PostOffice.Common.Requests;
public class ParcelRequest
{
    public int ParcelId { get; set; }

    [StringLength(10)]
    [Required]
    public string ParcelNumber { get; set; }

    [MaxLength(100)]
    [Required]
    public string RecipientName { get; set; }

    [StringLength(2)]
    [Required]
    public string DestinationCountry { get; set; }

    public decimal Weight { get; set; }

    public decimal Price { get; set; }

    public int? BagId { get; set; }
}
