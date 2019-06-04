using PCLStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Конвертер
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProsmotrZametka : ContentPage
    {
        public ProsmotrZametka(string _name)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                LabelName.Text = _name;
                ReadFromFileZametka();
            }
            catch(Exception ex)
            {
                DisplayAlert("Ошибка",ex.Message,"OK");
            }
        }
        public async void ReadFromFileZametka()
        {
            string txtfile = null;
            string[] mas = new string[20];
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder ZametkaFolder = await rootFolder.CreateFolderAsync("Zametki", CreationCollisionOption.OpenIfExists);
            IFile zametkaFile = await ZametkaFolder.GetFileAsync(LabelName.Text + ".xml");
            txtfile = await zametkaFile.ReadAllTextAsync();
            mas = txtfile.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            LabelDate.Text = mas[0];
            for (int i = 1; i < mas.Length; i+=4)
            {
                var NewStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 10 };
                var LabelName = new Label() { Text = mas[i], FontSize = 30, WidthRequest = 230, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
                var LabelCount = new Label() { Text = mas[i+1], FontSize = 30, WidthRequest = 70, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
                var LabelCost = new Label() { Text = mas[i+2], FontSize = 30, WidthRequest = 90, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
                NewStackLayout.Children.Add(LabelName);
                NewStackLayout.Children.Add(LabelCount);
                NewStackLayout.Children.Add(LabelCost);
                ZametkiStackLayout.Children.Add(NewStackLayout);
            }
        }
    }
}