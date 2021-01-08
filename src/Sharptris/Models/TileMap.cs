namespace Sharptris.Models
{
    using System.Globalization;
    using System.Text.Json.Serialization;

    public partial class TileMap
    {
        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("infinite")]
        public bool Infinite { get; set; }

        [JsonPropertyName("layers")]
        public Layer[] Layers { get; set; }

        [JsonPropertyName("nextlayerid")]
        public int Nextlayerid { get; set; }

        [JsonPropertyName("nextobjectid")]
        public int Nextobjectid { get; set; }

        [JsonPropertyName("orientation")]
        public string Orientation { get; set; }

        [JsonPropertyName("renderorder")]
        public string Renderorder { get; set; }

        [JsonPropertyName("tiledversion")]
        public string TiledVersion { get; set; }

        [JsonPropertyName("tileheight")]
        public int TileHeight { get; set; }

        [JsonPropertyName("tilesets")]
        public Tileset[] Tilesets { get; set; }

        [JsonPropertyName("tilewidth")]
        public int TileWidth { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("version")]
        public double Version { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }
    }

    public partial class Layer
    {
        [JsonPropertyName("data")]
        public int[] Data { get; set; }

        [JsonPropertyName("height")]
        public int Height { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("opacity")]
        public int Opacity { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("visible")]
        public bool Visible { get; set; }

        [JsonPropertyName("width")]
        public int Width { get; set; }

        [JsonPropertyName("x")]
        public int X { get; set; }

        [JsonPropertyName("y")]
        public int Y { get; set; }
    }

    public partial class Tileset
    {
        [JsonPropertyName("firstgid")]
        public int Firstgid { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }
    }
}
