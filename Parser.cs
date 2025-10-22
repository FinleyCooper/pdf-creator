using System.Globalization;

namespace PdfCreator
{
    public class Parser
    {
        private const float DefaultFontSize = 12.0f;
        private const float IndentAmount = 12.0f;

        private bool isBold;
        private bool isItalic;
        private float indentSize;
        private float currentFontSize = DefaultFontSize;
        private bool isFill;

    private ParagraphContent? activeParagraph;
    private bool paragraphHasContent;

        public Content ParseFile(string[] lines)
        {
            Content content = new Content();

            foreach (string rawLine in lines)
            {
                string line = rawLine.TrimEnd('\r', '\n');

                if (string.IsNullOrWhiteSpace(line))
                {
                    FinishParagraph(content);
                    continue;
                }

                if (line.StartsWith('.'))
                {
                    HandleCommand(line, content);
                }
                else
                {
                    AddText(line);
                }
            }

            FinishParagraph(content);
            return content;
        }

        private void HandleCommand(string rawCommand, Content content)
        {
            string[] commandParts = rawCommand.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            string command = commandParts[0].ToLowerInvariant();

            switch (command)
            {
                case ".paragraph":
                    FinishParagraph(content);
                    break;
                case ".fill":
                    isFill = true;
                    break;
                case ".nofill":
                    isFill = false;
                    break;
                case ".regular":
                    isBold = false;
                    isItalic = false;
                    break;
                case ".italic":
                    isItalic = true;
                    break;
                case ".bold":
                    isBold = true;
                    break;
                case ".indent":
                    ChangeIndent(commandParts, content);
                    break;
                case ".large":
                    currentFontSize += 8f;
                    break;
                case ".normal":
                    currentFontSize = DefaultFontSize;
                    break;
            }
        }

        private void ChangeIndent(string[] commandParts, Content content)
        {
            string rawValue = commandParts[1];

            if (!float.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float indentValue))
            {
                Console.Error.WriteLine($"Could not parse indent value: {rawValue}");
                return;
            }

            if (paragraphHasContent)
            {
                FinishParagraph(content);
            }

            indentSize += indentValue * IndentAmount;

            if (activeParagraph is { } paragraph && !paragraphHasContent)
            {
                paragraph.IndentSize = indentSize;
            }
        }

        private void AddText(string text)
        {
            if (activeParagraph is null)
            {
                activeParagraph = new ParagraphContent
                {
                    IndentSize = indentSize,
                    IsJustified = isFill,
                };

                paragraphHasContent = false;
            }

            string preparedText = PrepText(text);

            if (preparedText.Length == 0)
            {
                return;
            }

            TextSegment segment = new TextSegment(preparedText, currentFontSize, isBold, isItalic);
            activeParagraph.Segments.Add(segment);
            paragraphHasContent = true;
        }
        private string PrepText(string text)
        {
            if (!paragraphHasContent || string.IsNullOrEmpty(text))
            {
                return text;
            }

            return char.IsWhiteSpace(text[0]) ? text : " " + text;
        }

        private void FinishParagraph(Content content)
        {
            if (activeParagraph is not null)
            {
                if (paragraphHasContent)
                {
                    content.Paragraphs.Add(activeParagraph);
                }

                activeParagraph = null;
            }

            paragraphHasContent = false;
        }
    }
}