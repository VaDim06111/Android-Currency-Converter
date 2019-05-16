using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Конвертер
{
    public class MainViewModel
    {
        private Zametki _oldZametki;
        int index;      
        public ObservableCollection<Zametki> mZametki { get; set; }

        public MainViewModel()
        {
            mZametki = new ObservableCollection<Zametki>
            {
                new Zametki
                {
                    Title = "Новая заметка",
                    Description = "Описание",
                    IsVisible = false
                }               
            };

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
                if (_oldZametki != null && index !=-1)
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
    }
}
