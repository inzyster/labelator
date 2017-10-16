using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace labelassembler
{

    public class TileDef
    {
        public ushort StartColumn { get; set; }
        public ushort StartRow { get; set; }
        public ushort EndColumn { get; set; }
        public ushort EndRow { get; set; }

        public ushort RowSpan => (ushort)((EndRow - StartRow) + 1);
        public ushort ColumnSpan => (ushort)((EndColumn - StartColumn) + 1);

        public TileDef() { }

        public TileDef(ushort[] components)
        {
            StartColumn = components[0];
            StartRow = components[1];
            if (components.Length == 4)
            {
                EndColumn = components[2];
                EndRow = components[3];
            }
            else
            {
                EndColumn = StartColumn;
                EndRow = StartRow;
            }
        }

    }

    [JsonObject]
    public class Document
    {

        [JsonProperty("config")]
        public Config Config { get; set; }

        [JsonIgnore]
        public Dictionary<string, TileDef[]> TileDefs { get; private set; }

        [JsonProperty("tiles")]
        private Dictionary<string, ushort[][]> _tiledefs
        {
            set
            {
                var dict = value;
                this.TileDefs = new Dictionary<string, TileDef[]>();
                foreach (var kvp in dict)
                {
                    List<TileDef> defs = new List<TileDef>();
                    foreach (ushort[] defData in kvp.Value)
                    {
                        defs.Add(new TileDef(defData));
                    }
                    this.TileDefs.Add(kvp.Key, defs.ToArray());
                }
            }
        }

        public void Process(string outputFile)
        {

        }

    }
}
