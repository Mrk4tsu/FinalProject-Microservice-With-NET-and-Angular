using FN.ViewModel.Helper.Device;
using UAParser;

namespace FN.Utilities
{
    public class Commons
    {
        public static DeviceInfo ParseUserAgent(string userAgent)
        {
            var parser = Parser.GetDefault();
            var clientInfo = parser.Parse(userAgent);

            return new DeviceInfo
            {
                Browser = $"{clientInfo.UA.Family} {clientInfo.UA.Major}",
                OS = $"{clientInfo.OS.Family} {clientInfo.OS.Major}",
                DeviceType = clientInfo.Device.Family
            };
        }
    }
}
