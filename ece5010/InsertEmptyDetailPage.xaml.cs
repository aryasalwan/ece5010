using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using backend;

namespace ece5010;

public partial class InsertEmptyDetailPage : ContentPage
{
    private string[] selectedFilePaths;
    private int pageNumber = 0;
    private string file_name_ex;
    private string directory_path;
    private string file_name;
    private string file_name_no_extension;
    public InsertEmptyDetailPage()
	{
		InitializeComponent();
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
        var result = await FilePicker.PickAsync(new PickOptions
        {
            PickerTitle = "Select PDF",
            FileTypes = FilePickerFileType.Pdf
        });
        if (result != null)
        {
            file_name_ex = result.FileName;
            file_name = Path.GetFileNameWithoutExtension(file_name_ex);
            selectedFilePaths = new string[] { result.FullPath };
            PdfWebViewSource = "file:///" + selectedFilePaths[0];
            await DisplayAlert("Files Selected", $"You have selected " + file_name_ex, "OK");
        }
    }
    private async void InsertEmptyButtonClicked(object sender, EventArgs e)
    {
        if (selectedFilePaths == null || !selectedFilePaths.Any())
        {
            await DisplayAlert("Error", "No PDF file selected. Please select a file before splitting.", "OK");
            return;
        }

        string[] FilePaths = await InsertEmptyPage(selectedFilePaths);
        if (FilePaths != null)
        {
            if (!string.IsNullOrEmpty(FilePaths[0]))
            {
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(FilePaths[0]),
                    Title = "Open File"
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

        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            pageNumber = 0;
            return; 
        }

        if (!int.TryParse(e.NewTextValue, out int newPageNumber))
        {
            ((Entry)sender).Text = pageNumber > 0 ? pageNumber.ToString() : "";
        }
        else
        {
            pageNumber = newPageNumber;
        }
    }
    async Task<string[]> InsertEmptyPage(string[] pdfFiles)
    {
        if (pageNumber <= 0 || pdfFiles == null || pdfFiles.Length == 0)
        {
            await DisplayAlert("Error", "Please select a valid page number and PDF file before splitting.", "OK");
            return null;
        }

        PdfDocument mainDoc = PdfReader.Open(pdfFiles[0], PdfDocumentOpenMode.Import);
        if (pageNumber > mainDoc.PageCount)
        {
            Console.WriteLine("Out of index error.");
            return null;
        }

        // Call backend to insert an empty page
        var outputPath = InsertEmptyPagePDF.InsertEmptyPage(pdfFiles[0], pageNumber);

        string[] outputPaths = [outputPath];

        await DisplayAlert("Done", "Insertion Operation is Complete. You can find the file at " + outputPath, "OK");
        return outputPaths;

    }

}