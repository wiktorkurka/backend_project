using System.ComponentModel.DataAnnotations;

namespace UniIMP.Models
{
    public class SnmpAgentIntermediateModel
    {
        [Required]
        public int AssetId { get; set; }

        [Required]
        public string IpAddress { get; set; }

        [Required]
        public string Community { get; set; }

    }
}