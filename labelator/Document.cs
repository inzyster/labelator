using System;
using System.Linq;
using System.Collections.Generic;
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

        public CP437Character GetCharacter()
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

        public CP437Character[][] Characters { get; private set; }

        internal LineStyle GetLineStyleFor(CP437Character character)
        {
            if (character == CP437Character.BOX_DRAWINGS_LIGHT_VERTICAL || character == CP437Character.BOX_DRAWINGS_LIGHT_HORIZONTAL)
                return LineStyle.Single;
            else if (character == CP437Character.BOX_DRAWINGS_DOUBLE_VERTICAL || character == CP437Character.BOX_DRAWINGS_DOUBLE_HORIZONTAL)
                return LineStyle.Double;

            return LineStyle.None;
        }

        public void Parse(string[] lines)
        {
            var nonEmptyLines = lines.Select(l => l.Trim()).Where(l => string.IsNullOrEmpty(l) == false).ToList();
            Height = nonEmptyLines.Count();
            foreach (string line in nonEmptyLines)
            {
                if (line.Length > Width)
                {
                    Width = line.Length;
                }
            }
            List<CP437Character[]> rows = new List<CP437Character[]>();
            for (int r = 0; r < Height; r++)
            {
                CP437Character[] row = Enumerable.Range(0, Width).Select(x => CP437Character.SPACE).ToArray();
                rows.Add(row);
            }
            Characters = rows.ToArray();
            for (int r = 0; r < Height; r++)
            {
                string line = nonEmptyLines[r];
                for (int c = 0; c < Width; c++)
                {
                    string character = line[c].ToString();
                    if (character == this.Config.HorizontalDoubleLine)
                    {
                        this.Characters[r][c] = CP437Character.BOX_DRAWINGS_DOUBLE_HORIZONTAL;
                    }
                    else if (character == this.Config.HorizontalSingleLine)
                    {
                        this.Characters[r][c] = CP437Character.BOX_DRAWINGS_LIGHT_HORIZONTAL;
                    }
                    else if (character == this.Config.VerticalDoubleLine)
                    {
                        this.Characters[r][c] = CP437Character.BOX_DRAWINGS_DOUBLE_VERTICAL;
                    }
                    else if (character == this.Config.VerticalSingleLine)
                    {
                        this.Characters[r][c] = CP437Character.BOX_DRAWINGS_LIGHT_VERTICAL;
                    }
                    else if (character != this.Config.Joint)
                    {
                        this.Characters[r][c] = (CP437Character)line[c];
                    }
                }
            }
            for (int r = 0; r < Height; r++)
            {
                string line = nonEmptyLines[r];
                for (int c = 0; c < Width; c++)
                {
                    string character = line[c].ToString();
                    if (character == this.Config.Joint)
                    {
                        Joint j = new Joint();
                        CP437Character left = CP437Character.NULL;
                        CP437Character right = CP437Character.NULL;
                        CP437Character top = CP437Character.NULL;
                        CP437Character bottom = CP437Character.NULL;

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
            
        }

    }
}
