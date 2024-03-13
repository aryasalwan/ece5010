using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
namespace ece5010;
public partial class SplitDetailPage : ContentPage
{
    private string[] selectedFilePaths;
    private int pageNumber=0;
    private string file_name;
    public SplitDetailPage()
    {
        InitializeComponent();
    }

    private async void OpenFilesButtonClicked(object sender, EventArgs e)
    {
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select PDF",
            FileTypes = FilePickerFileType.Pdf
        });
        if (result != null)
        {
            file_name = result.FileName;
            file_name = Path.GetFileNameWithoutExtension(file_name);
            selectedFilePaths = new string[] { result.FullPath };
            // Optionally, inform the user that files have been selected successfully
            await DisplayAlert("Files Selected", $"You have selected {selectedFilePaths.Length} file(s).", "OK");
        }
    }

    private async void SplitFilesButtonClicked(object sender, EventArgs e)
    {
        if (selectedFilePaths == null || !selectedFilePaths.Any())
        {
            await DisplayAlert("Error", "No PDF file selected. Please select a file before splitting.", "OK");
            return;
        }

        string[] splitFilePaths = await Split(selectedFilePaths);
        if (splitFilePaths != null && splitFilePaths.Length == 2)
        {
            if (!string.IsNullOrEmpty(splitFilePaths[0]) && !string.IsNullOrEmpty(splitFilePaths[1]))
            {
                // Display success message or open the split files as needed
                //await DisplayAlert("Success", "The PDF file has been successfully split.", "OK");

                // Example of opening the first split file
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(splitFilePaths[0]),
                    Title = "Open First Part"
                });

                // Optionally, open the second split file
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(splitFilePaths[1]),
                    Title = "Open Second Part"
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
            // If parsing fails, revert to the last valid pageNumber
            // Optionally, you could also display a brief error message to the user explaining why their input was invalid
            ((Entry)sender).Text = pageNumber > 0 ? pageNumber.ToString() : "";
        }
        else
        {
            // Update pageNumber with the new, successfully parsed value
            pageNumber = newPageNumber;
        }
    }

    async Task<string[]> Split(string[] pdfFiles)
        {
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

         for (int pageIndex2 = pageNumber; pageIndex2 < inputDocument.PageCount; pageIndex2++)
         {
             PdfPage page = inputDocument.Pages[pageIndex2];
             outputDocument2.AddPage(page);
         }
         outputDocument2.Version = inputDocument.Version;
         outputDocument2.Info.Title = "Split2";
         outputDocument2.Info.Creator = inputDocument.Info.Creator;
         // Save the merged document to a file
         string fileName = file_name + "_" + "split1.pdf";
         string localPath = "C:\\Users\\DELL\\Documents";
         string fullPath = Path.Combine(localPath, fileName);

         await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + fullPath, "OK");
         outputDocument1.Save(fullPath);
         string fileName2 = file_name + "_" + "split2.pdf";
         string fullPath2 = Path.Combine(localPath, fileName2);

         await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + fullPath2, "OK");
         outputDocument2.Save(fullPath2);
        string[] SplitFilePaths=[];
        SplitFilePaths.Append(fullPath);
        SplitFilePaths.Append(fullPath2);
        return SplitFilePaths;

       }

     }
    