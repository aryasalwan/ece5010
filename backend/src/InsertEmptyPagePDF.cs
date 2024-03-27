using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace backend;

/// <summary>
/// InsertEmptyPagePDF class
/// </summary>
public class InsertEmptyPagePDF : EditPDF
{
    public InsertEmptyPagePDF(string? filePath = null) : base(filePath) {}

    public static string InsertEmptyPage(string mainFilePath, int pageNumber)
    {   
        // Uses Insert Class
        var emptyPageFile = new InsertEmptyPagePDF();
        emptyPageFile.AddPage();

        var localPath = Path.GetDirectoryName(mainFilePath);
        var outputFileName_EmptyPage = Path.GetFileNameWithoutExtension(mainFilePath) + "_empty.pdf";
        var outputFilePath_EmptyPage = Path.Combine(localPath, outputFileName_EmptyPage);
        emptyPageFile.Save(outputFilePath_EmptyPage);

        var outputFilePath = InsertPDF.Insert(mainFilePath, outputFilePath_EmptyPage, pageNumber);

        File.Delete(outputFilePath_EmptyPage);

        return outputFilePath;
    }

}