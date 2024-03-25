using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
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
            await DisplayAlert("Error", "No PDF file selected. Please select a file before splitting.", "OK");
            return;
        }

        // Split the PDF files
        string[] MergeFilePaths = await Merge(filePaths);

        // Check if the split operation was successful
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
        file_name = file_name_no_extension + "_merged.pdf";
        string localPath = Path.GetDirectoryName(pdfFiles[0]); ;
        string[] fullPath=new string[1];
        fullPath[0]=Path.Combine(localPath, file_name);
        outputDocument.Save(fullPath[0]);

        await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + fullPath[0],"OK");
        return fullPath;
    }

}
