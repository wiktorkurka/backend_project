﻿using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UniIMP.DataAccess.Entities

{
    public class Asset : DatabaseEntity
    {
#pragma warning disable 8618

        [Required]
        public string Name { get; set; }

#pragma warning restore 8618

        [Column(TypeName = "jsonb")]
        public string? Properties { get; set; }

        [JsonIgnore]
        public List<AssetTag> Tags { get; set; }

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