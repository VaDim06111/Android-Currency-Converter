using BottomBar.XamarinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.IO;
using System.Text.RegularExpressions;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System.Diagnostics;
using System.Xml.Linq;
using PCLStorage;
using Rg.Plugins.Popup.Services;

namespace Конвертер
{
    public partial class MainPage : BottomBarPage
    {
        string[] hm = new string[105];
        Zametki zametki;
        MainViewModel vm;
        int count = 1;
        string Title = "";
        public MainPage()
        {

            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (CheckConnection())
            {
                var web = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                };
                string html1 = "https://myfin.by/currency/pln/grodno";
                HtmlDocument HD1 = new HtmlDocument();
                var web1 = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                };
                HD1 = web.Load(html1);
                HtmlNodeCollection NoAltElements1 = HD1.DocumentNode.SelectNodes("//div[@class='table-responsive']");
                if (NoAltElements1 != null)
                {
                    foreach (HtmlNode HN in NoAltElements1)
                    {
                        hm = HN.InnerText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                //29 30 $
                //41 42 €
                //54 55 100₽
                // 17 18 10zl
                // 66 67 100grivna
                hm[29] = hm[29].Remove(hm[29].Length - 1, 1).Replace(".", ",");
                hm[30] = hm[30].Remove(hm[30].Length - 1, 1).Replace(".", ",");
                hm[41] = hm[41].Remove(hm[41].Length - 1, 1).Replace(".", ",");
                hm[42] = hm[42].Remove(hm[42].Length - 1, 1).Replace(".", ",");
                hm[54] = hm[54].Remove(hm[54].Length - 1, 1).Replace(".", ",");
                hm[55] = hm[55].Remove(hm[55].Length - 1, 1).Replace(".", ",");
                hm[17] = hm[17].Remove(hm[17].Length - 1, 1).Replace(".", ",");
                hm[18] = hm[18].Remove(hm[18].Length - 1, 1).Replace(".", ",");
                hm[66] = hm[66].Remove(hm[66].Length - 1, 1).Replace(".", ",");
                hm[67] = hm[67].Remove(hm[67].Length - 1, 1).Replace(".", ",");
                Create_file();
            }
            else
                ReadFileKurs();
        }

        // обработка изменения состояния подключения
        public void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckConnection();
        }
        // получаем состояние подключения
        public bool CheckConnection()
        {
            bool connection = false;
            if (CrossConnectivity.Current != null &&
                CrossConnectivity.Current.ConnectionTypes != null &&
                CrossConnectivity.Current.IsConnected == true)
            {
                var connectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();
                connection = true;
            }
            return connection;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (CheckConnection())
            {
                var web = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                };
                string html1 = "https://myfin.by/currency/pln/grodno";
                HtmlDocument HD1 = new HtmlDocument();
                var web1 = new HtmlWeb
                {
                    AutoDetectEncoding = false,
                    OverrideEncoding = Encoding.UTF8,
                };
                HD1 = web.Load(html1);
                HtmlNodeCollection NoAltElements1 = HD1.DocumentNode.SelectNodes("//div[@class='table-responsive']");

                if (NoAltElements1 != null)
                {
                    foreach (HtmlNode HN in NoAltElements1)
                    {
                        hm = HN.InnerText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    }
                }
                //29 30 $
                //41 42 €
                //54 55 100₽
                // 17 18 10zl
                // 66 67 100grivna
                hm[29] = hm[29].Remove(hm[29].Length - 1, 1).Replace(".", ",");
                hm[30] = hm[30].Remove(hm[30].Length - 1, 1).Replace(".", ",");
                hm[41] = hm[41].Remove(hm[41].Length - 1, 1).Replace(".", ",");
                hm[42] = hm[42].Remove(hm[42].Length - 1, 1).Replace(".", ",");
                hm[54] = hm[54].Remove(hm[54].Length - 1, 1).Replace(".", ",");
                hm[55] = hm[55].Remove(hm[55].Length - 1, 1).Replace(".", ",");
                hm[17] = hm[17].Remove(hm[17].Length - 1, 1).Replace(".", ",");
                hm[18] = hm[18].Remove(hm[18].Length - 1, 1).Replace(".", ",");
                hm[66] = hm[66].Remove(hm[66].Length - 1, 1).Replace(".", ",");
                hm[67] = hm[67].Remove(hm[67].Length - 1, 1).Replace(".", ",");
                Edit_USD_buy.Text = hm[29];
                Edit_USD_send.Text = hm[30];
                Edit_EUR_buy.Text = hm[41];
                Edit_EUR_send.Text = hm[42];
                Edit_RUS_buy.Text = hm[54];
                Edit_RUS_send.Text = hm[55];
                Edit_ZL_buy.Text = hm[17];
                Edit_ZL_send.Text = hm[18];
                Edit_GR_buy.Text = hm[66];
                Edit_GR_send.Text = hm[67];
                Create_file();
            }
            else
                DisplayAlert("Ошибка", "Отсутствует интернет-соединение!", "ОK");
            Kurs_valut.Opacity = 1;
        }

        private void Kurs_valut_Appearing(object sender, EventArgs e)
        {
            if (hm.Length > 0)
            {
                Edit_USD_buy.Text = hm[29];
                Edit_USD_send.Text = hm[30];
                Edit_EUR_buy.Text = hm[41];
                Edit_EUR_send.Text = hm[42];
                Edit_RUS_buy.Text = hm[54];
                Edit_RUS_send.Text = hm[55];
                Edit_ZL_buy.Text = hm[17];
                Edit_ZL_send.Text = hm[18];
                Edit_GR_buy.Text = hm[66];
                Edit_GR_send.Text = hm[67];
            }
        }

        private void Editor_USD_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Editor_USD.IsFocused)
                if (Editor_USD.Text != "")
                {
                    try
                    {
                        Editor_BYN.Text = Math.Round(double.Parse(Editor_USD.Text.Replace(" ", "")) * Convert.ToDouble(hm[30]), 3).ToString();
                        Editor_EUR.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[42]), 3).ToString();
                        Editor_RUB.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[55]), 3).ToString();
                        Editor_ZL.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 10 / Convert.ToDouble(hm[18]), 3).ToString();
                        Editor_UAH.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[67]), 3).ToString();
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Ошибка", "Введите число!", "ОК");
                        Editor_USD.Text = "";
                    }
                }
        }

        private void Editor_EUR_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Editor_EUR.IsFocused)
                if (Editor_EUR.Text != "")
                {
                    try
                    {
                        double d = Convert.ToDouble(hm[42]);
                        Editor_BYN.Text = Math.Round(double.Parse(Editor_EUR.Text.Replace(" ", "")) * double.Parse(hm[42]), 3).ToString();
                        Editor_USD.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / double.Parse(hm[30]), 3).ToString();
                        Editor_RUB.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / double.Parse(hm[55]), 3).ToString();
                        Editor_ZL.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 10 / Convert.ToDouble(hm[18]), 3).ToString();
                        Editor_UAH.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[67]), 3).ToString();
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Ошибка", "Введите число!", "ОК");
                        Editor_EUR.Text = "";
                    }
                }
        }

        private void Editor_RUB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Editor_RUB.IsFocused)
                if (Editor_RUB.Text != "")
                {
                    try
                    {
                        Editor_BYN.Text = Math.Round(double.Parse(Editor_RUB.Text.Replace(" ", "")) / 100 * Convert.ToDouble(hm[55]), 3).ToString();
                        Editor_USD.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[30]), 3).ToString();
                        Editor_EUR.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[42]), 3).ToString();
                        Editor_ZL.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 10 / Convert.ToDouble(hm[18]), 3).ToString();
                        Editor_UAH.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[67]), 3).ToString();
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Ошибка", "Введите число!", "ОК");
                        Editor_RUB.Text = "";
                    }
                }
        }

        private void Editor_BYN_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Editor_BYN.IsFocused)
                if (Editor_BYN.Text != "")
                {
                    try
                    {
                        Editor_USD.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[30]), 3).ToString();
                        Editor_EUR.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[42]), 3).ToString();
                        Editor_RUB.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[55]), 3).ToString();
                        Editor_ZL.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 10 / Convert.ToDouble(hm[18]), 3).ToString();
                        Editor_UAH.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[67]), 3).ToString();
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Ошибка", "Введите число!", "ОК");
                        Editor_BYN.Text = "";
                    }
                }
        }

        private void Editor_UAH_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Editor_UAH.IsFocused)
                if (Editor_UAH.Text != "")
                {
                    try
                    {
                        Editor_BYN.Text = Math.Round(double.Parse(Editor_UAH.Text.Replace(" ", "")) / 100 * Convert.ToDouble(hm[67]), 3).ToString();
                        Editor_USD.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[30]), 3).ToString();
                        Editor_EUR.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[42]), 3).ToString();
                        Editor_RUB.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[55]), 3).ToString();
                        Editor_ZL.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 10 / Convert.ToDouble(hm[18]), 3).ToString();
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Ошибка", "Введите число!", "ОК");
                        Editor_UAH.Text = "";
                    }
                }
        }

        private void Editor_ZL_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Editor_ZL.IsFocused)
                if (Editor_ZL.Text != "")
                {
                    try
                    {
                        Editor_BYN.Text = Math.Round(double.Parse(Editor_ZL.Text.Replace(" ", "")) / 10 * Convert.ToDouble(hm[18]), 3).ToString();
                        Editor_USD.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[30]), 3).ToString();
                        Editor_EUR.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) / Convert.ToDouble(hm[42]), 3).ToString();
                        Editor_RUB.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[55]), 3).ToString();
                        Editor_UAH.Text = Math.Round(double.Parse(Editor_BYN.Text.Replace(" ", "")) * 100 / Convert.ToDouble(hm[67]), 3).ToString();
                    }
                    catch (Exception)
                    {
                        DisplayAlert("Ошибка", "Введите число!", "ОК");
                        Editor_ZL.Text = "";
                    }
                }
        }

        private void Editor_BYN_Focused(object sender, FocusEventArgs e)
        {
            Editor_BYN.Text = "";
        }

        private void Editor_USD_Focused(object sender, FocusEventArgs e)
        {
            Editor_USD.Text = "";
        }

        private void Editor_EUR_Focused(object sender, FocusEventArgs e)
        {
            Editor_EUR.Text = "";
        }

        private void Editor_RUB_Focused(object sender, FocusEventArgs e)
        {
            Editor_RUB.Text = "";
        }

        private void Editor_UAH_Focused(object sender, FocusEventArgs e)
        {
            Editor_UAH.Text = "";
        }

        private void Editor_ZL_Focused(object sender, FocusEventArgs e)
        {
            Editor_ZL.Text = "";
        }

        private void ImageButton_update_Pressed(object sender, EventArgs e)
        {
            Kurs_valut.Opacity = 0.5;
        }
        //29 30 $
        //41 42 €
        //54 55 100₽
        // 17 18 10zl
        // 66 67 100grivna

        public async Task Create_file()
        {
            //C:\Users\Lenovo\AppData\Local\Packages\f267e075-aaba-416e-a192-95756684f011_4hs5j56ydjrzj\LocalState\Kurs

            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder KursFolder = await rootFolder.CreateFolderAsync("Kurs", CreationCollisionOption.OpenIfExists);
            IFile kursFile = await KursFolder.CreateFileAsync("kurs.xml", CreationCollisionOption.ReplaceExisting);
            kursFile.WriteAllTextAsync(DateTime.Now + " usd " + hm[29] + " " + hm[30] + " eur " + hm[41] + " " + hm[42] + " rub " + hm[54] + " " + hm[55] + " zl " + hm[17] + " " + hm[18] + " gr " + hm[66] + " " + hm[67]);
        }

        public async Task ReadFileKurs()
        {
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder KursFolder = await rootFolder.CreateFolderAsync("Kurs", CreationCollisionOption.OpenIfExists);
            IFile kursFile = await KursFolder.GetFileAsync("kurs.xml");
            string kurs = await kursFile.ReadAllTextAsync();
            string[] mas_kurs = kurs.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            hm[29] = mas_kurs[3];
            hm[30] = mas_kurs[4];
            hm[41] = mas_kurs[6];
            hm[42] = mas_kurs[7];
            hm[54] = mas_kurs[9];
            hm[55] = mas_kurs[10];
            hm[17] = mas_kurs[12];
            hm[18] = mas_kurs[13];
            hm[66] = mas_kurs[15];
            hm[67] = mas_kurs[16];
            hm[0] = mas_kurs[0] + mas_kurs[1];
        }

        public void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                zametki = e.Item as Zametki;
                ListView st = ((ListView)sender);
                vm = st.BindingContext as MainViewModel;
                vm.HideOrShowProduct(zametki);
                Title = zametki.Title;
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "OK");
            }

        }

        private void Button_List_Delete_Clicked(object sender, EventArgs e)
        {
            try
            {              
                vm.DeleteZametka();               
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }
       

        public void ShowPopup(MainViewModel vm)
        {
            PopupNavigation.Instance.PushAsync(new PopupView(vm));      
        }

        private void ImageButton_addZametka_Clicked(object sender, EventArgs e)
        {
            try
            {             
                vm = ListViewZametki.BindingContext as MainViewModel;
                ShowPopup(vm);
                                
            }
            catch (Exception ex)
            {
                DisplayAlert("Ошибка", ex.Message, "OK");
            }
        }      
        private async void Button_List_Prosmotr_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ProsmotrZametka(Title,hm[30], hm[42], hm[55], hm[18], hm[67]));
        }

        private async void Button_List_Change_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddZametka(Title,DateTime.Now.ToShortDateString()));
        }
    }
}


