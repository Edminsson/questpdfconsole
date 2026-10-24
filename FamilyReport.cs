using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

record Heir(string Name, string Born, string Status, string Relationship, string Basis, string Share);

record TreeNode(int Depth, string Name, string Lifespan, string Note);

record Excluded(string Name, string Relationship, string Reason);

static class FamilyReport
{
    const string Deceased = "Henrik Anders Lindqvist";
    static readonly string Accent = Colors.Blue.Darken3;

    static readonly Heir[] Heirs =
    {
        new("Margareta Lindqvist (née Holm)", "12 Mar 1951", "Living", "Surviving spouse", "Married 8 Jun 1974; marriage subsisting at death", "4/12 (1/3)"),
        new("Erik Henrik Lindqvist", "3 Feb 1976", "Living", "Child (of the marriage)", "Direct descendant, 1st degree", "2/12 (1/6)"),
        new("Sofia Margareta Lindqvist", "19 Oct 1979", "Living", "Child (of the marriage)", "Direct descendant, 1st degree", "2/12 (1/6)"),
        new("Anna Karin Berg", "22 Jul 1971", "Living", "Child (born before the marriage, paternity acknowledged)", "Direct descendant, 1st degree", "2/12 (1/6)"),
        new("Lena Johansson (née Lindqvist)", "14 Apr 2003", "Living", "Grandchild (daughter of Johan, predeceased)", "Represents her father, 2nd degree", "1/12"),
        new("Oskar Johan Lindqvist", "30 Nov 2006", "Living", "Grandchild (son of Johan, predeceased)", "Represents his father, 2nd degree", "1/12"),
    };

    static readonly TreeNode[] Tree =
    {
        new(0, "Henrik Anders Lindqvist", "1948 - 2026", "Deceased (propositus)"),
        new(1, "Margareta Holm", "b. 1951", "Spouse, married 1974"),
        new(1, "Anna Karin Berg", "b. 1971", "Child, born before marriage; paternity acknowledged 1972"),
        new(1, "Erik Henrik Lindqvist", "b. 1976", "Child of the marriage"),
        new(1, "Johan Anders Lindqvist", "1977 - 2019", "Child of the marriage; predeceased the deceased"),
        new(2, "Lena Johansson", "b. 2003", "Child of Johan; takes his share by representation"),
        new(2, "Oskar Johan Lindqvist", "b. 2006", "Child of Johan; takes his share by representation"),
        new(1, "Sofia Margareta Lindqvist", "b. 1979", "Child of the marriage"),
    };

    static readonly Excluded[] ExcludedPersons =
    {
        new("Birgitta Ek (née Sjöberg)", "Former spouse (m. 1969, divorced 1972)", "Marriage dissolved by final decree before the date of death; no right to inherit."),
        new("Gunnar Lindqvist", "Father (1919 - 1994)", "Predeceased. In any case, ascendants are excluded while descendants survive."),
        new("Ingrid Lindqvist (née Dahl)", "Mother (1923 - 2001)", "Predeceased. In any case, ascendants are excluded while descendants survive."),
        new("Stig Lindqvist", "Brother (b. 1945)", "Collateral relatives are excluded while descendants survive."),
        new("Karl Berg", "Spouse of Anna Berg", "Relationship by marriage only; not a blood relative of the deceased."),
    };

    static readonly (string Document, string Issuer, string Date)[] Sources =
    {
        ("Death certificate No. 2026/04418", "Population Registry, Uppsala", "18 Aug 2026"),
        ("Marriage certificate, Henrik Lindqvist / Margareta Holm", "Parish of St. Lars, Uppsala", "8 Jun 1974"),
        ("Decree of divorce, Lindqvist / Sjöberg", "Uppsala District Court, case 1972-D-0231", "14 Sep 1972"),
        ("Birth records of Anna, Erik, Johan and Sofia", "Population Registry, Uppsala", "1971 - 1979"),
        ("Acknowledgement of paternity (Anna Karin)", "Social Welfare Board, Uppsala", "2 Mar 1972"),
        ("Death certificate of Johan Anders Lindqvist", "Population Registry, Uppsala", "27 Jan 2019"),
        ("Birth records of Lena and Oskar", "Population Registry, Uppsala", "2003, 2006"),
        ("Search of the Register of Wills", "National Probate Registry", "1 Sep 2026"),
    };

    public static Document Create() => Document.Create(container =>
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.Margin(2, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(10).LineHeight(1.3f));

            page.Header().Column(col =>
            {
                col.Item().Row(row =>
                {
                    row.RelativeItem().Text("GENEALOGICAL REPORT - DETERMINATION OF HEIRS").FontSize(8).FontColor(Colors.Grey.Darken1);
                    row.RelativeItem().AlignRight().Text("Estate of " + Deceased).FontSize(8).FontColor(Colors.Grey.Darken1);
                });
                col.Item().PaddingTop(3).LineHorizontal(0.75f).LineColor(Accent);
            });

            page.Content().PaddingVertical(0.6f, Unit.Centimetre).Column(col =>
            {
                col.Spacing(12);

                col.Item().Element(Title);
                col.Item().Element(Purpose);
                col.Item().Element(DeceasedDetails);
                col.Item().Element(HeirsSummary);
                col.Item().Element(FamilyTree);
                col.Item().Element(ExcludedSection);
                col.Item().Element(SourcesSection);
                col.Item().Element(Basis);
                col.Item().Element(Certification);
            });

            page.Footer().Column(col =>
            {
                col.Item().LineHorizontal(0.5f).LineColor(Colors.Grey.Lighten1);
                col.Item().PaddingTop(3).Row(row =>
                {
                    row.RelativeItem().Text("Sample document with fictitious persons - not a legal instrument.").FontSize(8).FontColor(Colors.Grey.Darken1);
                    row.AutoItem().Text(x =>
                    {
                        x.DefaultTextStyle(s => s.FontSize(8).FontColor(Colors.Grey.Darken1));
                        x.Span("Page ");
                        x.CurrentPageNumber();
                        x.Span(" of ");
                        x.TotalPages();
                    });
                });
            });
        });
    })
    .WithMetadata(new DocumentMetadata
    {
        Title = "Genealogical Report - Determination of Heirs - " + Deceased,
        Author = "QuestPdfTest",
        Subject = "Family information establishing the persons entitled to inherit from " + Deceased,
        Keywords = "genealogy, heirs, inheritance, intestate succession, PDF/A-2b",
        Language = "en-US"
    })
    .WithSettings(new DocumentSettings
    {
        PDFA_Conformance = PDFA_Conformance.PDFA_2B
    });

    static void Title(IContainer c) => c.Column(col =>
    {
        col.Item().Text("Genealogical Report").FontSize(26).SemiBold().FontColor(Accent);
        col.Item().Text("Determination of persons entitled to inherit").FontSize(14).FontColor(Colors.Grey.Darken2);
        col.Item().PaddingTop(6).Text(x =>
        {
            x.Span("Estate of: ").SemiBold();
            x.Span(Deceased);
            x.Span("     Report no.: ").SemiBold();
            x.Span("GR-2026-0187");
            x.Span("     Issued: ").SemiBold();
            x.Span("14 Oct 2026");
        });
    });

    static void SectionHeading(ColumnDescriptor col, string number, string text) =>
        col.Item().PaddingBottom(2).BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten1)
            .Text($"{number}  {text}").FontSize(13).SemiBold().FontColor(Accent);

    static void Purpose(IContainer c) => c.Column(col =>
    {
        col.Spacing(4);
        SectionHeading(col, "1.", "Purpose and scope");
        col.Item().Text(
            "This report establishes the family relationships of the deceased and identifies, on the basis of the " +
            "documents listed in section 6, the persons who are entitled to inherit and the share to which each is entitled. " +
            "No will or other testamentary disposition was found, so the estate passes by intestate succession.");
    });

    static void DeceasedDetails(IContainer c) => c.Column(col =>
    {
        col.Spacing(4);
        SectionHeading(col, "2.", "The deceased");
        col.Item().Table(t =>
        {
            t.ColumnsDefinition(cd => { cd.ConstantColumn(130); cd.RelativeColumn(); });
            void Row(string k, string v)
            {
                t.Cell().Background(Colors.Grey.Lighten4).Padding(4).Text(k).SemiBold();
                t.Cell().BorderBottom(0.25f).BorderColor(Colors.Grey.Lighten2).Padding(4).Text(v);
            }
            Row("Full name", Deceased);
            Row("Date and place of birth", "5 May 1948, Uppsala");
            Row("Date and place of death", "14 Aug 2026, Uppsala");
            Row("Last residence", "Kungsgatan 12, 753 21 Uppsala");
            Row("Civil status at death", "Married (to Margareta Lindqvist, since 8 Jun 1974)");
            Row("Testamentary disposition", "None found (intestate)");
        });
    });

    static void HeirsSummary(IContainer c) => c.Column(col =>
    {
        col.Spacing(4);
        SectionHeading(col, "3.", "Persons entitled to inherit");
        col.Item().Table(t =>
        {
            t.ColumnsDefinition(cd =>
            {
                cd.RelativeColumn(2.6f);
                cd.RelativeColumn(1.4f);
                cd.RelativeColumn(2.3f);
                cd.RelativeColumn(2.3f);
                cd.RelativeColumn(1.6f);
            });
            t.Header(h =>
            {
                foreach (var title in new[] { "Name", "Born", "Relationship", "Basis", "Share" })
                    h.Cell().Background(Accent).Padding(4).Text(title).SemiBold().FontColor(Colors.White);
            });
            var i = 0;
            foreach (var heir in Heirs)
            {
                var bg = i++ % 2 == 0 ? Colors.White : Colors.Grey.Lighten4;
                IContainer Cell() => t.Cell().Background(bg).BorderBottom(0.25f).BorderColor(Colors.Grey.Lighten2).Padding(4);
                Cell().Text(heir.Name).SemiBold();
                Cell().Text($"{heir.Born}\n{heir.Status}");
                Cell().Text(heir.Relationship);
                Cell().Text(heir.Basis);
                Cell().Text(heir.Share).SemiBold();
            }
            t.Cell().ColumnSpan(4).Padding(4).AlignRight().Text("Total").SemiBold();
            t.Cell().Padding(4).Text("12/12 (100%)").SemiBold();
        });
        col.Item().PaddingTop(2).Text(
            "Calculation: the surviving spouse takes one third of the estate. The remaining two thirds are divided " +
            "equally among the deceased's children, one share per child (four shares of 1/6). The share of Johan, who " +
            "predeceased, passes to his children Lena and Oskar by right of representation (1/12 each).")
            .FontSize(9).Italic();
    });

    static void FamilyTree(IContainer c) => c.Column(col =>
    {
        col.Spacing(4);
        SectionHeading(col, "4.", "Family tree (descendants and spouse)");
        col.Item().Table(t =>
        {
            t.ColumnsDefinition(cd =>
            {
                cd.RelativeColumn(3);
                cd.RelativeColumn(1.3f);
                cd.RelativeColumn(4);
            });
            t.Header(h =>
            {
                foreach (var title in new[] { "Person", "Life", "Relationship / note" })
                    h.Cell().Background(Accent).Padding(4).Text(title).SemiBold().FontColor(Colors.White);
            });
            foreach (var n in Tree)
            {
                IContainer Cell() => t.Cell().BorderBottom(0.25f).BorderColor(Colors.Grey.Lighten2).Padding(4);
                Cell().PaddingLeft(n.Depth * 16).Text(x =>
                {
                    if (n.Depth > 0) x.Span("- ");
                    var s = x.Span(n.Name);
                    if (n.Depth == 0) s.SemiBold();
                });
                Cell().Text(n.Lifespan);
                Cell().Text(n.Note);
            }
        });
    });

    static void ExcludedSection(IContainer c) => c.Column(col =>
    {
        col.Spacing(4);
        SectionHeading(col, "5.", "Persons examined and found not entitled");
        col.Item().Table(t =>
        {
            t.ColumnsDefinition(cd =>
            {
                cd.RelativeColumn(2.4f);
                cd.RelativeColumn(2.6f);
                cd.RelativeColumn(4);
            });
            t.Header(h =>
            {
                foreach (var title in new[] { "Name", "Relationship", "Reason for exclusion" })
                    h.Cell().Background(Accent).Padding(4).Text(title).SemiBold().FontColor(Colors.White);
            });
            foreach (var e in ExcludedPersons)
            {
                IContainer Cell() => t.Cell().BorderBottom(0.25f).BorderColor(Colors.Grey.Lighten2).Padding(4);
                Cell().Text(e.Name);
                Cell().Text(e.Relationship);
                Cell().Text(e.Reason);
            }
        });
    });

    static void SourcesSection(IContainer c) => c.Column(col =>
    {
        col.Spacing(4);
        SectionHeading(col, "6.", "Documents examined");
        col.Item().Table(t =>
        {
            t.ColumnsDefinition(cd =>
            {
                cd.RelativeColumn(4);
                cd.RelativeColumn(3);
                cd.RelativeColumn(1.5f);
            });
            t.Header(h =>
            {
                foreach (var title in new[] { "Document", "Issuing authority", "Date" })
                    h.Cell().Background(Accent).Padding(4).Text(title).SemiBold().FontColor(Colors.White);
            });
            foreach (var (doc, issuer, date) in Sources)
            {
                IContainer Cell() => t.Cell().BorderBottom(0.25f).BorderColor(Colors.Grey.Lighten2).Padding(4);
                Cell().Text(doc);
                Cell().Text(issuer);
                Cell().Text(date);
            }
        });
    });

    static void Basis(IContainer c) => c.Column(col =>
    {
        col.Spacing(4);
        SectionHeading(col, "7.", "Rules applied");
        col.Item().Text(
            "The following illustrative rules of intestate succession were applied. They are simplified and do not " +
            "represent the law of any particular jurisdiction.");
        col.Item().PaddingLeft(10).Column(list =>
        {
            list.Spacing(2);
            foreach (var rule in new[]
            {
                "A surviving spouse inherits one third of the estate when descendants survive.",
                "Descendants inherit the remainder in equal shares per child (per stirpes).",
                "A child who predeceased the deceased is represented by his or her own descendants.",
                "Children born outside marriage have the same rights as children of the marriage once parentage is established.",
                "A former spouse whose marriage was dissolved before death has no right to inherit.",
                "Ascendants and collateral relatives inherit only if there are no descendants and no surviving spouse.",
            })
                list.Item().Text(x =>
                {
                    x.Span("- ");
                    x.Span(rule);
                });
        });
    });

    static void Certification(IContainer c) => c.ShowEntire().Column(col =>
    {
        col.Spacing(6);
        SectionHeading(col, "8.", "Certification");
        col.Item().Text(
            "I certify that the above has been compiled from the documents listed in section 6 and, to the best of my " +
            "knowledge, correctly identifies all persons entitled to inherit from the estate of " + Deceased + ".");
        col.Item().PaddingTop(24).Row(row =>
        {
            row.RelativeItem().Column(sig =>
            {
                sig.Item().LineHorizontal(0.5f);
                sig.Item().PaddingTop(2).Text("Genealogist: Dr. Elsa Nordin").FontSize(9);
                sig.Item().Text("Accredited genealogist, reg. no. 4471").FontSize(9).FontColor(Colors.Grey.Darken1);
            });
            row.ConstantItem(40);
            row.RelativeItem().Column(sig =>
            {
                sig.Item().LineHorizontal(0.5f);
                sig.Item().PaddingTop(2).Text("Date and place").FontSize(9);
                sig.Item().Text("14 Oct 2026, Uppsala").FontSize(9).FontColor(Colors.Grey.Darken1);
            });
        });
    });
}
