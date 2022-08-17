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

    public ShipmentService(IShipmentRepository shipmentRepository,
        IMapper mapper)
    {
        _shipmentRepository = shipmentRepository;
        _mapper = mapper;
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
        // map model to new Shipment object
        model.FlightDate = Convert.ToDateTime(model.FlightDate);
        var shipment = _mapper.Map<Shipment>(model);

        // save Shipment
        await _shipmentRepository.CreateAsync(shipment);
    }

    public async Task UpdateAsync(ShipmentUpdateRequest model)
    {
        // copy model to Shipment and update
        var shipment = _mapper.Map<Shipment>(model);
        model.FlightDate = Convert.ToDateTime(model.FlightDate);
        await _shipmentRepository.UpdateAsync(shipment);
    }

    public async Task DeleteAsync(int id)
    {
        await _shipmentRepository.DeleteAsync(id);
    }

}