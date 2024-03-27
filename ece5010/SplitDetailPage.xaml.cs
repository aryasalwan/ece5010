using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using backend;
namespace ece5010;
public partial class SplitDetailPage : ContentPage
{
    private string[] selectedFilePaths;
    private int pageNumber=0;
    private string file_name_ex;
    private string directory_path;
    private string file_name;
    private string file_name_no_extension;
    public SplitDetailPage()
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
            await DisplayAlert("Files Selected", $"You have selected " +file_name_ex, "OK");

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
                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(splitFilePaths[0]),
                    Title = "Open First Part"
                });


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

    async Task<string[]> Split(string[] pdfFiles)
    {
        if (pageNumber <= 0 || pdfFiles == null || pdfFiles.Length == 0)
        {
            await DisplayAlert("Error", "Please select a valid page number and PDF file before splitting.", "OK");
            return null;
        }

         PdfDocument inputDocument = PdfReader.Open(pdfFiles[0], PdfDocumentOpenMode.Import);

         if (pageNumber >= inputDocument.PageCount)
         {
            await DisplayAlert("Error", "Page number exceeds the total number of pages in the document.", "OK");
            return null;
         }

        // Call backend to Split the File
        var outputPaths = SplitPDF.Split(pdfFiles[0], pageNumber);

        await DisplayAlert("Done", "Your Files have been Split. You can find the file at " + outputPaths[0], "OK");
        await DisplayAlert("Done", "Your Files have been Split. You can find the file at " + outputPaths[1], "OK");
        return outputPaths;
    }

}
    