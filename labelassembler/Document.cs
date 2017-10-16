using Newtonsoft.Json;
using SkiaSharp;
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

        public void Process(string outputFile, string workDir)
        {
            using (var surface = SKSurface.Create((int)this.Config.OutputSize.Width, (int)this.Config.OutputSize.Height, SKImageInfo.PlatformColorType, SKAlphaType.Premul))
            using (System.IO.FileStream inputStream = System.IO.File.OpenRead(System.IO.Path.Combine(workDir, this.Config.InputImage)))
            using (var stream = new SKManagedStream(inputStream))
            using (var bitmap = SKBitmap.Decode(stream))
            using (var paint = new SKPaint())
            {

                paint.IsAntialias = false;
                var canvas = surface.Canvas;
                canvas.Clear(SKColors.White);

                SKColor bgColor = SKColor.Parse(this.Config.BackgroundColor);
                SKColor outlineColor = SKColor.Parse(this.Config.OutlineColor);


                var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100);
                using (System.IO.FileStream outStream = new System.IO.FileStream(outputFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                {
                    data.SaveTo(outStream);
                }
            }
        }

    }
}
