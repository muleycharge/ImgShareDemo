namespace ImgShareDemo.BO.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("Tag")]
    public class Tag : IEntity
    {
        [Column("TGT_Id")]
        public int Id { get; set; }

        [Column("TGT_TagValue")]
        public string TagValue { get; set; }

        [Column("TGT_USR_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
}
}
