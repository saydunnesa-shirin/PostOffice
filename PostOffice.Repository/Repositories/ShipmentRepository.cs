namespace PostOffice.Repository.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private DataContext _context;
    public ShipmentRepository(DataContext context)
    {
        _context = context;
    }
    public async Task<int> CreateAsync(Shipment shipment)
    {
        try
        {
            _context.Shipments.Add(shipment);
            await _context.SaveChangesAsync();
            return shipment.ShipmentId;
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
            var shipment = await GetByIdAsync(id);

            if(shipment == null)
                return false;
            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Shipment>> GetAllAsync()
    {
        try
        {
            var shipments = _context.Shipments
            .Include(x => x.Bags);
            return await shipments.ToListAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Shipment> GetByIdAsync(int id)
    {
        try
        {
            var shipment = await _context.Shipments
                .Where(x => x.ShipmentId == id)
                .Include(x => x.Bags)
                .SingleOrDefaultAsync();

            if (shipment == null) throw new KeyNotFoundException("Shipment not found");
            return shipment;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(Shipment shipment)
    {
        try
        {
            var entry = _context.Shipments.First(e => e.ShipmentId == shipment.ShipmentId);
            _context.Entry(entry).CurrentValues.SetValues(shipment);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            throw;
        }
    }
}
