using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FN.ViewModel.Helper.Device
{
    public class DeviceInfo
    {
        public string Browser { get; set; } = string.Empty;
        public string OS { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
    }
    public class DeviceInfoDetail : DeviceInfo
    {
        public string ClientId { get; set; } = string.Empty;
        public DateTime LastLogin { get; set; }
        public string IPAddress { get; set; } = string.Empty;
    }
}
