using System;
using System.Collections.Generic;
using System.Linq;

namespace labelator
{
    public class Config
    {

        public const int CharacterWidth = 9;
        public const int CharacterHeight = 16;

        private const char ConfigSeparator = '=';

        public string HorizontalSingleLine { get; set; } = "-";
        public string HorizontalDoubleLine { get; set; } = "=";
        public string VerticalSingleLine { get; set; } = "|";
        public string VerticalDoubleLine { get; set; } = "!";
        public string Joint { get; set; } = "+";

        public int TabSize { get; set; } = 4;

        public int Scale { get; set; } = 1;

        public string ForegroundColor = "#000";
        public string BackgroundColor = "#fff";

        public Dictionary<char, Character> Mapping { get; private set; } = new Dictionary<char, Character>();

        public Config()
        {
            this.BuildDefaultMapping();
        }

        public void BuildDefaultMapping()
        {
            this.Mapping.Add('å', Character.Regular(CP437Character.LATIN_SMALL_LETTER_A_WITH_RING_ABOVE));
            this.Mapping.Add('ø', Character.Composite(CP437Character.LATIN_SMALL_LETTER_O, CP437Character.SOLIDUS));
            this.Mapping.Add('æ', Character.Regular(CP437Character.LATIN_SMALL_LETTER_AE));
            this.Mapping.Add('Å', Character.Regular(CP437Character.LATIN_CAPITAL_LETTER_A_WITH_RING_ABOVE));
            this.Mapping.Add('Ø', Character.Composite(CP437Character.LATIN_CAPITAL_LETTER_O, CP437Character.SOLIDUS));
            this.Mapping.Add('Æ', Character.Regular(CP437Character.LATIN_CAPITAL_LETTER_AE));
        }

        private void ProcessConfigLines(IEnumerable<string> lines, Action<string, string> processor)
        {
            if (processor == null)
                return;

            if ((lines?.Count() ?? 0) == 0)
                return;

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed))
                    continue;

                string[] components = trimmed.Split(new char[] { ConfigSeparator }, StringSplitOptions.RemoveEmptyEntries);
                if (components.Length < 2)
                    continue;

                if (components.Any(s => string.IsNullOrEmpty(s.Trim())))
                    continue;

                processor.Invoke(components[0].Trim(), components[1].Trim());
            }
        }

        public void Parse(IEnumerable<string> lines)
        {
            Action<string, string> processor = (key, value) =>
            {
                if (key == nameof(this.ForegroundColor))
                {
                    this.ForegroundColor = value;
                }
                else if (key == nameof(this.BackgroundColor))
                {
                    this.BackgroundColor = value;
                }
                else if (key == nameof(this.HorizontalDoubleLine))
                {
                    this.HorizontalDoubleLine = value;
                }
                else if (key == nameof(this.HorizontalSingleLine))
                {
                    this.HorizontalSingleLine = value;
                }
                else if (key == nameof(this.VerticalDoubleLine))
                {
                    this.VerticalDoubleLine = value;
                }
                else if (key == nameof(this.VerticalSingleLine))
                {
                    this.VerticalSingleLine = value;
                }
                else if (key == nameof(this.Scale))
                {
                    this.Scale = int.Parse(value);
                }
                else if (key == nameof(this.TabSize))
                {
                    this.TabSize = int.Parse(value);
                }
                else if (key == nameof(this.Joint))
                {
                    this.Joint = value;
                }
            };
            this.ProcessConfigLines(lines, processor);
        }

        public void ParseMapping(IEnumerable<string> lines)
        {
            Action<string, string> processor = (source, target) =>
            {
                Character c = new Character();
                if (target.StartsWith(@"\"))
                {
                    string literal = target.Substring(1);
                    c = Character.Composite(literal.Select(s => (CP437Character)s).ToArray());
                }
                else
                {
                    var characters = target.Length;
                    byte[] parsed = new byte[characters / 2];
                    for (int i = 0; i < characters; i += 2)
                    {
                        parsed[i / 2] = Convert.ToByte(target.Substring(i, 2), 16);
                    }
                    c = Character.Composite(parsed.Select(b => (CP437Character)b).ToArray());
                }
                if (c.IsEmpty == false)
                {
                    this.Mapping.Add(source.First(), c);
                }
            };
            this.ProcessConfigLines(lines, processor);
        }

    }
}
