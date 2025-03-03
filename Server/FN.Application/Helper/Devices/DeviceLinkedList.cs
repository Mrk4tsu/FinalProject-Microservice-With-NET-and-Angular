using FN.Application.Systems.Token;
using FN.Utilities.Device;
using FN.ViewModel.Systems.Token;
using MongoDB.Driver;

namespace FN.Application.Helper.Devices
{
    public class DeviceLinkedList
    {
        private DeviceNode? _head;
        private int _size;
        private readonly int _maxSize;
        private int _userId;
        private readonly ITokenService _tokenService;
        public DeviceLinkedList(int maxSize, int userId, ITokenService tokenService)
        {
            _head = null;
            _size = 0;
            _maxSize = maxSize;
            _userId = userId;
            _tokenService = tokenService;
        }

        public async Task Add(DeviceInfoDetail deviceInfo)
        {
            var newNode = new DeviceNode(deviceInfo);

            if (_head == null)
            {
                _head = newNode;
            }
            else
            {
                newNode.Next = _head;
                _head = newNode;
            }

            _size++;

            // Nếu vượt quá số lượng thiết bị tối đa, xóa thiết bị cũ nhất
            if (_size > _maxSize)
            {
                await RemoveLast();
            }
        }

        private async Task RemoveLast()
        {
            string clientIdToRemove;
            if (_head == null) return;

            if (_head.Next == null)
            {
                clientIdToRemove = _head.DeviceInfo.ClientId;
                _head = null;
            }
            else
            {
                var current = _head;
                while (current.Next!.Next != null)
                {
                    current = current.Next;
                }
                
                clientIdToRemove = current.Next.DeviceInfo.ClientId;
                current.Next = null;
            }
            _size--;
            await _tokenService.RemoveRefreshToken(new TokenRequest
            {
                UserId = _userId,
                ClientId = clientIdToRemove
            });
        }
        public List<DeviceInfoDetail> ToList()
        {
            var devices = new List<DeviceInfoDetail>();
            var current = _head;
            while (current != null)
            {
                devices.Add(current.DeviceInfo);
                current = current.Next;
            }
            return devices;
        }
    }
}
