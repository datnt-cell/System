using System.Collections.Generic;

namespace GameOfferSystem.Domain
{
    /// <summary>
    /// Lưu toàn bộ runtime state của GameOfferGroups
    /// </summary>
    public class GameOfferGroupState
    {
        private readonly Dictionary<string, GameOfferGroupRuntimeData> _groups
            = new Dictionary<string, GameOfferGroupRuntimeData>();

        /// <summary>
        /// Danh sách group đang active
        /// </summary>
        public IReadOnlyCollection<GameOfferGroupRuntimeData> Values
        {
            get { return _groups.Values; }
        }

        /// <summary>
        /// Thêm hoặc cập nhật group runtime data
        /// </summary>
        public void Add(GameOfferGroupRuntimeData data)
        {
            _groups[data.GroupId] = data;
        }

        /// <summary>
        /// Kiểm tra group tồn tại
        /// </summary>
        public bool ContainsKey(string groupId)
        {
            return _groups.ContainsKey(groupId);
        }

        /// <summary>
        /// Lấy runtime data của group
        /// </summary>
        public GameOfferGroupRuntimeData Get(string groupId)
        {
            _groups.TryGetValue(groupId, out var data);
            return data;
        }

        /// <summary>
        /// TryGet pattern (an toàn hơn)
        /// </summary>
        public bool TryGetValue(string groupId, out GameOfferGroupRuntimeData data)
        {
            return _groups.TryGetValue(groupId, out data);
        }

        /// <summary>
        /// Indexer tiện dụng
        /// </summary>
        public GameOfferGroupRuntimeData this[string groupId]
        {
            get => _groups[groupId];
            set => _groups[groupId] = value;
        }

        /// <summary>
        /// Xóa group (ví dụ khi hết hạn)
        /// </summary>
        public void Remove(string groupId)
        {
            _groups.Remove(groupId);
        }
    }
}