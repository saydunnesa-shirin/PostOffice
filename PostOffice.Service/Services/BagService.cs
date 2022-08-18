using PostOffice.Common.Requests;
using PostOffice.Repository.Entities;

namespace PostOffice.Service.Services;
public interface IBagService
{
    Task<IEnumerable<BagResponse>> GetAllAsync();
    Task<BagResponse> GetByIdAsync(int id);
    Task CreateAsync(BagRequest model);
    Task UpdateAsync(BagRequest model);
    Task DeleteAsync(int id);
}

public class BagService : IBagService
{
    private readonly IMapper _mapper;

    private readonly IBagRepository _bagRepository;

    private readonly IParcelRepository _parcelRepository;

    private readonly IShipmentRepository _shipmentRepository;
    public BagService(IMapper mapper, IBagRepository bagRepository, IParcelRepository parcelRepository, IShipmentRepository shipmentRepository)
    {
        _mapper = mapper;
        _bagRepository = bagRepository;
        _parcelRepository = parcelRepository;
        _shipmentRepository = shipmentRepository;
    }

    public async Task<IEnumerable<BagResponse>> GetAllAsync()
    {
        var bags = await _bagRepository.GetAllAsync();

        List<BagResponse> responses = new List<BagResponse>();

        foreach (var bag in bags)
        {
            responses.Add(_mapper.Map<BagResponse>(bag));
        }
        return responses;
    }

    public async Task<BagResponse> GetByIdAsync(int id)
    {
        var bag = await _bagRepository.GetByIdAsync(id);
        if (bag == null) throw new KeyNotFoundException("Bag not found");

        var response = _mapper.Map<BagResponse>(bag);
        return response;
    }

    public async Task CreateAsync(BagRequest model)
    {
        if (model == null) throw new ArgumentNullException("model");

        // map model to new Bag object
        var bag = _mapper.Map<Bag>(model);

        // save Bag
        var bagId = await _bagRepository.CreateAsync(bag);

        if(bagId > 0 && model.ParcelRequests != null && model.ParcelRequests.Count > 0)
        {
            foreach (var parcelRequest in model.ParcelRequests)
            {
                var parcel = _mapper.Map<Parcel>(parcelRequest);

                parcel.BagId = bagId;
                await _parcelRepository.CreateAsync(parcel);
            }
        }
    }

    public async Task UpdateAsync(BagRequest model)
    {
        if (model == null) throw new ArgumentNullException("model");

        if(!IsValid(model.ShipmentId ?? 0))
            return;

        // copy model to Bag and update
        var bag = _mapper.Map<Bag>(model);
        await _bagRepository.UpdateAsync(bag);

        if (model.ParcelRequests != null && model.ParcelRequests.Count > 0)
        {
            var parcels = await _parcelRepository.GetAllByBagIdAsync(model.BagId);

            var newParcels = model.ParcelRequests.Where(q => q.ParcelId == 0).ToList();

            foreach (var parcelRequest in newParcels)
            {
                var parcel = _mapper.Map<Parcel>(parcelRequest);
                parcel.BagId = model.BagId;
                await _parcelRepository.CreateAsync(parcel);
            }

            var excludeParcels = parcels.Where(q => model.ParcelRequests.Any(p => p.ParcelId > 0 && p.ParcelId != q.ParcelId)).ToList();

            foreach (var parcel in excludeParcels)
            {
                parcel.BagId = null;
                await _parcelRepository.UpdateAsync(parcel);
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        var model = await GetByIdAsync(id);

        if (!IsValid(model?.ShipmentId ?? 0))
            return;

        var parcels = await _parcelRepository.GetAllByBagIdAsync(id);

        foreach (var parcel in parcels)
        {
            await _parcelRepository.DeleteAsync(parcel.ParcelId);
        }

        await _bagRepository.DeleteAsync(id);
    }

    public bool IsValid(int shipmentId)
    {
        if(shipmentId > 0)
        {
            var shipment = _shipmentRepository.GetByIdAsync(shipmentId);

            if (shipment != null && (Status)shipment.Status == Status.Finalized)
                return false;
        }

        return true;
    }
}