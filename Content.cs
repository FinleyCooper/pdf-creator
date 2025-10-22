namespace PdfCreator
{
	public class Content
	{
		public List<ParagraphContent> Paragraphs { get; } = new();
	}

	public class ParagraphContent
	{
		public float IndentSize { get; set; }

		public bool IsJustified { get; set; }

		public List<TextSegment> Segments { get; } = new();
	}

	public record TextSegment(string Text, float FontSize, bool IsBold, bool IsItalic);
}
