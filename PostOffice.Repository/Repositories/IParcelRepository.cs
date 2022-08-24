namespace PostOffice.Repository.Repositories;

public interface IParcelRepository
{
    Task<IEnumerable<Parcel>> GetAllAsync(List<int>? ids = null);
    Task<Parcel> GetByIdAsync(int id);
    Task<IEnumerable<Parcel>> GetAllByBagIdAsync(int bagId);
    Task<int> CreateAsync(Parcel model);
    Task<bool> UpdateAsync(Parcel model);
    Task<bool> DeleteAsync(int id);
}
