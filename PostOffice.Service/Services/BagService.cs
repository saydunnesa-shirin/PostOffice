namespace PostOffice.Service.Services;
public interface IBagService
{
    Task<IEnumerable<BagResponse>> GetAllAsync();
    Task<BagResponse> GetByIdAsync(int id);
    Task CreateAsync(BagRequest model);
    Task UpdateAsync(BagUpdateRequest model);
    Task DeleteAsync(int id);
}

public class BagService : IBagService
{
    private readonly IMapper _mapper;

    private readonly IBagRepository _bagRepository;

    public BagService(IBagRepository bagRepository,
        IMapper mapper)
    {
        _bagRepository = bagRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BagResponse>> GetAllAsync()
    {
        var bags = await _bagRepository.GetAllAsync();

        List<BagResponse> responses = new List<BagResponse>();

        foreach (var bag in bags)
        {
            responses.Add(_mapper.Map<BagResponse>(bag));
        }
        return responses;
    }

    public async Task<BagResponse> GetByIdAsync(int id)
    {
        var bag = await _bagRepository.GetByIdAsync(id);
        if (bag == null) throw new KeyNotFoundException("Bag not found");

        var response = _mapper.Map<BagResponse>(bag);
        return response;
    }

    public async Task CreateAsync(BagRequest model)
    {
        // map model to new Bag object
        var bag = _mapper.Map<Bag>(model);

        // save Bag
        await _bagRepository.CreateAsync(bag);
    }

    public async Task UpdateAsync(BagUpdateRequest model)
    {
        // copy model to Bag and update
        var bag = _mapper.Map<Bag>(model);
        await _bagRepository.UpdateAsync(bag);
    }

    public async Task DeleteAsync(int id)
    {
        await _bagRepository.DeleteAsync(id);
    }
}