using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using backend;

namespace ece5010;
public partial class InsertDetailPage : ContentPage
{
    private string[] InsertFilePaths = new string[2];
    private int pageNumber = 0;
    private string main_file_name;
    private string insert_file_name;
    private string insert_file_name_ex;
    private string main_file_name_ex;
    public InsertDetailPage()
    {
        InitializeComponent();
    }
    private WebViewSource _pdfWebViewSource1;
    public WebViewSource PdfWebViewSource1
    {
        get => _pdfWebViewSource1;
        set
        {
            if (_pdfWebViewSource1 != value)
            {
                _pdfWebViewSource1 = value;
                OnPropertyChanged(nameof(PdfWebViewSource1)); // Notify UI of change
            }
        }
    }

    private WebViewSource _pdfWebViewSource2;
    public WebViewSource PdfWebViewSource2
    {
        get => _pdfWebViewSource2;
        set
        {
            if (_pdfWebViewSource2 != value)
            {
                _pdfWebViewSource2 = value;
                OnPropertyChanged(nameof(PdfWebViewSource2)); // Notify UI of change
            }
        }
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
            main_file_name_ex = result.FileName;
            main_file_name = Path.GetFileNameWithoutExtension(main_file_name_ex);
            InsertFilePaths[0] = result.FullPath;
            PdfWebViewSource1 = "file:///" + InsertFilePaths[0];
            await DisplayAlert("Files Selected", $"You have selected {main_file_name_ex}", "OK");
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
            insert_file_name_ex = result.FileName;
            insert_file_name = Path.GetFileNameWithoutExtension(insert_file_name_ex);
            InsertFilePaths[1] = result.FullPath;
            await DisplayAlert("Files Selected", $"You have selected {insert_file_name_ex}", "OK");
            PdfWebViewSource2 = "file:///" + InsertFilePaths[1];
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

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(InsertFilePaths[0]),
                    Title = "Open FIle"
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
        if (pageNumber <= 0 || pdfFiles == null || pdfFiles.Length == 0)
        {
            // Adjust the message according to your needs
            await DisplayAlert("Error", "Please select a valid page number and PDF file before splitting.", "OK");
            return null;
        }

        PdfDocument mainDoc = PdfReader.Open(pdfFiles[0], PdfDocumentOpenMode.Import);
        if (pageNumber > mainDoc.PageCount)
        {
            Console.WriteLine("Out of index error.");
            return null;
        }

        var outputPath = InsertPDF.Insert(pdfFiles[0], pdfFiles[1], pageNumber);

        string[] outputPaths = [outputPath];

        await DisplayAlert("Done", "Insertion Operation is Complete. You can find the file at " + outputPath, "OK");
        return outputPaths;

    }
}
