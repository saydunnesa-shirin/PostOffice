namespace PostOffice.Repository.Repositories;

public class ParcelRepository : IParcelRepository
{
    private DataContext _context;
    public ParcelRepository(DataContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Parcel parcel)
    {
        _context.Parcels.Add(parcel);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var parcel = await GetByIdAsync(id);

        _context.Parcels.Remove(parcel);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Parcel>> GetAllAsync()
    {
        var parcels = _context.Parcels;
        return await parcels.ToListAsync();
    }

    public async Task<IEnumerable<Parcel>> GetAllByBagIdAsync(int bagId)
    {
        var parcels = _context.Parcels.Where(q=>q.BagId == bagId);
        return await parcels.ToListAsync();
    }

    public async Task<Parcel> GetByIdAsync(int id)
    {
        var parcel = await _context.Parcels.FindAsync(id);
        if (parcel == null) throw new KeyNotFoundException("Parcel not found");
        return parcel;
    }

    public async Task UpdateAsync(Parcel parcel)
    {
        _context.Parcels.Update(parcel);
        await _context.SaveChangesAsync();
    }
}
