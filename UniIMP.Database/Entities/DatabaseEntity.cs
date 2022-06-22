using System.ComponentModel.DataAnnotations;

namespace UniIMP.DataAccess.Entities
{
    public class DatabaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}