namespace ImgShareDemo.BO.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("AS_AssetTag")]
    public class AssetTag
    {
        [Column("AST_Id")]
        [Key]
        public int Id { get; set; }

        [Column("AST_ASA_Id")]
        [ForeignKey("Asset")]
        public int ASA_Id { get; set; }

        [Column("AST_TGT_Id")]
        [ForeignKey("Tag")]
        public int TGT_Id { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual Asset Asset { get; set; }
    }
}
