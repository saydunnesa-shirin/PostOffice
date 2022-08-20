namespace PostOffice.Repository.Repositories;

public interface IBagRepository
{
    Task<IEnumerable<Bag>> GetAllAsync(List<int> ids = null);
    Task<Bag> GetByIdAsync(int id);
    Task<IEnumerable<Bag>> GetAllByShipmentIdAsync(int shipmentId);
    Task<int> CreateAsync(Bag model);
    Task<bool> UpdateAsync(Bag model);
    Task<bool> DeleteAsync(int id);
}
