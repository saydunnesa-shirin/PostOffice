using System.Transactions;

namespace PostOffice.Service.Services;
public interface IShipmentService
{
    Task<IEnumerable<ShipmentResponse>> GetAllAsync();
    Task<ShipmentResponse> GetByIdAsync(int id);
    Task<int> CreateAsync(ShipmentRequest model);
    Task<bool> UpdateAsync(ShipmentUpdateRequest model);
    Task<bool> DeleteAsync(int id);
}

public class ShipmentService : IShipmentService
{
    private readonly IMapper _mapper;

    private readonly IShipmentRepository _shipmentRepository;

    private readonly IBagRepository _bagRepository;


    public ShipmentService(IMapper mapper, IShipmentRepository shipmentRepository, IBagRepository bagRepository)
    {
        _mapper = mapper;
        _shipmentRepository = shipmentRepository;
        _bagRepository = bagRepository;
    }

    public async Task<IEnumerable<ShipmentResponse>> GetAllAsync()
    {
        var shipments = await _shipmentRepository.GetAllAsync();

        var responses = new List<ShipmentResponse>();

        foreach (var shipment in shipments)
        {
            responses.Add(_mapper.Map<ShipmentResponse>(shipment));
        }
        return responses;
    }

    public async Task<ShipmentResponse> GetByIdAsync(int id)
    {
        var shipment = await _shipmentRepository.GetByIdAsync(id);
        if (shipment == null) throw new KeyNotFoundException("Shipment not found");

        var shipmentModel = _mapper.Map<ShipmentResponse>(shipment);
        return shipmentModel;
    }

    public async Task<int> CreateAsync(ShipmentRequest model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            // map model to new Shipment object
            model.FlightDate = Convert.ToDateTime(model.FlightDate);
            var shipment = _mapper.Map<Shipment>(model);

            // save Shipment
            var shipmentId = await _shipmentRepository.CreateAsync(shipment);

            if (shipmentId > 0 && model.BagIds != null && model.BagIds.Count > 0)
            {
                foreach (var bagId in model.BagIds)
                {
                    var bag = await _bagRepository.GetByIdAsync(bagId);
                    bag.ShipmentId = shipmentId;
                    await _bagRepository.UpdateAsync(bag);
                }
            }

            transaction.Complete();
            return shipmentId;
        }
        catch (Exception)
        {
            transaction.Dispose();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(ShipmentUpdateRequest model)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        try
        {
            var isValid = await IsValid(model);
            if (!isValid)
                return false;
            // copy model to Shipment and update
            model.FlightDate = Convert.ToDateTime(model.FlightDate);
            var shipment = _mapper.Map<Shipment>(model);
            var result = await _shipmentRepository.UpdateAsync(shipment);

            if (result && model.BagIds != null && model.BagIds.Count > 0)
            {
                var existingBags = await _bagRepository.GetAllByShipmentIdAsync(model.ShipmentId);

                var bags = await _bagRepository.GetAllAsync(model.BagIds);

                var existing = existingBags.ToList();

                var newBags = bags.ToList();
                var bagsToAdd = newBags.Except(existing).ToList();
                var bagsToRemove = existing.Except(newBags).ToList();

                foreach (var bag in bagsToAdd)
                {
                    bag.ShipmentId = model.ShipmentId;
                    result = await _bagRepository.UpdateAsync(bag);
                }

                foreach (var bag in bagsToRemove)
                {
                    bag.ShipmentId = null;
                    result = await _bagRepository.UpdateAsync(bag);
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
            var shipment = await _shipmentRepository.GetByIdAsync(id);

            if (shipment != null && shipment.Status == Status.Finalized)
                return false;

            var bags = await _bagRepository.GetAllByShipmentIdAsync(id);

            foreach (var bag in bags)
            {
                bag.ShipmentId = null;

                await _bagRepository.UpdateAsync(bag);
            }

            transaction.Complete();
            return await _shipmentRepository.DeleteAsync(id);
        }
        catch (Exception)
        {
            transaction.Dispose();
            throw;
        }
    }

    public async Task<bool> IsValid(ShipmentUpdateRequest model)
    {
        if (model.ShipmentId > 0)
        {
            var shipment = await _shipmentRepository.GetByIdAsync(model.ShipmentId);

            if (shipment != null && shipment.Status == Status.Finalized)
                return false;

            if (model.Status == Common.Status.Finalized && (model.BagIds == null || model.BagIds != null && model.BagIds.Count == 0))
                return false;
        }

        return true;
    }

}