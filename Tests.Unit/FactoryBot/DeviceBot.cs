using DataAccess.Entities;

namespace Test.Unit.FactoryBot;

public class DeviceBot
{
    private Device _device;

    public DeviceBot()
    {
        Initialize();
    }

    private void Initialize()
    {
        _device = new Device
        {
            FirmwareVersion = "1.1.0",
            EncryptedSharedSecret = "randomEncryptedSecret",
        };
    }

    public Device GetObject() => _device;
}