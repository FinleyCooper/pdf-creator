using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace PdfCreator
{
	public class PDFWriter
	{
        private PdfFont regularFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        private PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
        private PdfFont italicFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
        private PdfFont boldItalicFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLDOBLIQUE);
		public void Write(Content content)
		{
			using iText.Kernel.Pdf.PdfWriter writer = new("output.pdf");
			using PdfDocument pdf = new(writer);
			using Document document = new(pdf);

			document.SetMargins(50, 50, 50, 50);
			pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new PageNumberEventHandler(document, regularFont));

			int iteration = 0;

			while (pdf.GetNumberOfPages() < 3)
			{
				AppendContent(document, content);
				iteration++;
			}
		}

		private bool AppendContent(Document document, Content content)
		{
			foreach (ParagraphContent paragraphContent in content.Paragraphs)
			{
				Paragraph paragraph = new Paragraph()
					.SetMarginLeft(paragraphContent.IndentSize)
					.SetTextAlignment(paragraphContent.IsJustified ? TextAlignment.JUSTIFIED : TextAlignment.LEFT);

				foreach (TextSegment segment in paragraphContent.Segments)
				{
					Text text = new Text(segment.Text)
						.SetFont(GetFontForSegment(segment))
						.SetFontSize(segment.FontSize);

					paragraph.Add(text);
				}

				document.Add(paragraph);
			}

			return true;
		}

		private PdfFont GetFontForSegment(TextSegment segment)
		{
			if (segment.IsBold && segment.IsItalic)
			{
				return boldItalicFont;
			}

			if (segment.IsBold)
			{
				return boldFont;
			}

			if (segment.IsItalic)
			{
				return italicFont;
			}

			return regularFont;
		}

		private class PageNumberEventHandler : IEventHandler
		{
			private readonly Document document;
			private readonly PdfFont font;
			private const float FontSize = 10f;

			public PageNumberEventHandler(Document document, PdfFont font)
			{
				this.document = document;
				this.font = font;
			}

			public void HandleEvent(Event currentEvent)
			{
				if (currentEvent is not PdfDocumentEvent docEvent)
				{
					return;
				}

				PdfDocument pdf = docEvent.GetDocument();
				PdfPage page = docEvent.GetPage();
				int pageNumber = pdf.GetPageNumber(page);
				Rectangle pageSize = page.GetPageSize();

				Paragraph footer = new Paragraph(pageNumber.ToString())
					.SetFont(font)
					.SetFontSize(FontSize)
					.SetMargin(0);

				document.ShowTextAligned(footer, pageSize.GetWidth() / 2, document.GetBottomMargin() / 2, pageNumber, TextAlignment.CENTER, VerticalAlignment.BOTTOM, 0);
			}
		}
	}
}
