using System;
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

    }
}
