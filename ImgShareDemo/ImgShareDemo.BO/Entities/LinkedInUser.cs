namespace ImgShareDemo.BO.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("US_LinkedInUser")]
    public class LinkedInUser : IEntity, ITrackable
    {
        [Column("USL_ID")]
        [Key]
        public int Id { get; set; }

        [Column("USL_USR_ID")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("USL_LinkedInId")]
        [MaxLength(64)]
        public string LinkedInId { get; set; }

        [Column("USL_Token")]
        public string Token { get; set; }

        [Column("USL_RefreshToken")]
        public string RefreshToken { get; set; }

        [Column("USL_TokenExpires")]
        public DateTime TokenExpires { get; set; }

        [Column("USL_ProfileRequestUrl")]
        public string ProfileRequestUrl { get; set; }

        [Column("USL_ProfileImageUrl")]
        public string ProfileImageUrl { get; set; }

        [Column("USL_DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("USL_DateModified")]
        public DateTime? DateModified { get; set; }

        public virtual User User { get; set; }

    }
}
