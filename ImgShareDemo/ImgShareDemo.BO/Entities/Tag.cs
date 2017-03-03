namespace ImgShareDemo.BO.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Tag")]
    public class Tag : IEntity
    {
        [Column("TGT_Id")]
        public int Id { get; set; }

        [Column("TGT_TagValue")]
        [MaxLength(64)]
        [Index("idxUniqueUserTag", IsUnique = true, Order = 2)]
        public string TagValue { get; set; }

        [Column("TGT_USR_Id")]
        [ForeignKey("User")]
        [Index("idxUniqueUserTag", IsUnique = true, Order = 1)]
        public int UserId { get; set; }

        public virtual User User { get; set; }
}
}
