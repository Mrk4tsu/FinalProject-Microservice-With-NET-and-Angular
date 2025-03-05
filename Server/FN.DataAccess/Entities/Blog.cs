namespace FN.DataAccess.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string Detail { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
    }
}
