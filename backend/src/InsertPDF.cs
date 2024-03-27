using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace backend;

/// <summary>
/// InsertPDF class
/// </summary>
public class InsertPDF : EditPDF
{
    public InsertPDF(string? filePath = null) : base(filePath) {}

    public static string Insert(string mainFilePath, string insertFilePath, int pageNumber)
    {   
        // Uses Split and Merge PDF Classes
        var splittedFilePaths = SplitPDF.Split(mainFilePath, pageNumber);   
        string[] filesToBeMerged = {splittedFilePaths[0], insertFilePath, splittedFilePaths[1]};

        var localPath = Path.GetDirectoryName(mainFilePath);
        var outputFileName = Path.GetFileNameWithoutExtension(mainFilePath) + "_inserted.pdf";
        var outputFilePath_Intermediate = Path.Combine(localPath, outputFileName);

        var outputFilePath = MergePDF.Merge(filesToBeMerged, outputFilePath_Intermediate);
        
        foreach (var file in splittedFilePaths)
        {
            File.Delete(file);
        }
        return outputFilePath;
    }

}