using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace labelassembler
{

    public class Size
    {
        public uint Width { get; set; }
        public uint Height { get; set; }

        public Size() { }

        public Size(uint[] components)
        {
            this.Width = components[0];
            this.Height = components[1];
        }
    }

    [JsonObject]
    public class Config
    {

        private const string TileSizeKey = "_tile";
        private const string OutputSizeKey = "_output";

        [JsonProperty("input")]
        public string InputImage { get; set; }

        [JsonProperty("background")]
        public string BackgroundColor { get; set; }

        [JsonProperty("outline")]
        public string OutlineColor { get; set; }

        [JsonIgnore]
        public Dictionary<string, Size> Sizes { get; private set; }

        public Size TileSize => this.Sizes[TileSizeKey];
        public Size OutputSize => this.Sizes[OutputSizeKey];

        [JsonProperty("sizes")]
        private Dictionary<string, uint[]> _sizes
        {
            set
            {
                var dict = value;
                this.Sizes = new Dictionary<string, Size>();
                foreach (var kvp in dict)
                {
                    this.Sizes.Add(kvp.Key, new Size(kvp.Value));
                }
            }
        }

    }
}
