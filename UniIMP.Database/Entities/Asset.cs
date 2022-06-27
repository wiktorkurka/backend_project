using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [Column(TypeName = "jsonb")]
        public string? Properties { get; set; }

        public List<AssetTag> Tags { get; set; }


        [ForeignKey("Agent")]
        [JsonIgnore]
        public int? AgentId { get; set; }
        public virtual SnmpAgent? Agent { get; set; }

        [NotMapped]
        [JsonIgnore]
        public JObject? jObject =>
            Properties != null ? JObject.Parse(Properties) : null;

        public Asset()
        {
            Tags = new List<AssetTag>();
        }
    }
}