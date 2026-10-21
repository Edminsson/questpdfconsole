using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

var document = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.DefaultTextStyle(x => x.FontSize(12));

        page.Header()
            .Text("PDF/A-2b sample")
            .SemiBold().FontSize(24).FontColor(Colors.Blue.Darken2);

        page.Content()
            .PaddingVertical(1, Unit.Centimetre)
            .Column(column =>
            {
                column.Spacing(10);
                column.Item().Text("This document was generated with QuestPDF and conforms to PDF/A-2b.");
                column.Item().Text(Placeholders.LoremIpsum());
            });

        page.Footer()
            .AlignCenter()
            .Text(x =>
            {
                x.Span("Page ");
                x.CurrentPageNumber();
                x.Span(" of ");
                x.TotalPages();
            });
    });
})
.WithMetadata(new DocumentMetadata
{
    Title = "PDF/A-2b sample",
    Author = "QuestPdfTest",
    Subject = "PDF/A-2b example",
    Language = "en-US"
})
.WithSettings(new DocumentSettings
{
    PDFA_Conformance = PDFA_Conformance.PDFA_2B
});

document.GeneratePdf("sample-pdfa2b.pdf");
Console.WriteLine($"Created {Path.GetFullPath("sample-pdfa2b.pdf")}");
