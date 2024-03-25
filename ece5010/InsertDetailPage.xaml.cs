using ece5010.ViewModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics;
using System.Reflection.PortableExecutable;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
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
            // Optionally, inform the user that files have been selected successfully
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
        using (PdfDocument mainDoc = PdfReader.Open(pdfFiles[0], PdfDocumentOpenMode.Import))
        using (PdfDocument insertDoc = PdfReader.Open(pdfFiles[1], PdfDocumentOpenMode.Import))

        {
            if (pageNumber > mainDoc.PageCount)
            {
                Console.WriteLine("Out of index error.");
                return null;
            }
            else
            {
                // Split first PdfDocument
                PdfDocument splitBeforeDoc = new PdfDocument();
                PdfDocument splitAfterDoc = new PdfDocument();

                for (int pageIndex = 0; pageIndex < pageNumber; pageIndex++)
                {
                    PdfPage page = mainDoc.Pages[pageIndex];
                    splitBeforeDoc.AddPage(page);
                }

                for (int pageIndex = pageNumber; pageIndex < mainDoc.PageCount; pageIndex++)
                {
                    PdfPage page = mainDoc.Pages[pageIndex];
                    splitAfterDoc.AddPage(page);
                }

                string localPath = Path.GetDirectoryName(pdfFiles[0]);
                splitBeforeDoc.Save(localPath + "Before_pdfsharp.pdf");
                splitAfterDoc.Save(localPath+"After_pdfsharp.pdf");
                splitBeforeDoc.Dispose();
                splitAfterDoc.Dispose();

                // Now merge all three PdfDocument
                PdfDocument outputDoc = new PdfDocument();
                var splitBeforeDocActual = PdfReader.Open(localPath+"Before_pdfsharp.pdf", PdfDocumentOpenMode.Import);
                var splitAfterDocActual = PdfReader.Open(localPath+"After_pdfsharp.pdf", PdfDocumentOpenMode.Import);
                PdfDocument[] pdfArray = { splitBeforeDocActual, insertDoc, splitAfterDocActual };

                foreach (PdfDocument doc in pdfArray)
                {
                    for (int pageIndex = 0; pageIndex < doc.PageCount; pageIndex++)
                    {
                        PdfPage page = doc.Pages[pageIndex];
                        outputDoc.AddPage(page);
                    }
                }
                string fileName = main_file_name + "_" + "inserted.pdf";
                
                string fullPath = Path.Combine(localPath, fileName);
                outputDoc.Save(fullPath);
                InsertFilePaths[0] = fullPath;
                InsertFilePaths[1] = null;
                await DisplayAlert("Done", "Insertion Operation is Complete. You can find the file at " + fullPath, "OK");
                splitBeforeDocActual.Dispose();
                splitAfterDocActual.Dispose();
                File.Delete(localPath+"Before_pdfsharp.pdf");
                File.Delete(localPath+ "After_pdfsharp.pdf");

                return InsertFilePaths;
            }
        }
    }
}
