using System.Transactions;

namespace PostOffice.Service.Services;
public interface IBagService
{
    Task<IEnumerable<BagResponse>> GetAllAsync();
    Task<BagResponse> GetByIdAsync(int id);
    Task<int> CreateAsync(BagRequest model);
    Task<bool> UpdateAsync(BagRequest model);
    Task<bool> DeleteAsync(int id);
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

        return bags.Select(bag => _mapper.Map<BagResponse>(bag)).ToList();
    }

    public async Task<BagResponse> GetByIdAsync(int id)
    {
        var bag = await _bagRepository.GetByIdAsync(id);
        if (bag == null) throw new KeyNotFoundException("Bag not found");

        var response = _mapper.Map<BagResponse>(bag);
        return response;
    }

    public async Task<int> CreateAsync(BagRequest model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            // map model to new Bag object
            var bag = _mapper.Map<Bag>(model);

            // save Bag
            var bagId = await _bagRepository.CreateAsync(bag);

            // update parcels
            if (bagId > 0 && model.ParcelIds != null && model.ParcelIds.Count > 0)
            {
                foreach (var parcelId in model.ParcelIds)
                {
                    var parcel = await _parcelRepository.GetByIdAsync(parcelId);
                    parcel.BagId = bagId;
                    await _parcelRepository.UpdateAsync(parcel);
                }
            }

            transaction.Complete();
            return bagId;
        }
        catch (Exception)
        {
            transaction.Dispose();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(BagRequest model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            var shipmentId = model.ShipmentId > 0 ? model.ShipmentId : 0;
            var isValid = await IsValid(model);
            if (!isValid)
                return false;

            // copy model to Bag and update
            var bag = _mapper.Map<Bag>(model);
            var result = await _bagRepository.UpdateAsync(bag);

            if (result && model.ParcelIds != null && model.ParcelIds.Count > 0)
            {
                var existingParcels = await _parcelRepository.GetAllByBagIdAsync(model.BagId);

                var parcels = await _parcelRepository.GetAllAsync(model.ParcelIds);

                var existing = existingParcels.ToList();

                var newParcels = parcels.ToList();
                var parcelsToAdd = newParcels.Except(existing).ToList();
                var parcelsToRemove = existing.Except(newParcels).ToList();

                foreach (var parcel in parcelsToAdd)
                {
                    parcel.BagId = model.BagId;
                    result = await _parcelRepository.UpdateAsync(parcel);
                }

                foreach (var parcel in parcelsToRemove)
                {
                    parcel.BagId = null;
                    result = await _parcelRepository.UpdateAsync(parcel);
                }
            }

            transaction.Complete();
            return result;
        }
        catch (Exception)
        {
            transaction.Dispose();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            var model = await GetByIdAsync(id);

            var shipment = await _shipmentRepository.GetByIdAsync(model.ShipmentId);
            if (shipment != null && shipment.Status == Status.Finalized)
                return false;

            var parcels = await _parcelRepository.GetAllByBagIdAsync(id);

            foreach (var parcel in parcels)
            {
                parcel.BagId = null;
                await _parcelRepository.UpdateAsync(parcel);
            }

            var result = await _bagRepository.DeleteAsync(id);

            transaction.Complete();
            return result;
        }
        catch (Exception)
        {
            transaction.Dispose();
            throw;
        }
    }

    public async Task<bool> IsValid(BagRequest bag)
    {
        var shipmentId = bag.ShipmentId ?? 0;

        if(shipmentId > 0)
        {
            var shipment = await _shipmentRepository.GetByIdAsync(shipmentId);

            if (shipment != null && shipment.Status == Status.Finalized)
                return false;
        }

        if(bag.ContentType == Common.ContentType.Letter && bag.ParcelIds != null && bag.ParcelIds.Count > 0)
            return false;

        if (bag.ContentType == Common.ContentType.Parcel && (bag.ParcelIds == null || (bag.ParcelIds != null && bag.ParcelIds.Count == 0)))
            return false;


        return true;
    }
}