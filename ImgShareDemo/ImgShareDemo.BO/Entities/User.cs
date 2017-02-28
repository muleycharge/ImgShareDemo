namespace ImgShareDemo.BO.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("US_User")]
    public class User : IEntity, ITrackable
    {
        [Column(name:"USR_ID")]
        [Key]
        public int Id { get; set; }

        [Column("USR_Username")]
        [MaxLength(64)]
        [Index("idxUniqueUsername", IsUnique = true)]
        public string Username { get; set; }

        [Column("USR_FirstName")]
        public string FirstName { get; set; }

        [Column("USR_LastName")]
        public string LastName { get; set; }

        [Column("USR_Email")]
        public string Email { get; set; }

        [Column("USR_ImageUrl")]
        public string ImageUrl { get; set; }

        [Column("USR_DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("USR_DateModified")]
        public DateTime? DateModified { get; set; }
    }
}
