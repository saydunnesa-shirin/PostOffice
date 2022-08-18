using PostOffice.Common.Requests;
using PostOffice.Repository.Entities;

namespace PostOffice.Service.Services;
public interface IShipmentService
{
    Task<IEnumerable<ShipmentResponse>> GetAllAsync();
    Task<ShipmentResponse> GetByIdAsync(int id);
    Task CreateAsync(ShipmentRequest model);
    Task UpdateAsync(ShipmentUpdateRequest model);
    Task DeleteAsync(int id);
}

public class ShipmentService : IShipmentService
{
    private readonly IMapper _mapper;

    private readonly IShipmentRepository _shipmentRepository;

    private readonly IBagRepository _bagRepository;

    private readonly IParcelRepository _parcelRepository;

    public ShipmentService(IMapper mapper, IShipmentRepository shipmentRepository, IBagRepository bagRepository, IParcelRepository parcelRepository)
    {
        _mapper = mapper;
        _shipmentRepository = shipmentRepository;
        _bagRepository = bagRepository;
        _parcelRepository = parcelRepository;
    }

    public async Task<IEnumerable<ShipmentResponse>> GetAllAsync()
    {
        var shipments = await _shipmentRepository.GetAllAsync();

        List<ShipmentResponse> responses = new List<ShipmentResponse>();

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

    public async Task CreateAsync(ShipmentRequest model)
    {
        if (model == null) throw new ArgumentNullException("model");

        // map model to new Shipment object
        model.FlightDate = Convert.ToDateTime(model.FlightDate);
        var shipment = _mapper.Map<Shipment>(model);

        // save Shipment
        var shipmentId = await _shipmentRepository.CreateAsync(shipment);

        if (shipmentId > 0 && model.Bags != null && model.Bags.Count > 0)
        {
            foreach (var bagRequest in model.Bags)
            {
                var bag = _mapper.Map<Bag>(bagRequest);

                bag.ShipmentId = shipmentId;
                var bagId = await _bagRepository.CreateAsync(bag);
            }
        }
    }

    public async Task UpdateAsync(ShipmentUpdateRequest model)
    {
        if (model == null) throw new ArgumentNullException("model");

        if (!IsValid(model.ShipmentId))
            return;

        // copy model to Shipment and update
        var shipment = _mapper.Map<Shipment>(model);
        model.FlightDate = Convert.ToDateTime(model.FlightDate);
        await _shipmentRepository.UpdateAsync(shipment);

        if (model.Bags != null && model.Bags.Count > 0)
        {
            var bags = await _bagRepository.GetAllByShipmentIdAsync(model.ShipmentId);

            var newBags = model.Bags.Where(q => q.BagId == 0).ToList();

            foreach (var bagRequest in newBags)
            {
                var bag = _mapper.Map<Bag>(bagRequest);
                bag.ShipmentId = model.ShipmentId;
                await _bagRepository.CreateAsync(bag);
            }

            var excludeBags = bags.Where(q => model.Bags.Any(p => p.BagId > 0 && p.BagId != q.BagId)).ToList();

            foreach (var bag in excludeBags)
            {
                bag.ShipmentId = null;
                await _bagRepository.UpdateAsync(bag);
            }
        }
    }

    public async Task DeleteAsync(int id)
    {
        if (!IsValid(id))
            return;

        var bags = await _bagRepository.GetAllByShipmentIdAsync(id);

        foreach (var bag in bags)
        {
            var parcels = await _parcelRepository.GetAllByBagIdAsync(bag.BagId);

            foreach (var parcel in parcels)
            {
                await _parcelRepository.DeleteAsync(parcel.ParcelId);
            }

            await _bagRepository.DeleteAsync(bag.BagId);
        }

        await _shipmentRepository.DeleteAsync(id);
    }

    public bool IsValid(int shipmentId)
    {
        if (shipmentId > 0)
        {
            var shipment = _shipmentRepository.GetByIdAsync(shipmentId);

            if (shipment != null && (Status)shipment.Status == Status.Finalized)
                return false;
        }

        return true;
    }

}