using PdfSharp.Pdf.IO;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Security;

namespace ece5010;

public partial class SecureDetailPage : ContentPage
{
	public SecureDetailPage()
	{
		InitializeComponent();
	}
    public void OnPasswordShowButtonClicked(object sender, EventArgs e)
    {
        EnterPasssword.IsPassword = !EnterPasssword.IsPassword;
        ((Button)sender).Text = EnterPasssword.IsPassword ? "Show" : "Hide";
    }
    string[] filePaths;
    string password;
    private int pageNumber = 0;
    private string file_name;
    private string[] selectedFilePath_to_secure;
    private string merge_files_string;
    private string directory_path;
    private string file_name_no_extension;
    private WebViewSource _pdfWebViewSource;
    //This is used to display the PDF inside the application
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
        //Async means elect only one file
        var result = await FilePicker.PickMultipleAsync(new PickOptions
        {
            PickerTitle = "Select PDF",
            //Restricting file picker to only pick pdf files 
            FileTypes = FilePickerFileType.Pdf

        });
        foreach (var r in result)
        {
            file_name = r.FileName;
            file_name_no_extension = Path.GetFileNameWithoutExtension(file_name);
            selectedFilePath_to_secure = new string[] { r.FileName };
            merge_files_string = merge_files_string + file_name + " \n ";
        }
        if (result != null)
        {
            filePaths = result.Select(file => file.FullPath).ToArray();
        }
        PdfWebViewSource = "file:///" + filePaths[0];
        await DisplayAlert("Files Selected", $"You have selected the following {filePaths.Length} file(s). \n" +
    merge_files_string, "OK");

    }
    private async void SecureFilesButtonClicked(object sender, EventArgs e)
    {
        if (selectedFilePath_to_secure == null || !selectedFilePath_to_secure.Any())
        {
            await DisplayAlert("Error", "No PDF file selected. Please select a file before securing.", "OK");
            return;
        }

        string[] SecureFilePath = await Secure(filePaths);

        if (SecureFilePath != null)
        {
            if (!string.IsNullOrEmpty(SecureFilePath[0]))
            {

                await Launcher.OpenAsync(new OpenFileRequest
                {
                    File = new ReadOnlyFile(SecureFilePath[0]),
                    Title = "Open Secured File"
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
    public void OnPasswordEntered(object sender, TextChangedEventArgs e)
    {
       
        if (string.IsNullOrEmpty(e.NewTextValue))
        {
            password = ""; // Reset the password
            return; 
        }

        bool isValidPassword = true; 

        if (isValidPassword)
        {
  
            password = e.NewTextValue;
        }
        else
        { 
            ((Entry)sender).Text = "";
        }
    }

    //Keeping the pdfFiles variable still as string[] so that if in the future multiple files need to secured at once, that can be done easily.
    async Task<string[]> Secure(string[] pdfFiles)
    {
        PdfDocument document = PdfReader.Open(pdfFiles[0]);
        PdfSecuritySettings SecuritySettings = document.SecuritySettings;

        if (password =="")
        {
            await DisplayAlert("Done", "Enter a valid password", "OK");
        }
        SecuritySettings.UserPassword = password;
        string[] SecuredFilePath= new string[1]; 
        SecuredFilePath[0]= pdfFiles[0] + "_secured";
        document.Save(SecuredFilePath[0]);

        await DisplayAlert("Done", "Your File has been Secured. You can find the file at " + SecuredFilePath[0], "OK");
        return SecuredFilePath;
    }

}
