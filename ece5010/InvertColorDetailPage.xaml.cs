using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
namespace ece5010;
public partial class InvertColorDetailPage : ContentPage
{
    private string[] selectedFilePaths;
    private string file_name_ex;
    private string file_name;
    private string file_name_no_extension;
    private string htmlContent;
    private string file_path_wv;
    public InvertColorDetailPage()
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
                OnPropertyChanged(nameof(PdfWebViewSource));
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
        file_path_wv="file:///" + selectedFilePaths[0];

    }
    private async void InvertColorButtonClicked(object sender, EventArgs e)
    {
        htmlContent = $@"
<!DOCTYPE html>
<html lang=""en"">
<head>
  <meta charset=""UTF-8"">
  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
  <title>PDF Viewer</title>
</head>
<body>
<iframe src=""" + file_path_wv + $@"""style=""width:100%; height:100vh; border: none;""></iframe>
<script>
  var cover = document.createElement('div');
  let css = `
      position: fixed;
      pointer-events: none;
      top: 0;
      left: 0;
      width: 100vw;
      height: 100vh;
      background-color: white;
      mix-blend-mode: difference;
      z-index: 1;
  `;
  cover.setAttribute('style', css);
  document.body.appendChild(cover);
</script>
</body>
</html>
";
        
        file_name = file_name_no_extension + "_inverted.html";
        string localPath = Path.GetDirectoryName(selectedFilePaths[0]); ;
        string[] fullPath = new string[1];
        fullPath[0] = Path.Combine(localPath, file_name);
        File.WriteAllTextAsync(fullPath[0], htmlContent);
        PdfWebViewSource = "file:///" + fullPath[0];
        if (fullPath != null)
        {
            if (!string.IsNullOrEmpty(fullPath[0]))
            {

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(fullPath[0]),
                    Title = "Open Inverted File"
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

}


