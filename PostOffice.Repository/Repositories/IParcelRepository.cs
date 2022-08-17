namespace PostOffice.Repository.Repositories;

public interface IParcelRepository
{
    Task<IEnumerable<Parcel>> GetAllAsync();
    Task<Parcel> GetByIdAsync(int id);
    Task CreateAsync(Parcel model);
    Task UpdateAsync(Parcel model);
    Task Delete(int id);
}
