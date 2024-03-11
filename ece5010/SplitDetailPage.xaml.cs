using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
namespace ece5010;
public partial class SplitDetailPage : ContentPage
{
    private int pageNumber=0;
    public SplitDetailPage()
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
            string[] SplitFilePath = await Split(filePaths);
            if (SplitFilePath != null && SplitFilePath.Length >= 2)
            {
                if (!string.IsNullOrEmpty(SplitFilePath[0]))
                {
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(SplitFilePath[0]),
                        Title = "Open Split1"
                    });
                }
                else
                {
                    return;
                }
                if (!string.IsNullOrEmpty(SplitFilePath[1]))
                {
                    await Launcher.OpenAsync(new OpenFileRequest
                    {
                        File = new ReadOnlyFile(SplitFilePath[1]),
                        Title = "Open Split2"
                    });
                }
                else
                {
                    return;
                }
            }
            else
            {
                // Handle the case when SplitFilePath is null or doesn't have enough elements
                return;
            }
        }
    }

    public void OnPageNumberEntered(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(e.NewTextValue, out pageNumber))
            {
                ((Entry)sender).Text = e.OldTextValue;
                return;
            }
            if(!int.TryParse(e.NewTextValue,out pageNumber))
        {
            ((Entry)sender).Text=e.OldTextValue;
        }
        }
        async Task<string[]> Split(string[] pdfFiles)
            {
        // Paths to the PDF files you want to merge

        // Create a new PDF document
        if (pageNumber <= 0 || pdfFiles == null || pdfFiles.Length == 0)
        {
            // Adjust the message according to your needs
            await DisplayAlert("Error", "Please select a valid page number and PDF file before splitting.", "OK");
            return null;
        }
        

         PdfDocument inputDocument = PdfReader.Open(pdfFiles[0], PdfDocumentOpenMode.Import);

         if (pageNumber >= inputDocument.PageCount)
         {
             await DisplayAlert("Error", "Page number exceeds the total number of pages in the document.", "OK");
             return null;
         }

         PdfSharp.Pdf.PdfDocument outputDocument1 = new PdfSharp.Pdf.PdfDocument();

         outputDocument1.Version = inputDocument.Version;
         outputDocument1.Info.Title = "Split1";
         outputDocument1.Info.Creator = inputDocument.Info.Creator;
         for (int pageIndex = 0; pageIndex < pageNumber; pageIndex++)
         {
             PdfPage page = inputDocument.Pages[pageIndex];
             outputDocument1.AddPage(page);
         }
         PdfSharp.Pdf.PdfDocument outputDocument2 = new PdfSharp.Pdf.PdfDocument();

         for (int pageIndex2 = pageNumber + 1; pageIndex2 < inputDocument.PageCount; pageIndex2++)
         {
             PdfPage page = inputDocument.Pages[pageIndex2];
             outputDocument2.AddPage(page);
         }
         outputDocument2.Version = inputDocument.Version;
         outputDocument2.Info.Title = "Split2";
         outputDocument2.Info.Creator = inputDocument.Info.Creator;
         // Save the merged document to a file
         string fileName = "split1.pdf";
         string localPath = "C:\\Users\\DELL\\Documents";
         string fullPath = Path.Combine(localPath, fileName);

         await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + fullPath, "OK");
         outputDocument1.Save(fullPath);
         string fileName2 = "split2.pdf";
         string fullPath2 = Path.Combine(localPath, fileName2);

         await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + fullPath2, "OK");
         outputDocument2.Save(fullPath2);
        string[] SplitFilePaths=[];
        SplitFilePaths.Append(fullPath);
        SplitFilePaths.Append(fullPath2);
        return SplitFilePaths;

       }

     }
    