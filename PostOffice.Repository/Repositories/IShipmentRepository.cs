namespace PostOffice.Repository.Repositories;

public interface IShipmentRepository
{
    Task<IEnumerable<Shipment>> GetAllAsync();
    Task<Shipment> GetByIdAsync(int id);
    Task<int> CreateAsync(Shipment model);
    Task<bool> UpdateAsync(Shipment model);
    Task<bool> DeleteAsync(int id);
}
