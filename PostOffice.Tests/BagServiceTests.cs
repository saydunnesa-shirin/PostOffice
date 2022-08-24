using ContentType = PostOffice.Common.ContentType;

namespace PostOffice.Tests;

public class BagServiceTests
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IShipmentRepository> _shipmentRepository;
    private readonly Mock<IBagRepository>  _bagRepository;
    private readonly Mock<IParcelRepository> _parcelRepository;
    private readonly IBagService _shipmentService;

    public BagServiceTests()
    {
        _mapper = new Mock<IMapper>();
        _shipmentRepository = new Mock<IShipmentRepository>();
        _bagRepository = new Mock<IBagRepository>();
        _parcelRepository = new Mock<IParcelRepository>();

        _shipmentService = new BagService(_mapper.Object, _bagRepository.Object, _parcelRepository.Object, _shipmentRepository.Object);
    }

    [SetUp]
    public void Setup()
    {
        _mapper.Reset();
        _shipmentRepository.Reset();
        _bagRepository.Reset();
        _parcelRepository.Reset();
    }

    [Test]
    public async Task CreateAsync()
    {
        //Arrange
        var command = SetUpBagRequest();
        SetUpMapper(command);
        var id = 1;
        _bagRepository.Setup(x => x.CreateAsync(It.IsAny<Bag>())).ReturnsAsync(id);

        //Act
        var result = await _shipmentService.CreateAsync(command);

        //Arrange
        _bagRepository.Verify(x => x.CreateAsync(It.IsAny<Bag>()), Times.Once);
        _bagRepository.VerifyNoOtherCalls();
        Assert.AreEqual(id, result);
    }

    [Test]
    public async Task UpdateAsync()
    {
        //Arrange
        var command = SetUpBagUpdateRequest();
        SetUpMapper(command);
        _bagRepository.Setup(x => x.UpdateAsync(It.IsAny<Bag>())).ReturnsAsync(true);

        //Act
        await _shipmentService.UpdateAsync(command);

        //Arrange
        _bagRepository.Verify(x => x.UpdateAsync(It.IsAny<Bag>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ContentTypeLetter_Fail()
    {
        //Arrange
        var command = SetUpBagUpdateRequest();
        command.ContentType = ContentType.Letter;
        command.ParcelIds = new List<int> {1,2};
        SetUpMapper(command);
        _bagRepository.Setup(x => x.UpdateAsync(It.IsAny<Bag>())).ReturnsAsync(false);

        //Act
        var result = await _shipmentService.UpdateAsync(command);

        //Arrange
        _bagRepository.Verify(x => x.UpdateAsync(It.IsAny<Bag>()), Times.Never);
        Assert.AreEqual(false, result);
    }

    [Test]
    public async Task UpdateAsync_ContentTypeParcel_Fail()
    {
        //Arrange
        var command = SetUpBagUpdateRequest();
        command.ContentType = ContentType.Parcel;
        SetUpMapper(command);
        _bagRepository.Setup(x => x.UpdateAsync(It.IsAny<Bag>())).ReturnsAsync(false);

        //Act
        var result = await _shipmentService.UpdateAsync(command);

        //Arrange
        _bagRepository.Verify(x => x.UpdateAsync(It.IsAny<Bag>()), Times.Never);
        Assert.AreEqual(false, result);
    }

    [Test]
    public async Task GetAllAsync()
    {
        //Arrange
        _bagRepository.Setup(x => x.GetAllAsync(null)).Returns(Task.FromResult(GetBags()));

        //Act
        var response = await _shipmentService.GetAllAsync();

        //Arrange
        Assert.Greater(response.ToList().Count, 0, "No bags were found");
        _bagRepository.Verify(x => x.GetAllAsync(null), Times.Once);
        _bagRepository.VerifyNoOtherCalls();
    }

    private static BagRequest SetUpBagRequest()
    {
        return new BagRequest()
        {
            ContentType = ContentType.Letter,
            Weight = 1.5m,
            Price = 2.5m,
            BagNumber = "BA123",
            ItemCount = 10,
            ShipmentId = 1
        };
    }
    private static BagRequest SetUpBagUpdateRequest()
    {
        return new BagRequest()
        {
            ContentType = ContentType.Letter,
            Weight = 1.5m,
            Price = 2.5m,
            BagNumber = "BA123",
            ItemCount = 10,
            ShipmentId = 1
        };
    }

    private void SetUpMapper(BagRequest command)
    {
        _mapper.Setup(x => x.Map<Bag>(command)).Returns
        (
            new Bag
            {
                ContentType = (Repository.Entities.ContentType)command.ContentType,
                Weight = command.Weight,
                Price = command.Price,
                BagNumber = command.BagNumber,
                ItemCount = command.ItemCount,
                ShipmentId = command.ShipmentId
            }
        );
    }
    private static IEnumerable<Bag> GetBags()
    {
        return new List<Bag>
        {
            new()
            {
                ContentType = (Repository.Entities.ContentType)ContentType.Parcel,
                BagNumber = "BP0123",
                ShipmentId = 1,
                Parcels = new List<Parcel>
                {
                    new()
                    {
                        ParcelId = 10001,
                        ParcelNumber = "BN10001",
                        RecipientName = "Test",
                        Price = 200.00m,
                        Weight = 5.07m
                    },
                    new()
                    {
                        ParcelId = 10002,
                        ParcelNumber = "BN10002",
                        RecipientName = "Test 2",
                        Price = 100.00m,
                        Weight = 3.07m
                    }
                },
                BagId = 101
            },

            new()
            {
                ContentType = (Repository.Entities.ContentType)ContentType.Letter,
                Weight = 1.5m,
                Price = 2.5m,
                BagNumber = "BL0623",
                ItemCount = 10,
                ShipmentId = 1,
                BagId = 102
            }
        };
    }
}