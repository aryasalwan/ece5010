using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using backend;

namespace ece5010;
public partial class MergeDetailPage : ContentPage
{
    string[] filePaths;
    private string file_name;
    private string[] selectedFilePaths_to_merge;
    private string merge_files_string;
    private string directory_path;
    private string file_name_no_extension;
    public MergeDetailPage()
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
        
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Select PDF",
                FileTypes = FilePickerFileType.Pdf

            });
        foreach (var r in result)
        {
            file_name = r.FileName;
            file_name_no_extension = Path.GetFileNameWithoutExtension(file_name);
            selectedFilePaths_to_merge=new string[] {r.FileName};
            merge_files_string=merge_files_string +file_name + " \n ";
            // Optionally, inform the user that files have been selected successful
        }
        if (result != null)
        {
            filePaths = result.Select(file => file.FullPath).ToArray();
        }
        PdfWebViewSource = "file:///" + filePaths[0];
        await DisplayAlert("Files Selected", $"You have selected the following {filePaths.Length} file(s). \n" +
    merge_files_string, "OK");

    }
    private async void MergeFilesButtonClicked(object sender, EventArgs e)
    {
        if (selectedFilePaths_to_merge == null || !selectedFilePaths_to_merge.Any())
        {
            await DisplayAlert("Error", "No PDF file selected. Please select a file before merging.", "OK");
            return;
        }

        // Merge the PDF files
        string[] MergeFilePaths = await Merge(filePaths);

        // Check if the Merge operation was successful
        if (MergeFilePaths != null)
        {
            if (!string.IsNullOrEmpty(MergeFilePaths[0]))
            {

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(MergeFilePaths[0]),
                    Title = "Open Merged File"
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
    
    async Task<string[]> Merge(string[] pdfFiles)
    {
        // Call backend to merge files
        var outputPath = MergePDF.Merge(pdfFiles);

        string[] fullPath = [outputPath];

        await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + outputPath, "OK");
        return fullPath;
    }

}
