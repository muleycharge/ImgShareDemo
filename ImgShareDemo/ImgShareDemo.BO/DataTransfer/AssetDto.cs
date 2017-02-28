namespace ImgShareDemo.BO.DataTransfer
{
    public class AssetDto
    {
        public int? Id { get; set; }        
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceUrl { get; set; }
    }
}
