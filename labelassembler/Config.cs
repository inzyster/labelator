using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace labelassembler
{

    public class Size
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public Size() { }

        public Size(int[] components)
        {
            this.Width = components[0];
            this.Height = components[1];
        }

        public static Size operator -(Size lhs, Size rhs) => new Size { Width = lhs.Width - rhs.Width, Height = lhs.Height - rhs.Height };

        public static Size operator +(Size lhs, Size rhs) => new Size { Width = lhs.Width + rhs.Width, Height = lhs.Height + rhs.Height };

        public override string ToString()
        {
            return string.Format("{0}, {1}", this.Width, this.Height);
        }

    }

    [JsonObject]
    public class Config
    {

        private const string TileSizeKey = "_tile";
        private const string OutputSizeKey = "_output";
        private const string MarginSizeKey = "_margin";
        private const string SpacingKey = "_spacing";
        private const string OutlineKey = "_outline";

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
        public Size Margin => this.Sizes[MarginSizeKey];
        public Size Spacing => this.Sizes[SpacingKey];
        public Size OutlineSize => this.Sizes[OutlineKey];

        [JsonProperty("sizes")]
        private Dictionary<string, int[]> _sizes
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
