using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using CommunityToolkit.Maui.Storage;
using System.Text;
namespace ece5010;
public partial class MergeDetailPage : ContentPage
{
    IFileSaver fileSaver;
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    List<string> filePaths = new List<string>();
    private int i = 0;
    private string file_name;
    private string selectedFilePaths_to_merge;
    public string merge_files_string = " ";
    private string directory_path;
    private string file_name_no_extension;


    public MergeDetailPage(IFileSaver fileSaver)
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
                OnPropertyChanged(nameof(PdfWebViewSource));
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
        var file_ext = Path.GetExtension(result.FullPath);
        Console.WriteLine(file_ext);
        if (result != null && file_ext == ".pdf")
        {
            file_name = result.FileName;
            Console.WriteLine(file_name);
            file_name_no_extension = Path.GetFileNameWithoutExtension(file_name);
            selectedFilePaths_to_merge = result.FullPath;
            if (file_name != null) { merge_files_string = merge_files_string + file_name + " \n "; }
            if (filePaths != null)
            {
                filePaths.Add(selectedFilePaths_to_merge);
            }
        }
        else if (result != null && file_ext != ".pdf")
        {
            await DisplayAlert("Wrong file type selected", "Please select a valid .pdf file \n", "OK");
            return;
        }
        else
        {
            return;
        }

        if (selectedFilePaths_to_merge.Length > 0)
        {
            PdfWebViewSource = "file:///" + selectedFilePaths_to_merge;
            await DisplayAlert("Files Selected", $"You have selected the following file(s). \n" +
        merge_files_string, "OK");
        }
        i = i + 1;

    }
    private async void MergeFilesButtonClicked(object sender, EventArgs e)
    {
        if (filePaths.Count < 2)
        {
            await DisplayAlert("Error", "Please select at least 2 files before merging.", "OK");
            return;
        }

        // Merge the PDF files
        await Merge(filePaths);

        //// Check if the Merge operation was successful
        //if (MergeFilePaths != null)
        //{
        //    if (!string.IsNullOrEmpty(MergeFilePaths[0]))
        //    {

        //        await Launcher.OpenAsync(new OpenFileRequest
        //        {
        //            File = new ReadOnlyFile(MergeFilePaths[0]),
        //            Title = "Open Merged File"
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
    async Task<string[]> Merge(List<string> pdfFiles)
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
        file_name = file_name_no_extension + "_merged.pdf";
        using (MemoryStream stream = new MemoryStream())
        {
            string localPath = Path.GetDirectoryName(pdfFiles[0]);
            outputDocument.Save(stream, false);
            var path = await fileSaver.SaveAsync(localPath, file_name, stream, cancellationTokenSource.Token);
        };

        //// Save the merged document to a file
        //string[] fullPath = new string[1];
        //fullPath[0] = Path.Combine(localPath, file_name);
        //outputDocument.Save(fullPath[0]);

        //await DisplayAlert("Done", "Your Files have been Merged. You can find the file at " + fullPath[0], "OK");
        return null;
    }

}
