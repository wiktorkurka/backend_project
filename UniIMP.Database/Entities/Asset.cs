using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniIMP.DataAccess.Entities

{
    public class Asset
    {
        [Key]
        public int Id { get; set; }

#pragma warning disable 8618

        [Required]
        public string Name { get; set; }

#pragma warning restore 8618

        public List<AssetTag> Tags { get; set; }

        [ForeignKey("Agent")]
        [JsonIgnore]
        public int? AgentId { get; set; }

        public virtual SnmpAgent? Agent { get; set; }

        [Column(TypeName = "jsonb")]
        public JObject Properties { get; set; }

        [Required]
        public DateTimeOffset Created { get; set; }

        public Asset()
        {
            Tags = new List<AssetTag>();
            Created = DateTimeOffset.Now;
        }
    }
}