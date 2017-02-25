using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgShareDemo.BO.Entities
{
    [Table("US_User")]
    public class User : IEntity
    {
        [Column(name:"USR_ID")]
        [Key]
        public int Id { get; set; }

        [Column(name:"USR_UserName")]
        public string UserName { get; set; }
    }
}
