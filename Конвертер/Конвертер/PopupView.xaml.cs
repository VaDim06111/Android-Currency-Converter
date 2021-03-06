﻿using PCLStorage;
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
            Picker_valuta.Items.Add("USD");
            Picker_valuta.Items.Add("EUR");
            Picker_valuta.Items.Add("BYN");
            Picker_valuta.Items.Add("RUB");
            Picker_valuta.Items.Add("UAH");
            Picker_valuta.Items.Add("ZL");

        }

        private void ButtonPopup_add_Clicked(object sender, EventArgs e)
        {
            
            name = Entry_name.Text;
            description = Entry_description.Text;
            PopupNavigation.Instance.PopAsync(true);
            viewmodel.AddZametka(name,description);
            SaveListView();
            CreateFileZametka(name);           
        }
        public async void CreateFileZametka(string name)
        {
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder ZametkaFolder = await rootFolder.CreateFolderAsync("Zametki", CreationCollisionOption.OpenIfExists);
            IFile zametkaFile = await ZametkaFolder.CreateFileAsync(name + ".xml", CreationCollisionOption.ReplaceExisting);           
            await zametkaFile.WriteAllTextAsync(DateTime.Now.ToShortDateString() + "|" + Picker_valuta.SelectedItem + "|");
        }
        public async void SaveListView()
        {
            try
            {
                string txt = "";
                IFileSystem fileSystem = FileSystem.Current;
                IFolder rootFolder = fileSystem.LocalStorage;
                IFolder ListFolder = await rootFolder.CreateFolderAsync("ListView", CreationCollisionOption.OpenIfExists);
                IFile listFile = await ListFolder.CreateFileAsync("list.xml", CreationCollisionOption.ReplaceExisting);               
                foreach (var item in viewmodel.mZametki)
                {
                    if (item.Description==null)
                    {
                        item.Description = "";
                    }
                    txt += item.Title + "|" + item.Description + "|" + item.IsVisible+"|";
                }
                if (txt != "")
                    await listFile.WriteAllTextAsync(txt);
            }
            catch (Exception)
            {
            }
        }
    }
}