using Airport = PostOffice.Common.Airport;
using Status = PostOffice.Common.Status;

namespace PostOffice.Tests;

public class ShipmentServiceTests
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IShipmentRepository> _shipmentRepository;
    private readonly Mock<IBagRepository>  _bagRepository;
    private readonly IShipmentService _shipmentService;

    public ShipmentServiceTests()
    {
        _mapper = new Mock<IMapper>();
        _shipmentRepository = new Mock<IShipmentRepository>();
        _bagRepository = new Mock<IBagRepository>();

        _shipmentService = new ShipmentService(_mapper.Object, _shipmentRepository.Object, _bagRepository.Object);
    }

    [SetUp]
    public void Setup()
    {
        _mapper.Reset();
        _shipmentRepository.Reset();
        _bagRepository.Reset();
    }

    [Test]
    public async Task CreateAsync()
    {
        //Arrange
        var command = SetUpShipmentRequest();
        SetUpMapper(command);
        var shipmentId = 1;
        _shipmentRepository.Setup(x => x.CreateAsync(It.IsAny<Shipment>())).ReturnsAsync(shipmentId);

        //Act
        var result = await _shipmentService.CreateAsync(command);

        //Arrange
        _shipmentRepository.Verify(x => x.CreateAsync(It.IsAny<Shipment>()), Times.Once);
        _shipmentRepository.VerifyNoOtherCalls();
        Assert.AreEqual(shipmentId, result);
    }

    [Test]
    public async Task UpdateAsync()
    {
        //Arrange
        var command = SetUpShipmentUpdateRequest();
        SetUpMapper(command);
        _shipmentRepository.Setup(x => x.UpdateAsync(It.IsAny<Shipment>())).ReturnsAsync(true);

        //Act
        await _shipmentService.UpdateAsync(command);

        //Arrange
        _shipmentRepository.Verify(x => x.UpdateAsync(It.IsAny<Shipment>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_StatusFinalize_Pass()
    {
        //Arrange
        var command = SetUpShipmentUpdateRequest();
        SetUpMapper(command);
        command.Status = Status.Finalized;
        command.BagIds = new List<int> { 1, 2 };
        _shipmentRepository.Setup(x => x.UpdateAsync(It.IsAny<Shipment>())).ReturnsAsync(true);

        //Act
        await _shipmentService.UpdateAsync(command);

        //Arrange
        _shipmentRepository.Verify(x => x.UpdateAsync(It.IsAny<Shipment>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_StatusFinalizeWithEmptyBags_Fail()
    {
        //Arrange
        var command = SetUpShipmentUpdateRequest();
        SetUpMapper(command);
        command.Status = Status.Finalized;
        _shipmentRepository.Setup(x => x.UpdateAsync(It.IsAny<Shipment>())).ReturnsAsync(true);

        //Act
        await _shipmentService.UpdateAsync(command);

        //Arrange
        _shipmentRepository.Verify(x => x.UpdateAsync(It.IsAny<Shipment>()), Times.Never);
    }

    [Test]
    public async Task GetAllAsync()
    {
        //Arrange
        _shipmentRepository.Setup(x => x.GetAllAsync()).Returns(Task.FromResult(GetShipments()));

        //Act
        var response = await _shipmentService.GetAllAsync();

        //Arrange
        Assert.Greater(response.ToList().Count, 0, "No shipments were found");
        _shipmentRepository.Verify(x => x.GetAllAsync(), Times.Once);
        _shipmentRepository.VerifyNoOtherCalls();
    }

    private ShipmentRequest SetUpShipmentRequest()
    {
        return new ShipmentRequest()
        {
            Airport = Airport.Tll,
            FlightDate = DateTime.UtcNow.AddDays(-10),
            FlightNumber = "FN123",
            ShipmentNumber = "SN123",
            Status = Status.Initial
        };
    }
    private ShipmentUpdateRequest SetUpShipmentUpdateRequest()
    {
        return new ShipmentUpdateRequest()
        {
            ShipmentId = 1,
            Airport = Airport.Tll,
            FlightDate = DateTime.UtcNow.AddDays(-10),
            FlightNumber = "FN123",
            ShipmentNumber = "SN123",
            Status = Status.Initial
        };
    }

    private void SetUpMapper(ShipmentRequest command)
    {
        _mapper.Setup(x => x.Map<Shipment>(command)).Returns
        (
            new Shipment
            {
                Airport = (Repository.Entities.Airport)command.Airport,
                FlightDate = command.FlightDate,
                FlightNumber = command.FlightNumber,
                ShipmentNumber = command.ShipmentNumber
                
            }
        );
    }
    private IEnumerable<Shipment> GetShipments()
    {
        return new List<Shipment>
        {
            new()
            {
                ShipmentNumber = "SN1001",
                Airport = Repository.Entities.Airport.Rix,
                FlightNumber = "FN101",
                FlightDate = DateTime.UtcNow.AddDays(-12),
                Bags = new List<Bag>
                {
                    new()
                    {
                        BagId = 10001,
                        BagNumber = "BN10001",
                        ContentType = ContentType.Parcel,
                        ItemCount = 25,
                        Parcels = new List<Parcel>
                        {

                        },
                        Price = 200.00m,
                        Weight = 5.07m
                    },
                    new()
                    {
                        BagId = 10002,
                        BagNumber = "BN10002",
                        ContentType = ContentType.Parcel,
                        ItemCount = 15,
                        Parcels = new List<Parcel>
                        {

                        },
                        Price = 100.00m,
                        Weight = 3.07m
                    }
                },
                ShipmentId = 101
            },

            new()
            {
                ShipmentNumber = "SN1002",
                Airport = Repository.Entities.Airport.Tll,
                FlightNumber = "FN102",
                FlightDate = DateTime.UtcNow.AddDays(-11),
                Bags = new List<Bag>
                {
                    new()
                    {
                        BagId = 10101,
                        BagNumber = "BN10101",
                        ContentType = ContentType.Letter,
                        ItemCount = 45,
                        Price = 30.00m,
                        Weight = 207m
                    },
                    new()
                    {
                        BagId = 10102,
                        BagNumber = "BN10102",
                        ContentType = ContentType.Letter,
                        ItemCount = 55,
                        Price = 30.00m,
                        Weight = 3.07m
                    },
                    new()
                    {
                        BagId = 10103,
                        BagNumber = "BN10103",
                        ContentType = ContentType.Letter,
                        ItemCount = 50,
                        Price = 32.00m,
                        Weight = 3.25m
                    }
                },
                ShipmentId = 101
            }
        };
    }
}