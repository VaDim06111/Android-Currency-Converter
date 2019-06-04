using PCLStorage;
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
    public partial class AddZametka : ContentPage
    {
        public int count = 1;
        public int mascount = 0;
        string[] masIdName = new string[20];
        string[] masIdCount = new string[20];
        string[] masIdCost = new string[20];
        string[] masIdLayout = new string[20];
        public AddZametka(string _name, string _date)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LabelName.Text = _name;
            LabelDate.Text = _date;
            ReadFromFileZametka();
        }

        private void ButtonNewField_Clicked(object sender, EventArgs e)
        {
            CreateNewField();
        }

        public void CreateNewField()
        {
            
            var NewStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 10 };
            masIdLayout[mascount] = NewStackLayout.Id.ToString();
            var EditorName = new Editor() { Text = "Товар " + count, FontSize = 30, WidthRequest = 180, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
            masIdName[mascount] = EditorName.Id.ToString();
            var EditorCount = new Editor() { Text = "1", FontSize = 30, WidthRequest = 70, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
            masIdCount[mascount] = EditorCount.Id.ToString();
            var EditorCost = new Editor() { Text = "10", FontSize = 30, WidthRequest = 85, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
            masIdCost[mascount] = EditorCost.Id.ToString();
            NewStackLayout.Children.Add(EditorName);
            NewStackLayout.Children.Add(EditorCount);
            NewStackLayout.Children.Add(EditorCost);
            ZametkiStackLayout.Children.Add(NewStackLayout);
            count++;
            mascount++;
        }
        public void CreateNewFieldFromFile(string mas1, string mas2, string mas3)
        {

            var NewStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 10 };
            masIdLayout[mascount] = NewStackLayout.Id.ToString();
            var EditorName = new Editor() { Text = mas1, FontSize = 30, WidthRequest = 180, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
            masIdName[mascount] = EditorName.Id.ToString();
            var EditorCount = new Editor() { Text = mas2, FontSize = 30, WidthRequest = 70, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
            masIdCount[mascount] = EditorCount.Id.ToString();
            var EditorCost = new Editor() { Text = mas3, FontSize = 30, WidthRequest = 85, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
            masIdCost[mascount] = EditorCost.Id.ToString();
            NewStackLayout.Children.Add(EditorName);
            NewStackLayout.Children.Add(EditorCount);
            NewStackLayout.Children.Add(EditorCost);
            ZametkiStackLayout.Children.Add(NewStackLayout);            
            mascount++;
        }

        private void ButtonSaveZametka_Clicked(object sender, EventArgs e)
        {
            string[] mas = new string[20];
                     
            for (int i = 0; i < mascount; i++)
            {
                var layout = (ZametkiStackLayout.Children.Where(x => x.Id.ToString() == masIdLayout[i])).FirstOrDefault() as StackLayout;
                var name = (layout.Children.Where(y => y.Id.ToString() == masIdName[i]).FirstOrDefault() as Editor).Text;
                var count = (layout.Children.Where(y => y.Id.ToString() == masIdCount[i]).FirstOrDefault() as Editor).Text;
                var cost = (layout.Children.Where(y => y.Id.ToString() == masIdCost[i]).FirstOrDefault() as Editor).Text;
                mas[i] ="|"+DateTime.Now.ToShortDateString() + "|" + name + "|" + count + "|" + cost+"|";               
            }
            GetFileZametka(mas);
            
            
            Navigation.PopAsync();
        }
        public async void GetFileZametka(string[] mas)
        {
            string txtfile = null;
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder ZametkaFolder = await rootFolder.CreateFolderAsync("Zametki", CreationCollisionOption.OpenIfExists);
            IFile zametkaFile = await ZametkaFolder.GetFileAsync(LabelName.Text + ".xml");
            for (int i = 0; i < mascount; i++)
            {
                txtfile += mas[i];
            }
            await zametkaFile.WriteAllTextAsync(txtfile);
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
            if (mas !=null)
            {
                for (int i = 1; i < mas.Length; i += 4)
                {
                    CreateNewFieldFromFile(mas[i], mas[i + 1], mas[i + 2]);
                }
            }
            else
            {
                CreateNewField();
            }
            
            
        }
    }


}