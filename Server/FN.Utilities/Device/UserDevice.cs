using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FN.Utilities.Device
{
    public class UserDevice
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        public int UserId { get; set; }
        public List<DeviceInfoDetail> Devices { get; set; } = new List<DeviceInfoDetail>();
    }
}
