using FN.Utilities.Device;

namespace FN.Application.Helper.Devices
{
    public class DeviceNode
    {
        public DeviceInfoDetail DeviceInfo { get; set; }
        public DeviceNode? Next { get; set; }

        public DeviceNode(DeviceInfoDetail deviceInfo)
        {
            DeviceInfo = deviceInfo;
            Next = null;
        }
    }
}
