namespace ImgShareDemo.BO.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AS_Asset")]
    public class Asset : IEntity
    {
        [Column("ASA_Id")]
        [Key]
        public int Id { get; set; }

        [Column("ASA_USR_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("ASA_Name")]
        public string Name { get; set; }

        [Column("ASA_Description")]
        public string Description { get; set; }

        [Column("ASA_SourceUrl")]
        public string SourceUrl { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<AssetTag> AssetTags { get; set; }
    }
}
