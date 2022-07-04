using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace UniIMP.DataAccess.Entities
{
    public enum AgentState
    {
        Up = 1,
        Down = 2,
    }

    public class SnmpAgent
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Asset")]
        [Required]
        [JsonIgnore]
        public int AssetId { get; set; }

        public virtual Asset? Asset { get; set; }

        [Required]
        public IPAddress IpAddress { get; set; }

        [Required]
        public string Community { get; set; }

        [Required]
        public DateTimeOffset Created { get; set; }

        [Required]
        public DateTimeOffset LastSeen { get; set; }

        [Required]
        public DateTimeOffset LastUpdated {get; set;}

        [Required]
        public AgentState State { get; set; }

        public SnmpAgent()
        {
            Created = DateTimeOffset.UtcNow;
            LastSeen = DateTimeOffset.MinValue;
            LastUpdated = DateTimeOffset.MinValue;
            State = AgentState.Down;
        }
    }
}