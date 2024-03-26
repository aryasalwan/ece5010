using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace backend;

/// <summary>
/// MergePDF class for merging PDFs
/// </summary>
public class MergePDF : EditPDF
{
    /// <summary>
    /// Constuctor for MergePDF.
    /// Imports from parent EditPDF Class
    /// </summary>
    /// <param name="filePath"></param>
    public MergePDF(string? filePath = null) : base(filePath) {}

    /// <summary>
    /// Merge PDF Documents
    /// </summary>
    /// <param name="inputFilePaths"></param>
    /// <param name="outputFilePath"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static string Merge(string[] inputFilePaths, string? outputFilePath = null)
    {
        // Open the resultant document.
        var outputDocument = new MergePDF();

        // File path validation is handled by frontend for now.
        // Iterate the files.
        foreach (var file in inputFilePaths)
        {
            // Open the document to import pages from it.
            var inputDocument = PdfReader.Open(file, PdfDocumentOpenMode.Import);

            // Iterate over pages.
            var count = inputDocument.PageCount;
            for (var idx = 0; idx < count; idx++)
            {
                // Get the page from the external document 
                // and add it to the output document.
                var page = inputDocument.Pages[idx];
                outputDocument.AddPage(page);
            }
        }

        if (outputFilePath == null)
        {   
            // Default output path.
            var localPath = Path.GetDirectoryName(inputFilePaths[0]);
            var outputFileName = Path.GetFileNameWithoutExtension(inputFilePaths[0]) + "_merged.pdf";
            var localOutputFilePath = Path.Combine(localPath, outputFileName);

            outputDocument.Save(localOutputFilePath);
            return localOutputFilePath;
        } 
        else 
        {
            // Path Validation would be done by frontend
            outputDocument.Save(outputFilePath);
            return outputFilePath;
        }

    }
}
