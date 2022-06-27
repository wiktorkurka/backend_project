using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Text.Json.Serialization;

namespace UniIMP.DataAccess.Entities
{
    public class AssetTag
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public List<Asset> Assets { get; set; }

        [Column("Color")]
        public int Argb
        {
            get { return Color.ToArgb(); }
            set { Color = Color.FromArgb(value); }
        }

        [NotMapped]
        [JsonIgnore]
        public Color Color { get; set; }

        public AssetTag()
        {
            Random random = new Random();
            Color = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

            Assets = new List<Asset>();
        }

        public void SetLabelRGB(byte red, byte green, byte blue)
        {
            Color = Color.FromArgb(red, green, blue);
        }
    }
}