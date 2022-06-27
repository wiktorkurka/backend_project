using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Text.Json.Serialization;

namespace UniIMP.DataAccess.Entities
{
    public class SnmpAgent
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Asset")]
        [Required]
        [JsonIgnore]
        public int AssetId { get; set; }

        public Asset? Asset { get; set; }

        public string IpAddress
        {
            get
            {
                if (ipAddress == null)
                    return string.Empty;
                else
                    return ipAddress.ToString();
            }
            set
            {
                IPAddress? addr;
                IPAddress.TryParse(value, out addr);
                ipAddress = addr;
            }
        }

        [NotMapped]
        private IPAddress? ipAddress { get; set; }
    }
}