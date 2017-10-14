using System;
using System.Linq;
using System.Collections.Generic;

using SkiaSharp;

namespace labelator
{

    internal enum LineStyle
    {
        None = 0,
        Single,
        Double
    }

    internal struct Joint
    {
        internal LineStyle Top { get; set; }
        internal LineStyle Right { get; set; }
        internal LineStyle Bottom { get; set; }
        internal LineStyle Left { get; set; }

        public Character GetCharacter()
        {
            return Character.Regular(GetCharacterValue());
        }

        public CP437Character GetCharacterValue()
        {
            Func<LineStyle, int> counter = (s) => s > LineStyle.None ? 1 : 0;
            int nonEmptyCount = counter(Top) + counter(Right) + counter(Bottom) + counter(Left);
            if (nonEmptyCount < 2)
                return CP437Character.PLUS_SIGN;

            if (nonEmptyCount == 2)
            {
                if (Left != LineStyle.None && Top != LineStyle.None)
				{
					if (Left == LineStyle.Single)
					{
						return Top == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_UP_AND_LEFT : CP437Character.BOX_DRAWINGS_UP_DOUBLE_AND_LEFT_SINGLE;
					}
					else if (Left == LineStyle.Double)
					{
						return Top == LineStyle.Single ? CP437Character.BOX_DRAWINGS_UP_SINGLE_AND_LEFT_DOUBLE : CP437Character.BOX_DRAWINGS_DOUBLE_UP_AND_LEFT;
					}
				}                
                else if (Top != LineStyle.None && Right != LineStyle.None)
                {
                    if (Top == LineStyle.Single)
                    {
                        return Right == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_UP_AND_RIGHT : CP437Character.BOX_DRAWINGS_UP_SINGLE_AND_RIGHT_DOUBLE;
                    }
                    else if (Top == LineStyle.Double)
                    {
                        return Right == LineStyle.Single ? CP437Character.BOX_DRAWINGS_UP_DOUBLE_AND_RIGHT_SINGLE : CP437Character.BOX_DRAWINGS_DOUBLE_UP_AND_RIGHT;
                    }
                }
                else if (Right != LineStyle.None && Bottom != LineStyle.None)
                {
                    if (Right == LineStyle.Single)
                    {
                        return Bottom == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_DOWN_AND_RIGHT : CP437Character.BOX_DRAWINGS_DOWN_DOUBLE_AND_RIGHT_SINGLE;
                    }
                    else if (Right == LineStyle.Double)
                    {
                        return Bottom == LineStyle.Single ? CP437Character.BOX_DRAWINGS_DOWN_SINGLE_AND_RIGHT_DOUBLE : CP437Character.BOX_DRAWINGS_DOUBLE_DOWN_AND_RIGHT;
                    }
                }
                else if (Bottom != LineStyle.None && Left != LineStyle.None)
                {
                    if (Bottom == LineStyle.Single)
                    {
                        return Left == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_DOWN_AND_LEFT : CP437Character.BOX_DRAWINGS_DOWN_SINGLE_AND_LEFT_DOUBLE;
                    }
                    else if (Bottom == LineStyle.Double)
                    {
                        return Left == LineStyle.Single ? CP437Character.BOX_DRAWINGS_DOWN_DOUBLE_AND_LEFT_SINGLE : CP437Character.BOX_DRAWINGS_DOUBLE_DOWN_AND_LEFT;
                    }
                }
            }
            else if (nonEmptyCount == 3)
            {
                if (Left != LineStyle.None && Top != LineStyle.None && Right != LineStyle.None)
                {
                    if (Left == LineStyle.Single && Right == LineStyle.Single)
                    {
                        return Top == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_UP_AND_HORIZONTAL : CP437Character.BOX_DRAWINGS_UP_DOUBLE_AND_HORIZONTAL_SINGLE;
                    }
                    else if (Left == LineStyle.Double && Right == LineStyle.Double)
                    {
                        return Top == LineStyle.Single ? CP437Character.BOX_DRAWINGS_UP_SINGLE_AND_HORIZONTAL_DOUBLE : CP437Character.BOX_DRAWINGS_DOUBLE_UP_AND_HORIZONTAL;
                    }
                }
                else if (Top != LineStyle.None && Right != LineStyle.None && Bottom != LineStyle.None)
                {
                    if (Top == LineStyle.Single && Bottom == LineStyle.Single)
                    {
                        return Right == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_VERTICAL_AND_RIGHT : CP437Character.BOX_DRAWINGS_VERTICAL_SINGLE_AND_RIGHT_DOUBLE;
                    }
                    else if (Top == LineStyle.Double && Bottom == LineStyle.Double)
                    {
                        return Right == LineStyle.Single ? CP437Character.BOX_DRAWINGS_VERTICAL_DOUBLE_AND_RIGHT_SINGLE : CP437Character.BOX_DRAWINGS_DOUBLE_VERTICAL_AND_RIGHT;
                    }
                }
                else if (Right != LineStyle.None && Bottom != LineStyle.None && Left != LineStyle.None)
                {
                    if (Left == LineStyle.Single && Right == LineStyle.Single)
                    {
                        return Bottom == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_DOWN_AND_HORIZONTAL : CP437Character.BOX_DRAWINGS_DOWN_DOUBLE_AND_HORIZONTAL_SINGLE;
                    }
                    else if (Left == LineStyle.Double && Right == LineStyle.Double)
                    {
                        return Bottom == LineStyle.Single ? CP437Character.BOX_DRAWINGS_DOWN_SINGLE_AND_HORIZONTAL_DOUBLE : CP437Character.BOX_DRAWINGS_DOUBLE_DOWN_AND_HORIZONTAL;
                    }
                }
                else if (Bottom != LineStyle.None && Left != LineStyle.None && Top != LineStyle.None)
                {
                    if (Top == LineStyle.Single && Bottom == LineStyle.Single)
                    {
                        return Left == LineStyle.Single ? CP437Character.BOX_DRAWINGS_LIGHT_VERTICAL_AND_LEFT : CP437Character.BOX_DRAWINGS_VERTICAL_SINGLE_AND_LEFT_DOUBLE;
                    }
                    else if (Top == LineStyle.Double && Bottom == LineStyle.Double)
                    {
                        return Left == LineStyle.Single ? CP437Character.BOX_DRAWINGS_VERTICAL_DOUBLE_AND_LEFT_SINGLE : CP437Character.BOX_DRAWINGS_DOUBLE_VERTICAL_AND_LEFT;
                    }
                }
            }
            else 
            {
                if (Top == LineStyle.Single && Bottom == LineStyle.Single)
                {
                    if (Left == LineStyle.Single && Right == LineStyle.Single)
                    {
                        return CP437Character.BOX_DRAWINGS_LIGHT_VERTICAL_AND_HORIZONTAL;
                    }
                    else if (Left == LineStyle.Double && Right == LineStyle.Double)
                    {
                        return CP437Character.BOX_DRAWINGS_VERTICAL_SINGLE_AND_HORIZONTAL_DOUBLE;
                    }
                }
                else if (Top == LineStyle.Double && Bottom == LineStyle.Double)
                {
                    if (Left == LineStyle.Single && Right == LineStyle.Single)
                    {
                        return CP437Character.BOX_DRAWINGS_VERTICAL_DOUBLE_AND_HORIZONTAL_SINGLE;
                    }
                    else if (Left == LineStyle.Double && Right == LineStyle.Double)
                    {
                        return CP437Character.BOX_DRAWINGS_DOUBLE_VERTICAL_AND_HORIZONTAL;
                    }
                }
            }
            return CP437Character.QUESTION_MARK;
        }

    }

    public class Document
    {

        public Config Config { get; set; } = new Config();

        public int Width { get; set; }
        public int Height { get; set; }

        public Character[][] Characters { get; private set; }

        internal LineStyle GetLineStyleFor(Character character)
        {
            if (character.IsEmpty)
                return LineStyle.None;

            if (character.IsComposite)
                return LineStyle.None;

            var c = character.Values.First();

            if (c == CP437Character.BOX_DRAWINGS_LIGHT_VERTICAL || c == CP437Character.BOX_DRAWINGS_LIGHT_HORIZONTAL)
                return LineStyle.Single;
            else if (c == CP437Character.BOX_DRAWINGS_DOUBLE_VERTICAL || c == CP437Character.BOX_DRAWINGS_DOUBLE_HORIZONTAL)
                return LineStyle.Double;

            return LineStyle.None;
        }

        private Character ParseCharacter(char c)
        {
            if (this.Config.Mapping.ContainsKey(c))
            {
                return this.Config.Mapping[c];
            }
            CP437Character charValue = (CP437Character)c;
            return Character.Regular(charValue);
        }

        private const string ConfigHeader = "[Config]";
        private const string MappingHeader = "[Mapping]";
        private const string DocumentHeader = "[Document]";

        public void Parse(string[] lines)
        {
            var nonEmptyLines = lines.Select(l => l.Trim()).Where(l => string.IsNullOrEmpty(l) == false).ToList();
            Height = nonEmptyLines.Count();
            List<string> configLines = new List<string>();
            List<string> mappingLines = new List<string>();
            List<string> documentLines = new List<string>();
            int configSectionIndex = nonEmptyLines.IndexOf(ConfigHeader);
            int mappingSectionIndex = nonEmptyLines.IndexOf(MappingHeader);
            int documentSectionIndex = nonEmptyLines.IndexOf(DocumentHeader);
            bool justDocument = (configSectionIndex < 0 && mappingSectionIndex < 0 && documentSectionIndex < 0);
            if (justDocument == true)
            {
                documentLines.AddRange(nonEmptyLines);
            }
            else
            {
                if (configSectionIndex >= 0)
                {
                    for (int i = configSectionIndex + 1; i < Height; i++)
                    {
                        string l = nonEmptyLines[i];
                        if (l == MappingHeader || l == DocumentHeader)
                        {
                            break;
                        }
                        configLines.Add(l);
                    }
                }
                if (mappingSectionIndex >= 0)
                {
                    for (int i = mappingSectionIndex + 1; i < Height; i++)
                    {
                        string l = nonEmptyLines[i];
                        if (l == ConfigHeader || l == DocumentHeader)
                        {
                            break;
                        }
                        mappingLines.Add(l);
                    }
                }
                if (documentSectionIndex >= 0)
                {
                    for (int i = documentSectionIndex + 1; i < Height; i++)
                    {
                        string l = nonEmptyLines[i];
                        if (l == ConfigHeader || l == MappingHeader)
                        {
                            break;
                        }
                        documentLines.Add(l);
                    }
                }
                this.Config.Parse(configLines);
                this.Config.ParseMapping(mappingLines);
            }
            Height = documentLines.Count();
            foreach (string line in documentLines)
            {
                if (line.Length > Width)
                {
                    Width = line.Length;
                }
            }
            List<string> linesToUse = new List<string>();
            foreach (string line in documentLines)
            {
                if (line.Length == Width)
                {
                    linesToUse.Add(line);
                }
                else if (line.Length < Width)
                {
                    linesToUse.Add(line.PadRight(Width));
                }
            }
            Height = linesToUse.Count;
            List<Character[]> rows = new List<Character[]>();
            for (int r = 0; r < Height; r++)
            {
                Character[] row = Enumerable.Range(0, Width).Select(x => Character.Regular(CP437Character.SPACE)).ToArray();
                rows.Add(row);
            }
            Characters = rows.ToArray();
            for (int r = 0; r < Height; r++)
            {
                string line = linesToUse[r];
                for (int c = 0; c < Width; c++)
                {
                    string character = line[c].ToString();
                    if (character == this.Config.HorizontalDoubleLine)
                    {
                        this.Characters[r][c] = Character.Regular(CP437Character.BOX_DRAWINGS_DOUBLE_HORIZONTAL);
                    }
                    else if (character == this.Config.HorizontalSingleLine)
                    {
                        this.Characters[r][c] = Character.Regular(CP437Character.BOX_DRAWINGS_LIGHT_HORIZONTAL);
                    }
                    else if (character == this.Config.VerticalDoubleLine)
                    {
                        this.Characters[r][c] = Character.Regular(CP437Character.BOX_DRAWINGS_DOUBLE_VERTICAL);
                    }
                    else if (character == this.Config.VerticalSingleLine)
                    {
                        this.Characters[r][c] = Character.Regular(CP437Character.BOX_DRAWINGS_LIGHT_VERTICAL);
                    }
                    else if (character != this.Config.Joint)
                    {
                        this.Characters[r][c] = ParseCharacter(line[c]);
                    }
                }
            }
            for (int r = 0; r < Height; r++)
            {
                string line = linesToUse[r];
                for (int c = 0; c < Width; c++)
                {
                    string character = line[c].ToString();
                    if (character == this.Config.Joint)
                    {
                        Joint j = new Joint();
                        Character left, right, top, bottom;
                        left = right = top = bottom = default(Character);

                        if (c > 0)
                        {
                            left = this.Characters[r][c - 1];
                        }
                        if (c < Width-1)
                        {
                            right = this.Characters[r][c + 1];
                        }
                        if (r > 0)
                        {
                            top = this.Characters[r - 1][c];
                        }
                        if (r < Height-1)
                        {
                            bottom = this.Characters[r + 1][c];
                        }

                        j.Left = GetLineStyleFor(left);
                        j.Right = GetLineStyleFor(right);
                        j.Top = GetLineStyleFor(top);
                        j.Bottom = GetLineStyleFor(bottom);
                        this.Characters[r][c] = j.GetCharacter();
                    }
                }
            }
        }

        public void Render(string outputFile)
        {
            int targetWidth = this.Width * Config.CharacterWidth * this.Config.Scale;
            int targetHeight = this.Height * Config.CharacterHeight * this.Config.Scale;

            using (var surface = SKSurface.Create(targetWidth, targetHeight, SKImageInfo.PlatformColorType, SKAlphaType.Premul))
            using (System.IO.FileStream inputStream = System.IO.File.OpenRead("437.png"))
            using (var stream = new SKManagedStream(inputStream))
            using (var bitmap = SKBitmap.Decode(stream))
            using (var paint = new SKPaint())
            {
                SKColor fg = SKColor.Parse(this.Config.ForegroundColor);
                SKColor bg = SKColor.Parse(this.Config.BackgroundColor);

                byte[] rt = Enumerable.Range(0, 256).Select(x => x == 255 ? fg.Red : (byte)0).ToArray();
                byte[] gt = Enumerable.Range(0, 256).Select(x => x == 255 ? fg.Green : (byte)0).ToArray();
                byte[] bt = Enumerable.Range(0, 256).Select(x => x == 255 ? fg.Blue : (byte)0).ToArray();

				paint.Color = fg;
                paint.IsAntialias = false;
                paint.ColorFilter = SKColorFilter.CreateTable(null, rt, gt, bt);
                var canvas = surface.Canvas;
                canvas.Clear(bg);

                float charW = Config.CharacterWidth * this.Config.Scale;
                float charH = Config.CharacterHeight * this.Config.Scale;

                for (int r = 0; r < this.Height; r++)
                {
                    for (int c = 0; c < this.Width; c++)
                    {
                        Character character = this.Characters[r][c];
                        foreach (CP437Character characterValue in character.Values)
                        {
                            int bitmapRow = (int)characterValue / 32;
                            int bitmapColumn = (int)characterValue % 32;
                            int bitmapX = bitmapColumn * Config.CharacterWidth;
                            int bitmapY = bitmapRow * Config.CharacterHeight;
                            SKRect source = new SKRect(bitmapX, bitmapY, bitmapX + Config.CharacterWidth, bitmapY + Config.CharacterHeight);
                            SKRect dest = new SKRect(c * charW, r * charH, (c + 1) * charW, (r + 1) * charH);
                            canvas.DrawBitmap(bitmap, source, dest, paint);
                        }
                    }
                }

                var data = surface.Snapshot().Encode(SKEncodedImageFormat.Png, 100);
                using (System.IO.FileStream outStream = new System.IO.FileStream(outputFile, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
                {
                    data.SaveTo(outStream);
                }
            }

        }



    }
}
