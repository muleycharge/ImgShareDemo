
namespace ImgShareDemo.BO.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("US_UserSignOn")]
    public class UserSignOn : IEntity, ITrackable
    {
        [Column("USO_ID")]
        [Key]
        public int Id { get; set; }

        [Column("USO_State")]
        [Index("idxUniqueState")]
        [MaxLength(32)]
        public string State { get; set; }

        [Column("USO_USR_ID")]
        [ForeignKey("User")]
        public int? UserId { get; set; }

        [Column("USO_RedirectUrl")]
        [Required]
        public string RedirectUri { get; set; }

        [Column("USO_DateCreated")]
        public DateTime DateCreated { get; set; }

        [Column("USO_DateModified")]
        public DateTime? DateModified { get; set; }

        public virtual User User { get; set; }

    }
}
