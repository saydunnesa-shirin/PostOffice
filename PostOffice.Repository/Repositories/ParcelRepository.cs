namespace PostOffice.Repository.Repositories;

public class ParcelRepository : IParcelRepository
{
    private DataContext _context;
    public ParcelRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(Parcel parcel)
    {
        try
        {
            _context.Parcels.Add(parcel);
            await _context.SaveChangesAsync();
            return parcel.ParcelId;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var parcel = await GetByIdAsync(id);

            if(parcel == null)
                return false;

            _context.Parcels.Remove(parcel);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Parcel>> GetAllAsync(List<int> ids)
    {
        try
        {
            IQueryable<Parcel> parcels;

            if (ids != null && ids.Count > 0)
            {
                parcels = _context.Parcels.Where(x => ids.Contains(x.ParcelId));
            }
            else
            {
                parcels = _context.Parcels;
            }

            return await parcels.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Parcel>> GetAllByBagIdAsync(int bagId)
    {
        try
        {
            var parcels = _context.Parcels.Where(q => q.BagId == bagId);
            return await parcels.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Parcel> GetByIdAsync(int id)
    {
        try
        {
            var parcel = await _context.Parcels.FindAsync(id);
            if (parcel == null) throw new KeyNotFoundException("Parcel not found");
            return parcel;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Parcel parcel)
    {
        try
        {
            _context.Parcels.Update(parcel);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
