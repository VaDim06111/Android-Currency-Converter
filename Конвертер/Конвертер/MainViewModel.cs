using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using PCLStorage;

namespace Конвертер
{
    public class MainViewModel
    {
        private Zametki _oldZametki;
        int index;
        string[] masList = new string[] { };
        public ObservableCollection<Zametki> mZametki { get; set; }

        public  MainViewModel()
        {
            mZametki = new ObservableCollection<Zametki>
            {

            };

            GetListFromFile();
        }

        public void HideOrShowProduct(Zametki zametki)
        {
            if (_oldZametki == zametki)
            {
                // Click twice on the same item will hide it
                zametki.IsVisible = !zametki.IsVisible;
                UpdateZametki(zametki);
            }
            else
            {
                index = mZametki.IndexOf(_oldZametki);
                if (_oldZametki != null && index != -1)
                {
                    // hide previous selected item
                    _oldZametki.IsVisible = false;
                    UpdateZametki(_oldZametki);
                }

                //show selected item
                zametki.IsVisible = true;
                UpdateZametki(zametki);

            }

            _oldZametki = zametki;

        }
        public void UpdateZametki(Zametki zametki)
        {
            index = mZametki.IndexOf(zametki);
            mZametki.Remove(zametki);
            mZametki.Insert(index, zametki);

        }
        public void DeleteZametka()
        {
            DeleteFile(mZametki[index].Title);
            mZametki.RemoveAt(index);
        }
        public void AddZametka(string name, string description)
        {
            Zametki mZametki_new =
                new Zametki
                {
                    Title = name,
                    Description = description,
                    IsVisible = false
                };
            mZametki.Add(mZametki_new);
        }
        public async Task ReadFileZametka()
        {
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder zametkaFolder = await rootFolder.CreateFolderAsync("Zametki", CreationCollisionOption.OpenIfExists);
            IFile zametkaFile = await zametkaFolder.GetFileAsync(mZametki[index].Title + ".xml");
            string strZametka = await zametkaFile.ReadAllTextAsync();
            string[] mas_zametka = strZametka.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        }
        public async void DeleteFile(string _name)
        {
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder ZametkaFolder = await rootFolder.CreateFolderAsync("Zametki", CreationCollisionOption.OpenIfExists);
            IFile zametkaFile = await ZametkaFolder.GetFileAsync(_name + ".xml");
            await zametkaFile.DeleteAsync();

        }
        public async Task GetListFromFile()
        {
            string _txt = "";
            string[] _mas = new string[] { };
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder ListFolder = await rootFolder.CreateFolderAsync("ListView", CreationCollisionOption.OpenIfExists);
            IFile listFile = await ListFolder.GetFileAsync("list.xml");
            _txt = await listFile.ReadAllTextAsync();
            _mas = _txt.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            masList = _mas;
            int count = masList.Length / 3;
            Zametki[] zametki = new Zametki[count];
            count = 0;
            int i = 0;
            while (i<zametki.Length)
            {
                AddZametka(masList[count], masList[count + 1]);
                count += 3;
            }            
        }      
    }
}
