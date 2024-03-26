using System;
using System.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace backend;

/// <summary>
/// The Parent EditPDF Class.
/// Contains general methods (such as opening, deleting, etc)
/// </summary>
public class EditPDF
{
    // Change fields as needed
    private PdfDocument? pdfDoc;
    private string? filePath;

    /// <summary>
    /// Constructor.
    /// Maybe change if open function is changed.
    /// </summary>
    /// <param name="filePath"></param>
    public EditPDF(string? filePath = null)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            Open(filePath);
        }
        else
        {
            Create();
        }
    }

    /// <summary>
    /// Opens a PDF Document internally in Modify mode
    /// TODO for after project: Maybe implement other opening modes 
    /// as either different functions, or as an argument here.
    /// </summary>
    /// <param name="filePath"></param>
    public void Open(string? filePath)
    {
        if (pdfDoc != null)
        {
            pdfDoc.Dispose();
            pdfDoc = null;
        }

        if (filePath != null)
        {
            this.filePath = filePath;
            pdfDoc = PdfReader.Open(filePath, PdfDocumentOpenMode.Modify);
        }
        else Create();
    }

    /// <summary>
    /// Creates an empty PdfDocument
    /// </summary>
    public void Create()
    {
        if (pdfDoc != null)
        {
            pdfDoc.Dispose();
            pdfDoc = null;
        }

        pdfDoc = new PdfDocument();
    }

    /// <summary>
    /// Saves a file to a path
    /// </summary>
    /// <param name="filePath"></param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public void Save(string? filePath = null)
    {
        if (pdfDoc == null)
        {
            throw new InvalidOperationException("No PDF document is currently loaded.");
        }

        if (string.IsNullOrEmpty(filePath))
        {
            if (string.IsNullOrEmpty(this.filePath))
            {
                throw new ArgumentNullException("filePath", "No file path provided for saving PDF.");
            }

            filePath = this.filePath;
        }

        pdfDoc.Save(filePath);
    }

    /// <summary>
    /// Closes a file internally
    /// </summary>
    public void Close()
    {
        if (pdfDoc != null)
        {
            pdfDoc.Dispose();
            pdfDoc = null;
        }
    }

    /// <summary>
    /// Deletes a file, given that the filepath is in the object
    /// </summary>
    public void Delete()
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            File.Delete(filePath);
            filePath = null;
        }
    }

    public void AddPage(PdfPage page)
    {   
        if (pdfDoc == null) Create();
        pdfDoc.AddPage(page);
        
    }

        public void AddPage()
        {   
        if (pdfDoc == null) Create();
        pdfDoc.AddPage();
        
    }
}
