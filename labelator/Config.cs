using System;
using System.Collections.Generic;

namespace labelator
{
    public class Config
    {

        public const int CharacterWidth = 9;
        public const int CharacterHeight = 16;

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

    }
}
