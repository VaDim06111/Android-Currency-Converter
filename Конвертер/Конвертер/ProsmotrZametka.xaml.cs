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
        string[] masIdCost = new string[20];
        string[] masIdLayout = new string[20];
        string[] masKurs = new string[5];
        string SelectedItem = null;
        int masCount = 0;
        int count = 0;
        string LastValuePicker = null;
        public ProsmotrZametka(string _name,string usdKurs, string eurKurs, string rubKurs, string zlKurs, string uahKurs)
        {
            try
            {
                InitializeComponent();
                NavigationPage.SetHasNavigationBar(this, false);
                LabelName.Text = _name;
                PickerCost.Items.Add("USD");
                PickerCost.Items.Add("EUR");
                PickerCost.Items.Add("BYN");
                PickerCost.Items.Add("RUB");
                PickerCost.Items.Add("UAH");
                PickerCost.Items.Add("ZL");
                masKurs[0] = usdKurs;
                masKurs[1] = eurKurs;
                masKurs[2] = rubKurs;
                masKurs[3] = zlKurs;
                masKurs[4] = uahKurs;
                ReadFromFileZametka();
            }
            catch(Exception ex)
            {
                DisplayAlert("Ошибка",ex.Message,"OK");
            }
        }
        public async void ReadFromFileZametka()
        {
            masCount = 0;
            string txtfile = null;
            string[] mas = new string[20];           
            IFileSystem fileSystem = FileSystem.Current;
            IFolder rootFolder = fileSystem.LocalStorage;
            IFolder ZametkaFolder = await rootFolder.CreateFolderAsync("Zametki", CreationCollisionOption.OpenIfExists);
            IFile zametkaFile = await ZametkaFolder.GetFileAsync(LabelName.Text + ".xml");
            txtfile = await zametkaFile.ReadAllTextAsync();
            mas = txtfile.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            LabelDate.Text = mas[0];
            count = mas.Length / 5;
            switch(mas[1])
            {
                case "USD":
                    {
                        SelectedItem = "USD";
                        LastValuePicker = "USD";
                        break;
                    }
                case "EUR":
                    {
                        SelectedItem = "EUR";
                        LastValuePicker = "EUR";
                        break;
                    }
                case "BYN":
                    {
                        SelectedItem = "BYN";
                        LastValuePicker = "BYN";
                        break;
                    }
                case "RUB":
                    {
                       SelectedItem = "RUB";
                        LastValuePicker = "RUB";
                        break;
                    }
                case "UAH":
                    {
                        SelectedItem = "UAH";
                        LastValuePicker = "UAH";
                        break;
                    }
                case "ZL":
                    {
                        SelectedItem = "ZL";
                        LastValuePicker = "ZL";
                        break;
                    }
                default:
                    break;
            }
            
            for (int i = 2; i < mas.Length; i+=5)
            {
                var NewStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand, Padding = 10 };
                masIdLayout[masCount] = NewStackLayout.Id.ToString();
                var LabelName = new Label() { Text = mas[i], FontSize = 30, WidthRequest = 230, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center };
                var LabelCount = new Label() { Text = mas[i+1], FontSize = 30, WidthRequest = 55, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                var LabelCost = new Label() { Text = mas[i+2], FontSize = 30, WidthRequest = 120, TextColor = Color.Black, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.Center };
                masIdCost[masCount] = LabelCost.Id.ToString();
                NewStackLayout.Children.Add(LabelName);
                NewStackLayout.Children.Add(LabelCount);
                NewStackLayout.Children.Add(LabelCost);
                ZametkiStackLayout.Children.Add(NewStackLayout);
                masCount++;
            }
            PickerCost.SelectedItem = SelectedItem;
        }

        private void PickerCost_SelectedIndexChanged(object sender, EventArgs e)
        {            
            Label[] masLabel = new Label[count];
            string valueBYN = null;
            if(masIdCost != null && masIdLayout !=null)
            for (int i = 0; i < count; i++)
            {
                var layout = (ZametkiStackLayout.Children.Where(x => x.Id.ToString() == masIdLayout[i])).FirstOrDefault() as StackLayout;
                masLabel[i] = (layout.Children.Where(y => y.Id.ToString() == masIdCost[i]).FirstOrDefault() as Label);


                if (LastValuePicker == "USD")
                    switch (PickerCost.SelectedItem.ToString())
                    {
                        case "EUR":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[0]), 3).ToString();
                            masLabel[i].Text= Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[1])), 1).ToString();
                            break;
                        case "BYN":
                            masLabel[i].Text = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[0]), 1).ToString();
                            break;
                        case "RUB":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[0]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[2]), 1).ToString();
                            break;
                        case "UAH":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[0]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[4]), 1).ToString();
                            break;
                        case "ZL":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[0]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 10 / Convert.ToDouble(masKurs[3]), 1).ToString();
                            break;
                        default:
                            break;
                    }
                else
                    if (LastValuePicker == "EUR")
                    switch (PickerCost.SelectedItem.ToString())
                    {
                        case "USD":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[1]), 3).ToString();
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[0])), 1).ToString();
                            break;
                        case "BYN":
                            masLabel[i].Text = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[1]), 1).ToString();
                            break;
                        case "RUB":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[1]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[2]), 1).ToString();
                            break;
                        case "UAH":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[1]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[4]), 1).ToString();
                            break;
                        case "ZL":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) * Convert.ToDouble(masKurs[1]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 10 / Convert.ToDouble(masKurs[3]), 1).ToString();
                            break;
                        default:
                            break;
                    }
                else
                    if (LastValuePicker == "BYN")
                    switch (PickerCost.SelectedItem.ToString())
                    {
                        case "USD":
                            valueBYN = masLabel[i].Text;
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[0])), 1).ToString();
                            break;
                        case "EUR":
                            valueBYN = masLabel[i].Text;
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[1])), 1).ToString();
                            break;                                                   
                        case "RUB":
                            valueBYN = masLabel[i].Text;
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[2]), 1).ToString();
                            break;
                        case "UAH":
                            valueBYN = masLabel[i].Text;
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[4]), 1).ToString();
                            break;
                        case "ZL":
                            valueBYN = masLabel[i].Text;
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 10 / Convert.ToDouble(masKurs[3]), 1).ToString();
                            break;
                        default:
                            break;
                    }
                else
                    if (LastValuePicker == "RUB")
                    switch (PickerCost.SelectedItem.ToString())
                    {
                        case "USD":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) /100 * Convert.ToDouble(masKurs[2]), 3).ToString();
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[0])), 1).ToString();
                            break;
                        case "EUR":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[2]), 3).ToString();
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[0])), 1).ToString();
                            break;
                        case "BYN":
                            masLabel[i].Text = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) /100 * Convert.ToDouble(masKurs[2]), 1).ToString();
                            break;                      
                        case "UAH":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[2]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[4]), 1).ToString();
                            break;
                        case "ZL":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[2]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 10 / Convert.ToDouble(masKurs[3]), 1).ToString();
                            break;
                        default:
                            break;
                    }
                else
                    if (LastValuePicker == "UAH")
                    switch (PickerCost.SelectedItem.ToString())
                    {
                        case "USD":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[4]), 3).ToString();
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[0])), 1).ToString();
                            break;
                        case "EUR":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[4]), 3).ToString();
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[1])), 1).ToString();
                            break;
                        case "RUB":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[4]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[2]), 1).ToString();
                            break;
                        case "BYN":
                            masLabel[i].Text = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[4]), 1).ToString();
                            break;                        
                        case "ZL":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[4]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 10 / Convert.ToDouble(masKurs[3]), 1).ToString();
                            break;
                        default:
                            break;
                    }
                else
                    if (LastValuePicker == "ZL")
                    switch (PickerCost.SelectedItem.ToString())
                    {
                        case "USD":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 10 * Convert.ToDouble(masKurs[3]), 3).ToString();
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[0])), 1).ToString();
                            break;
                        case "EUR":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 10 * Convert.ToDouble(masKurs[3]), 3).ToString();
                            masLabel[i].Text = Math.Round((Convert.ToDouble(valueBYN) / Convert.ToDouble(masKurs[1])), 1).ToString();
                            break;
                        case "RUB":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 10 * Convert.ToDouble(masKurs[3]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[2]), 1).ToString();
                            break;
                        case "BYN":
                            masLabel[i].Text = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 100 * Convert.ToDouble(masKurs[4]), 1).ToString();
                            break;
                        case "UAH":
                            valueBYN = Math.Round(double.Parse(masLabel[i].Text.Replace(" ", "")) / 10 * Convert.ToDouble(masKurs[3]), 3).ToString();
                            masLabel[i].Text = Math.Round(double.Parse(valueBYN) * 100 / Convert.ToDouble(masKurs[4]), 1).ToString();
                            break;
                        default:
                            break;
                    }
            }
            LastValuePicker = PickerCost.SelectedItem.ToString();
        }
    }
}