namespace PostOffice.Tests;

public class ParcelServiceTests
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IParcelRepository> _parcelRepository;
    private readonly IParcelService _parcelService;

    public ParcelServiceTests()
    {
        _mapper = new Mock<IMapper>();
        _parcelRepository = new Mock<IParcelRepository>();

        _parcelService = new ParcelService(_mapper.Object, _parcelRepository.Object);
    }

    [SetUp]
    public void Setup()
    {
        _mapper.Reset();
        _parcelRepository.Reset();
    }

    [Test]
    public async Task CreateAsync()
    {
        //Arrange
        var command = SetUpParcelRequest();
        SetUpMapper(command);
        var parcelId = 1;
        _parcelRepository.Setup(x => x.CreateAsync(It.IsAny<Parcel>())).ReturnsAsync(parcelId);

        //Act
        var result = await _parcelService.CreateAsync(command);

        //Arrange
        _parcelRepository.Verify(x => x.CreateAsync(It.IsAny<Parcel>()), Times.Once);
        _parcelRepository.VerifyNoOtherCalls();
        Assert.AreEqual(parcelId, result);
    }

    [Test]
    public async Task UpdateAsync()
    {
        //Arrange
        var command = SetUpParcelUpdateRequest();
        SetUpMapper(command);
        _parcelRepository.Setup(x => x.UpdateAsync(It.IsAny<Parcel>())).ReturnsAsync(true);

        //Act
        await _parcelService.UpdateAsync(command);

        //Arrange
        _parcelRepository.Verify(x => x.UpdateAsync(It.IsAny<Parcel>()), Times.Once);
    }

    [Test]
    public async Task GetAllAsync()
    {
        //Arrange
        _parcelRepository.Setup(x => x.GetAllAsync(null)).Returns(Task.FromResult(GetParcels()));

        //Act
        var response = await _parcelService.GetAllAsync();

        //Arrange
        Assert.Greater(response.ToList().Count, 0, "No parcels were found");
        _parcelRepository.Verify(x => x.GetAllAsync(null), Times.Once);
        _parcelRepository.VerifyNoOtherCalls();
    }

    private static ParcelRequest SetUpParcelRequest()
    {
        return new ParcelRequest()
        {
            ParcelNumber = "PL1001",
            Price = 30.00m,
            Weight = 207m,
            RecipientName = "Test 1",
            DestinationCountry = "EE",
        };
    }
    private static ParcelRequest SetUpParcelUpdateRequest()
    {
        return new ParcelRequest()
        {
            ParcelId = 1,
            ParcelNumber = "PL1001",
            Price = 30.00m,
            Weight = 207m,
            RecipientName = "Test 1",
            DestinationCountry = "EE",
        };
    }

    private void SetUpMapper(ParcelRequest command)
    {
        _mapper.Setup(x => x.Map<Parcel>(command)).Returns
        (
            new Parcel
            {
                Price = command.Price,
                Weight = command.Weight,
                RecipientName = command.RecipientName,
                DestinationCountry = command.DestinationCountry,
                ParcelNumber = command.ParcelNumber
            }
        );
    }
    private static IEnumerable<Parcel> GetParcels()
    {
        return new List<Parcel>
        {
            new()
            {
                ParcelNumber = "PL1001",
                Price = 30.00m,
                Weight = 207m,
                RecipientName = "Test 1",
                DestinationCountry = "EE",
                ParcelId = 101
            },

            new()
            {
                ParcelNumber = "PL1002",
                Price = 33.00m,
                Weight = 209m,
                RecipientName = "Test 2",
                DestinationCountry = "FI",
                ParcelId = 102
            }
        };
    }
}