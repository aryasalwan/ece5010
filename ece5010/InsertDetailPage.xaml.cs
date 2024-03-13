using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
namespace ece5010;
public partial class InsertDetailPage : ContentPage
{
    private string[] InsertFilePaths = new string[2];
    private int pageNumber = 0;
    private string main_file_name;
    private string insert_file_name;
    public InsertDetailPage()
    {
        InitializeComponent();
    }

    private async void OpenFilesButtonClicked1(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select PDF",
            FileTypes = FilePickerFileType.Pdf
        });
        if (result != null)
        {
            main_file_name = result.FileName;
            main_file_name = Path.GetFileNameWithoutExtension(main_file_name);
            InsertFilePaths[0] = result.FullPath;
            // Optionally, inform the user that files have been selected successfully
            await DisplayAlert("Files Selected", $"You have selected {main_file_name} file(s).", "OK");
        }
    }
    private async void OpenFilesButtonClicked2(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select PDF",
            FileTypes = FilePickerFileType.Pdf
        });
        if (result != null)
        {
            insert_file_name = result.FileName;
            insert_file_name = Path.GetFileNameWithoutExtension(insert_file_name);
            InsertFilePaths[1] = result.FullPath;
            await DisplayAlert("Files Selected", $"You have selected {insert_file_name} file(s).", "OK");
        }
    }
    private async void InsertFilesButtonClicked(object sender, EventArgs e)
    {
        if (InsertFilePaths == null || !InsertFilePaths.Any())
        {
            await DisplayAlert("Error", "No PDF file selected. Please select a file before inserting operation can be done.", "OK");
            return;
        }

        // Split the PDF files
        InsertFilePaths = await Insert(InsertFilePaths);

        // Check if the split operation was successful
        if (InsertFilePaths != null && InsertFilePaths.Length == 2)
        {
            if (!string.IsNullOrEmpty(InsertFilePaths[0]))
            {

                await DisplayAlert("Success", "The Insertion Operation has been completed.", "OK");

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(InsertFilePaths[0]),
                    Title = "Open First Part"
                });
            }
            else
            {
                return;
            }
        }
        else
        {
            return;
        }
    }

    public void OnPageNumberEntered(object sender, TextChangedEventArgs e)
    {
        // Check if the new text is empty, allowing the user to clear the entry
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            pageNumber = 0; // Reset pageNumber or handle as needed
            return; // Exit early
        }

        // Try to parse the new text value
        if (!int.TryParse(e.NewTextValue, out int newPageNumber))
        {
            ((Entry)sender).Text = pageNumber > 0 ? pageNumber.ToString() : "";
        }
        else
        {
            // Update pageNumber with the new, successfully parsed value
            pageNumber = newPageNumber;
        }
    }

    async Task<string[]> Insert(string[] pdfFiles)
    {
        using (PdfDocument mainDoc = PdfReader.Open(pdfFiles[0], PdfDocumentOpenMode.Import))
        using (PdfDocument insertDoc = PdfReader.Open(pdfFiles[1], PdfDocumentOpenMode.Import))
        {
            if (pageNumber > mainDoc.PageCount)
            {
                Console.WriteLine("Out of index error.");
                return null;
            }
            else
            {
                // Split first PdfDocument
                PdfDocument splitBeforeDoc = new PdfDocument();
                PdfDocument splitAfterDoc = new PdfDocument();

                for (int pageIndex = 0; pageIndex < pageNumber; pageIndex++)
                {
                    PdfPage page = mainDoc.Pages[pageIndex];
                    splitBeforeDoc.AddPage(page);
                }

                for (int pageIndex = pageNumber; pageIndex < mainDoc.PageCount; pageIndex++)
                {
                    PdfPage page = mainDoc.Pages[pageIndex];
                    splitAfterDoc.AddPage(page);
                }

         
                splitBeforeDoc.Save("C:\\Users\\DELL\\Documents\\Before_pdfsharp.pdf");
                splitAfterDoc.Save("C:\\Users\\DELL\\Documents\\After_pdfsharp.pdf");
                splitBeforeDoc.Dispose();
                splitAfterDoc.Dispose();

                // Now merge all three PdfDocument
                PdfDocument outputDoc = new PdfDocument();
                var splitBeforeDocActual = PdfReader.Open("C:\\Users\\DELL\\Documents\\Before_pdfsharp.pdf", PdfDocumentOpenMode.Import);
                var splitAfterDocActual = PdfReader.Open("C:\\Users\\DELL\\Documents\\After_pdfsharp.pdf", PdfDocumentOpenMode.Import);
                PdfDocument[] pdfArray = { splitBeforeDocActual, insertDoc, splitAfterDocActual };

                foreach (PdfDocument doc in pdfArray)
                {
                    for (int pageIndex = 0; pageIndex < doc.PageCount; pageIndex++)
                    {
                        PdfPage page = doc.Pages[pageIndex];
                        outputDoc.AddPage(page);
                    }
                }
                string fileName = main_file_name + "_" + "inserted.pdf";
                string localPath = "C:\\Users\\DELL\\Documents";
                string fullPath = Path.Combine(localPath, fileName);
                outputDoc.Save(fullPath);
                InsertFilePaths[0] = fullPath;
                InsertFilePaths[1] = null;
                await DisplayAlert("Done", "Insertion Operation is Complete. You can find the file at " + fullPath, "OK");
                splitBeforeDocActual.Dispose();
                splitAfterDocActual.Dispose();
                File.Delete("C:\\Users\\DELL\\Documents\\Before_pdfsharp.pdf");
                File.Delete("C:\\Users\\DELL\\Documents\\After_pdfsharp.pdf");

                return InsertFilePaths;
            }
        }
    }
}
