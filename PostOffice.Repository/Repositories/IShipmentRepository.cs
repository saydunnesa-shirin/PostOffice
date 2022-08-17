namespace PostOffice.Repository.Repositories;

public interface IShipmentRepository
{
    Task<IEnumerable<Shipment>> GetAllAsync();
    Task<Shipment> GetByIdAsync(int id);
    Task CreateAsync(Shipment model);
    Task UpdateAsync(Shipment model);
    Task DeleteAsync(int id);
}
