namespace PostOffice.Service.Helpers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<ShipmentResponse, Shipment>().ReverseMap();
        CreateMap<ShipmentRequest, Shipment>().ReverseMap();
        CreateMap<BagResponse, Bag>().ReverseMap();
        CreateMap<BagRequest, Bag>();
        CreateMap<ParcelResponse, Parcel>().ReverseMap();
        CreateMap<ParcelRequest, Parcel>();
        CreateMap<ShipmentUpdateRequest, Shipment>();
        CreateMap<BagUpdateRequest, Bag>();
        CreateMap<ParcelUpdateRequest, Parcel>();
    }
}