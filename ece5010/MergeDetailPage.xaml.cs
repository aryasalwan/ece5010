using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
namespace ece5010;
public partial class MergeDetailPage : ContentPage
{
    public MergeDetailPage()
    {
        InitializeComponent();
    }
        private async void OpenFilesButtonClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Select PDF",
                FileTypes = FilePickerFileType.Pdf
            });
            if (result != null)
            {
                var filePaths = result.Select(file => file.FullPath).ToArray();
                var MergedFilePath= await Merge(filePaths);
            if (!string.IsNullOrEmpty(MergedFilePath))
            {
                await Launcher.OpenAsync(new OpenFileRequest{
                    File = new ReadOnlyFile(MergedFilePath),
                        Title = "Open Merged File"
                });
            }
            }
        }
    async Task<string> Merge(string[] pdfFiles)
    {
        // Paths to the PDF files you want to merge

        // Create a new PDF document
        PdfSharp.Pdf.PdfDocument outputDocument = new PdfSharp.Pdf.PdfDocument();

        foreach (string pdfFile in pdfFiles)
        {
            // Open each PDF file
            PdfSharp.Pdf.PdfDocument inputDocument = PdfReader.Open(pdfFile, PdfDocumentOpenMode.Import);

            // Iterate through each page of the input document and add it to the output document
            int pageCount = inputDocument.PageCount;
            for (int pageIndex = 0; pageIndex < pageCount; pageIndex++)
            {
                PdfPage page = inputDocument.Pages[pageIndex];
                outputDocument.AddPage(page);
            }
        }

        // Save the merged document to a file
        string fileName = "merged.pdf";
        string localPath = "C:\\Users\\DELL\\Documents";
        string fullPath=Path.Combine(localPath, fileName);

        await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + fullPath,"OK");
        outputDocument.Save(fullPath);
        return fullPath;
    }

}
