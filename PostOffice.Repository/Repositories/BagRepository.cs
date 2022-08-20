using Microsoft.EntityFrameworkCore;
using PostOffice.Repository.Entities;

namespace PostOffice.Repository.Repositories;
public class BagRepository : IBagRepository
{
    private DataContext _context;
    public BagRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(Bag bag)
    {
        _context.Bags.Add(bag);
        await _context.SaveChangesAsync();
        return bag.BagId;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var bag = await GetByIdAsync(id);

            if (bag == null)
                return false;

            _context.Bags.Remove(bag);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Bag>> GetAllAsync(List<int> ids = null)
    {
        try
        {
            if (ids != null && ids.Count > 0)
            {
                var bags = _context.Bags.Where(x => ids.Contains(x.BagId))
                  .Include(x => x.Parcels);

                return await bags.ToListAsync();
            }
            else
            {
                var bags = _context.Bags
                .Include(x => x.Parcels);

                return await bags.ToListAsync();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Bag>> GetAllByShipmentIdAsync(int shipmentId)
    {
        try
        {
            var bags = _context.Bags.Where(q => q.ShipmentId == shipmentId);
            return await bags.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Bag> GetByIdAsync(int id)
    {
        try
        {
            var bag = await _context.Bags
                .Where(x => x.BagId == id)
                .Include(x => x.Parcels)
                .SingleOrDefaultAsync();

            if (bag == null) throw new KeyNotFoundException("Bag not found");
            return bag;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Bag bag)
    {
        try
        {
            var entry = _context.Bags.First(e => e.BagId == bag.BagId);
            _context.Entry(entry).CurrentValues.SetValues(bag);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw;
        }
    }

    
}
