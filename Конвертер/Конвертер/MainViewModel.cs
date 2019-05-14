using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Конвертер
{
    public class MainViewModel
    {
        private Zametki _oldZametki;

        public ObservableCollection<Zametki> mZametki { get; set; }

        public MainViewModel()
        {
            mZametki = new ObservableCollection<Zametki>
            {
                new Zametki
                {
                    Title = "Новая заметка 1",
                    Description = "Описание",
                    IsVisible = false
                },
                new Zametki
                {
                    Title = "Новая заметка 2",
                    Description = "Описание",
                    IsVisible = false
                },
                new Zametki
                {
                    Title = "Новая заметка 3",
                    Description = "Описание",
                    IsVisible = false
                },
                new Zametki
                {
                    Title = "Новая заметка 4",
                    Description = "Описание",
                    IsVisible = false
                },
                new Zametki
                {
                    Title = "Новая заметка 5",
                    Description = "Описание",
                    IsVisible = false
                },
                new Zametki
                {
                    Title = "Новая заметка 6",
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
                if (_oldZametki != null)
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
            var index = mZametki.IndexOf(zametki);
            mZametki.Remove(zametki);
            mZametki.Insert(index, zametki);
        }
    }
}
