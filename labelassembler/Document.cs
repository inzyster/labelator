using Newtonsoft.Json;
using SkiaSharp;
using System;
using System.Linq;
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

    public class TileData
    {

        public TileDef TileDef { get; set; }
        public Size TargetSize { get; set; }
        public Size TargetSizeWithPadding { get; set; }
        public Size InitialSize { get; set; }
        public Node BinNode { get; set; }

    }

    internal class TargetSizeComparer : IComparer<Size>
    {
        public int Compare(Size x, Size y)
        {
            int maxX = Math.Max(x.Width, x.Height);
            int maxY = Math.Max(y.Width, y.Height);
            int result = maxX.CompareTo(maxY);
            if (result != 0)
                return result;

            int minX = Math.Min(x.Width, x.Height);
            int minY = Math.Min(y.Width, y.Height);
            result = minX.CompareTo(minY);
            if (result != 0)
                return result;

            result = new System.Random().Next(20) - 10;
            if (result != 0)
                return result;
            return -2;

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

        public int Process(string outputFile, string workDir)
        {

            Size avaliableSize = this.Config.OutputSize - this.Config.Margin;
            double availableArea = avaliableSize.Width * avaliableSize.Height;

            double requiredArea = 0.0;
            foreach (var kvp in this.TileDefs)
            {
                Size size = this.Config.Sizes[kvp.Key];
                requiredArea += (size.Width + this.Config.OutlineSize.Width + this.Config.Spacing.Width / 2) * (size.Height + this.Config.OutlineSize.Height + this.Config.Spacing.Height / 2) * kvp.Value.Count();
            }

            if (requiredArea > availableArea)
                return 2;

            List<TileData> allTiles = new List<TileData>();
            Size tileSize = this.Config.TileSize;
            foreach (var kvp in this.TileDefs)
            {
                Size targetSize = this.Config.Sizes[kvp.Key];
                foreach (var tiledef in kvp.Value)
                {
                    TileData data = new TileData
                    {
                        TargetSize = targetSize,
                        TileDef = tiledef
                    };
                    data.TargetSizeWithPadding = targetSize + this.Config.OutlineSize + this.Config.Spacing;
                    data.InitialSize = new Size
                    {
                        Width = tileSize.Width * data.TileDef.ColumnSpan,
                        Height = tileSize.Height * data.TileDef.RowSpan
                    };
                    allTiles.Add(data);
                }
            }

            var cmp = new TargetSizeComparer();
            allTiles = allTiles.OrderByDescending(t =>t.TargetSizeWithPadding, cmp).ToList();

            Bin bin = new Bin
            {
                Width = avaliableSize.Width,
                Height = avaliableSize.Height
            };
            var packed = bin.Pack(allTiles);

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

                int left = this.Config.Margin.Width / 2;
                int top = this.Config.Margin.Height / 2;

                int n = 0;
                foreach (var item in packed)
                {
                    if (item.BinNode == null)
                        continue;
                    System.Diagnostics.Debug.WriteLine(string.Format("item sized {0} in node: {1}", item.TargetSizeWithPadding, item.BinNode));
                    var tiledef = item.TileDef;
                    SKRect source = SKRect.Create(tiledef.StartColumn * tileSize.Width, tiledef.StartRow * tileSize.Height, tiledef.ColumnSpan * tileSize.Width, tiledef.RowSpan * tileSize.Height);
                    SKRect dest = SKRect.Create(left + item.BinNode.X, top + item.BinNode.Y, item.TargetSizeWithPadding.Width, item.TargetSizeWithPadding.Height);
                    canvas.DrawBitmap(bitmap, source, dest, paint);
                }

                var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100);
                using (System.IO.FileStream outStream = new System.IO.FileStream(outputFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                {
                    data.SaveTo(outStream);
                }
            }

            return 0;
        }

    }
}
