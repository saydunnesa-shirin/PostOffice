namespace PostOffice.Repository.Repositories;

public class ShipmentRepository : IShipmentRepository
{
    private DataContext _context;
    public ShipmentRepository(DataContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(Shipment shipment)
    {
        _context.Shipments.Add(shipment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var shipment = await GetByIdAsync(id);

        _context.Shipments.Remove(shipment);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Shipment>> GetAllAsync()
    {
        var shipments = _context.Shipments;
        return await shipments.ToListAsync();
    }

    public async Task<Shipment> GetByIdAsync(int id)
    {
        var shipment = await _context.Shipments.FindAsync(id);
        if (shipment == null) throw new KeyNotFoundException("Shipment not found");
        return shipment;
    }

    public async Task UpdateAsync(Shipment shipment)
    {
        _context.Shipments.Update(shipment);
        await _context.SaveChangesAsync();
    }
}
