namespace PostOffice.Common.Responses;
public class ParcelResponse
{
    public int ParcelId { get; set; }
    public string ParcelNumber { get; set; }
    public string RecipientName { get; set; }
    public string DestinationCountry { get; set; }
    public decimal Weight { get; set; }
    public decimal Price { get; set; }
    public int BagId { get; set; }
}