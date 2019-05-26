using PCLStorage;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Конвертер
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupView : PopupPage
    {
        public string name { get; set; }
        public string description { get; set; }
       public MainViewModel viewmodel { get; set; }
        public PopupView(MainViewModel vm)
        {
            InitializeComponent();
            viewmodel = vm;
        }

        private void ButtonPopup_add_Clicked(object sender, EventArgs e)
        {
            
            name = Entry_name.Text;
            description = Entry_description.Text;
            PopupNavigation.Instance.PopAsync(true);
            viewmodel.AddZametka(name,description);
            CreateFileZametka(name);
        }
        public async void CreateFileZametka(string name)
        {
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder ZametkaFolder = await rootFolder.CreateFolderAsync("Zametki", CreationCollisionOption.OpenIfExists);
            IFile zametkaFile = await ZametkaFolder.GetFileAsync(name + ".xml");
        }
    }
}