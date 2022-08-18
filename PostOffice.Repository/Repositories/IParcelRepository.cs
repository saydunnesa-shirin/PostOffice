namespace PostOffice.Repository.Repositories;

public interface IParcelRepository
{
    Task<IEnumerable<Parcel>> GetAllAsync();
    Task<Parcel> GetByIdAsync(int id);
    Task<IEnumerable<Parcel>> GetAllByBagIdAsync(int bagId);
    Task CreateAsync(Parcel model);
    Task UpdateAsync(Parcel model);
    Task DeleteAsync(int id);
}
