using Microsoft.EntityFrameworkCore;

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

    public async Task DeleteAsync(int id)
    {
        var bag = await GetByIdAsync(id);

        if(bag != null)
        {
            _context.Bags.Remove(bag);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Bag>> GetAllAsync()
    {
        var bags = _context.Bags
            .Include(x => x.Parcels);

        return await bags.ToListAsync();
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

    public async Task UpdateAsync(Bag bag)
    {
        //_context.Entry(bag).Property(x => x.BagId).IsModified = false;
        _context.Bags.Update(bag);
        await _context.SaveChangesAsync();
    }
}
