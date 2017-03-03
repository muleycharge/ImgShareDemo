
namespace ImgShareDemo.BO.DataTransfer
{
    using System.Collections.Generic;

    public class AssetDto
    {
        public static readonly string[] RequiredIncludes = new[] { "AssetTags", "AssetTags.Tag" };

        public int? Id { get; set; }        
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceUrl { get; set; }
        public IEnumerable<TagDto> Tags { get; set; }
    }
}
