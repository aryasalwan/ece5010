using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;

namespace ece5010.ViewModel
{
    public partial class MergeDetailViewModel:ObservableObject
    {
        async void OpenFilesButtonClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickMultipleAsync(new PickOptions
            {
                PickerTitle = "Pick PDF",
                FileTypes = FilePickerFileType.Pdf
            }) ;
            if (result == null)
            {
                return;
            }
        }
    }
}
