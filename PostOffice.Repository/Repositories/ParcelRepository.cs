namespace PostOffice.Repository.Repositories;

public class ParcelRepository : IParcelRepository
{
    private readonly DataContext _context;
    public ParcelRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(Parcel parcel)
    {
        _context.Parcels.Add(parcel);
        await _context.SaveChangesAsync();
        return parcel.ParcelId;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var parcel = await GetByIdAsync(id);

        _context.Parcels.Remove(parcel);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Parcel>> GetAllAsync(List<int>? ids)
    {
        IQueryable<Parcel> parcels;

        if (ids is { Count: > 0 })
        {
            parcels = _context.Parcels.Where(x => ids.Contains(x.ParcelId));
        }
        else
        {
            parcels = _context.Parcels;
        }

        return await parcels.ToListAsync();
    }

    public async Task<IEnumerable<Parcel>> GetAllByBagIdAsync(int bagId)
    {
        var parcels = _context.Parcels.Where(q => q.BagId == bagId);
        return await parcels.ToListAsync();
    }

    public async Task<Parcel> GetByIdAsync(int id)
    {
        var parcel = await _context.Parcels.FindAsync(id);
        if (parcel == null) throw new KeyNotFoundException("Parcel not found");
        return parcel;
    }

    public async Task<bool> UpdateAsync(Parcel parcel)
    {
        _context.Parcels.Update(parcel);
        await _context.SaveChangesAsync();
        return true;
    }
}
