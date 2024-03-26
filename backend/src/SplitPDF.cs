using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace backend;

/// <summary>
/// SplitPDF class for splitting PDFs
/// NOTE: Currently, it only splits into two PDFs.
/// Can be made more general in future.
/// </summary>
public class SplitPDF : EditPDF
{
    /// <summary>
    /// Constructor for SplitPDF
    /// </summary>
    /// <param name="filePath"></param>
    public SplitPDF(string? filePath = null) : base(filePath) {}

    /// <summary>
    /// Split PDF Documents, given a page number to split from.
    /// </summary>
    /// <param name="inputFilePath"></param>
    /// <param name="pageNumber"></param>
    /// <returns></returns>
    public static string[] Split(string inputFilePath, int pageNumber)
    {
        var inputDocument = PdfReader.Open(inputFilePath, PdfDocumentOpenMode.Import);

        var outputDoc0 = new SplitPDF();
        var outputDoc1 = new SplitPDF();

        for (int idx = 0; idx < pageNumber; idx++)
        {
            var page = inputDocument.Pages[idx];
            outputDoc0.AddPage(page);
        }

        for (int idx = pageNumber; idx < inputDocument.PageCount; idx++)
        {
            var page = inputDocument.Pages[idx];
            outputDoc1.AddPage(page);
        }

        var localPath = Path.GetDirectoryName(inputFilePath);
        var outputFileName = Path.GetFileNameWithoutExtension(inputFilePath);

        var outputFileNameDoc0 = outputFileName + "_split1.pdf";
        var outputFileNameDoc1 = outputFileName + "_split2.pdf";

        var outputFilePath0 = Path.Combine(localPath, outputFileNameDoc0);
        var outputFilePath1 = Path.Combine(localPath, outputFileNameDoc1);

        outputDoc0.Save(outputFilePath0);
        outputDoc1.Save(outputFilePath1);

        string[] outputArray = {outputFilePath0, outputFilePath1};
        return outputArray;

    }
}