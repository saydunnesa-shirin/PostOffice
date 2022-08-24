namespace PostOffice.Repository.Repositories;
public class BagRepository : IBagRepository
{
    private readonly DataContext _context;
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
        var bag = await GetByIdAsync(id);

        _context.Bags.Remove(bag);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Bag>> GetAllAsync(List<int>? ids = null)
    {
        if (ids is { Count: > 0 })
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

    public async Task<IEnumerable<Bag>> GetAllByShipmentIdAsync(int shipmentId)
    {
        var bags = _context.Bags.Where(q => q.ShipmentId == shipmentId);
        return await bags.ToListAsync();
    }

    public async Task<Bag> GetByIdAsync(int id)
    {
        var bag = await _context.Bags
            .Where(x => x.BagId == id)
            .Include(x => x.Parcels)
            .SingleOrDefaultAsync();

        if (bag == null) throw new KeyNotFoundException("Bag not found");
        return bag;
    }

    public async Task<bool> UpdateAsync(Bag bag)
    {
        var entry = _context.Bags.First(e => e.BagId == bag.BagId);
        _context.Entry(entry).CurrentValues.SetValues(bag);
        await _context.SaveChangesAsync();
        return true;
    }
}
