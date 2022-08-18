namespace PostOffice.Repository.Repositories;

public interface IBagRepository
{
    Task<IEnumerable<Bag>> GetAllAsync();
    Task<Bag> GetByIdAsync(int id);
    Task<IEnumerable<Bag>> GetAllByShipmentIdAsync(int shipmentId);
    Task<int> CreateAsync(Bag model);
    Task UpdateAsync(Bag model);
    Task DeleteAsync(int id);
}
