using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using CommunityToolkit.Maui.Storage;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace ece5010;

public partial class InsertEmptyDetailPage : ContentPage
{
    IFileSaver fileSaver;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private string[] selectedFilePaths;
    private int pageNumber = 0;
    private string file_name_ex;
    private string directory_path;
    private string file_name;
    private string file_name_no_extension;

    public InsertEmptyDetailPage(IFileSaver fileSaver)
    {
        InitializeComponent();
        this.fileSaver = fileSaver;
    }
    private WebViewSource _pdfWebViewSource;
    public WebViewSource PdfWebViewSource
    {
        get => _pdfWebViewSource;
        set
        {
            if (_pdfWebViewSource != value)
            {
                _pdfWebViewSource = value;
                OnPropertyChanged(nameof(PdfWebViewSource)); // Notify UI of change
            }
        }
    }
    private async void OpenFilesButtonClicked(object sender, EventArgs e)
    {
        //var result = await FilePicker.PickAsync(new PickOptions
        //{
        //    PickerTitle = "Select PDF",
        //    FileTypes = FilePickerFileType.Pdf
        //});
        var result = await FilePicker.PickAsync();
        if (result != null)
        {
            var file_ext = Path.GetExtension(result.FullPath);
            Console.WriteLine(file_ext);
            if (file_ext == ".pdf")
            {
                file_name_ex = result.FileName;
                file_name = Path.GetFileNameWithoutExtension(file_name_ex);
                selectedFilePaths = new string[] { result.FullPath };
                // Optionally, inform the user that files have been selected successfully
                PdfWebViewSource = "file:///" + selectedFilePaths[0];
                await DisplayAlert("Files Selected", $"You have selected " + file_name_ex, "OK");
            }
            else if (file_ext != ".pdf")
            {
                await DisplayAlert("Wrong file type selected", "Please select a valid .pdf file \n", "OK");
                return;
            }
        }
        else { return; }
    }
    private async void InsertEmptyButtonClicked(object sender, EventArgs e)
    {
        if (selectedFilePaths == null || !selectedFilePaths.Any())
        {
            await DisplayAlert("Error", "No PDF file selected. Please select a file before splitting.", "OK");
            return;
        }
        await InsertEmptyPage(selectedFilePaths);
        //string[] FilePaths = await InsertEmptyPage(selectedFilePaths);
        //if (FilePaths != null)
        //{
        //    if (!string.IsNullOrEmpty(FilePaths[0]))
        //    {
        //        // Display success message or open the split files as needed
        //        //await DisplayAlert("Success", "The PDF file has been successfully split.", "OK");

        //        // Example of opening the first split file
        //        await Launcher.OpenAsync(new OpenFileRequest
        //        {
        //            File = new ReadOnlyFile(FilePaths[0]),
        //            Title = "Open File"
        //        });
        //    }
        //    else
        //    {
        //        return;
        //    }
        //}
        //else
        //{
        //    return;
        //}
    }
    public void OnPageNumberEntered(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            pageNumber = 0;
            return;
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
    async Task<string[]> InsertEmptyPage(string[] pdfFiles)
    {
        Console.WriteLine(pageNumber);
        if (pageNumber > 0)
        {
            pageNumber--;
        }
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
        int pageIndex;
        for (pageIndex = 0; pageIndex < pageNumber; pageIndex++)
        {
            PdfPage page = inputDocument.Pages[pageIndex];
            outputDocument1.AddPage(page);
        }
        outputDocument1.AddPage();
        for (pageIndex = pageIndex; pageIndex < inputDocument.PageCount; pageIndex++)
        {
            PdfPage page = inputDocument.Pages[pageIndex];
            outputDocument1.AddPage(page);
        }
        file_name = file_name + "_" + "notes.pdf";
        //string localPath = Path.GetDirectoryName(pdfFiles[0]);
        //string fullPath = Path.Combine(localPath, fileName);
        using (MemoryStream stream = new MemoryStream())
        {
            string fn = "notes.pdf";
            try
            {
                Console.WriteLine(outputDocument1);
                string localPath = Path.GetDirectoryName(pdfFiles[0]);
                outputDocument1.Save(stream, false);
                var path = await fileSaver.SaveAsync(localPath, fn, stream, cancellationTokenSource.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        };
        //outputDocument1.Save(fullPath);
        //await DisplayAlert("Done", "You can find the file at " + fullPath, "OK");

        //string[] SplitFilePaths;
        //SplitFilePaths = new string[] { fullPath };
        //return SplitFilePaths;
        return null;
    }

}