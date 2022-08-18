namespace PostOffice.Service.Services;
public interface IParcelService
{
    Task<IEnumerable<ParcelResponse>> GetAllAsync();
    Task<ParcelResponse> GetByIdAsync(int id);
    Task CreateAsync(ParcelRequest model);
    Task UpdateAsync(ParcelRequest model);
    Task DeleteAsync(int id);
}

public class ParcelService : IParcelService
{
    private readonly IMapper _mapper;

    private readonly IParcelRepository _parcelRepository;

    public ParcelService(IParcelRepository parcelRepository,
        IMapper mapper)
    {
        _parcelRepository = parcelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ParcelResponse>> GetAllAsync()
    {
        var parcels = await _parcelRepository.GetAllAsync();

        List<ParcelResponse> responses = new List<ParcelResponse>();

        foreach (var parcel in parcels)
        {
            responses.Add(_mapper.Map<ParcelResponse>(parcel));
        }
        return responses;
    }

    public async Task<ParcelResponse> GetByIdAsync(int id)
    {
        var parcel = await _parcelRepository.GetByIdAsync(id);
        if (parcel == null) throw new KeyNotFoundException("Parcel not found");

        var response = _mapper.Map<ParcelResponse>(parcel);
        return response;
    }

    public async Task CreateAsync(ParcelRequest model)
    {
        // map model to new Parcel object
        var parcel = _mapper.Map<Parcel>(model);

        // save Parcel
        await _parcelRepository.CreateAsync(parcel);
    }

    public async Task UpdateAsync(ParcelRequest model)
    {
        // copy model to Parcel and update
        var parcel = _mapper.Map<Parcel>(model);
        await _parcelRepository.UpdateAsync(parcel);
    }

    public async Task DeleteAsync(int id)
    {
        await _parcelRepository.DeleteAsync(id);
    }
}