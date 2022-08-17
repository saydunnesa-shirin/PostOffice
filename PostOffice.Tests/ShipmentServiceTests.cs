using Airport = PostOffice.Common.Airport;

namespace PostOffice.Tests;

public class ShipmentServiceTests
{
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IShipmentRepository> _shipmentRepository;
    private readonly IShipmentService _shipmentService;

    public ShipmentServiceTests()
    {
        _shipmentRepository = new Mock<IShipmentRepository>();
        _mapper = new Mock<IMapper>();

        _shipmentService = new ShipmentService(_shipmentRepository.Object, _mapper.Object);
    }

    [SetUp]
    public void Setup()
    {
        _mapper.Reset();
        _shipmentRepository.Reset();
    }

    [Test]
    public async Task CreateAsync()
    {
        //Arrange
        var command = SetUpShipmentRequest();
        SetUpMapper(command);
        _shipmentRepository.Setup(x => x.CreateAsync(It.IsAny<Shipment>())).Returns(Task.FromResult(default(object)));
            
        //Act
        await _shipmentService.CreateAsync(command);

        //Arrange
        _shipmentRepository.Verify(x => x.CreateAsync(It.IsAny<Shipment>()), Times.Once);
        _shipmentRepository.VerifyNoOtherCalls();
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
            ShipmentNumber = "SN123"
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