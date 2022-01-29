using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace siegenia_calc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    //
    public partial class MainWindow : Window
    {
        public int smin;
        public int smax;
        public int wmin;
        public int wmax;
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbxControl = sender as TextBox;
            foreach (char z in tbxControl.Text)
            {
                if (!Char.IsDigit(z))
                {
                    tbxControl.BorderBrush = Brushes.Red;
                    tbxControl.Foreground = Brushes.Red;
                    LbErr2.Content = "Wprowadzono nieprawidłowe znaki!";
                    LbErr2.Foreground = Brushes.Red;
                }
                else
                {

                    tbxControl.BorderBrush = Brushes.Green;
                    tbxControl.Background = Brushes.Transparent;
                    tbxControl.Foreground = Brushes.Black;

                    LbErr2.Content = "";

                }

            }
            if (tbxControl.Text == "")
            {
                tbxControl.Background = Brushes.Transparent;
                tbxControl.BorderBrush = Brushes.Black;
                tbxControl.Foreground = Brushes.Black;
                LbErr2.Content = "";
            }

        }
        private void TbZam_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbxControl = sender as TextBox;
            foreach (char z in tbxControl.Text)
            {
                if (!Char.IsDigit(z))
                {
                    tbxControl.BorderBrush = Brushes.Red;
                    tbxControl.Foreground = Brushes.Red;
                }
                else
                {

                    tbxControl.BorderBrush = Brushes.Green;
                    tbxControl.Background = Brushes.Transparent;
                    tbxControl.Foreground = Brushes.Black;
                }

            }
            if (tbxControl.Text == "")
            {
                tbxControl.Background = Brushes.Transparent;
                tbxControl.BorderBrush = Brushes.Black;
                tbxControl.Foreground = Brushes.Black;
            }
        }
        private void TbKlient_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tbxControl = sender as TextBox;
            if (tbxControl.Text.Length != 0)
            {
                tbxControl.BorderBrush = Brushes.Green;
            }
            else tbxControl.BorderBrush = Brushes.Black;
        }
        private void BtnOblicz_Click(object sender, RoutedEventArgs e)
        {
            string filename = $"{TbKlient.Text}_{TbZam.Text}.txt";
            bool check = CheckAll();
            if (!check)
            {
                string text = "Formularz zawiera błędy!\nPopraw dane wejściowe";
                MessageBox.Show(text, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TbProfil.Foreground = Brushes.Black;
                TbOkucia.Foreground = Brushes.Black;

                if (TbOkucia.Text.Length > 10 && TbProfil.Text.Length > 13)
                {
                    TbOkucia.Clear();
                    TbProfil.Clear();
                    TbOkucia.Text = "Lista okuć";
                    TbProfil.Text = "Lista profili";
                }
                TbProfil.Text += "\n\n";
                TbProfil.Text += "Nr\tNazwa\t\tPozycja\t\tDługość[mm]\n\n";
                string inputFileName = SetInputFileName();
                FillFirstList(inputFileName);
                FillSecondList();
                SaveFileButton.IsEnabled = true;
            }
        }
        private void FillSecondList()
        {
            float x = float.Parse(TbSzer.Text);
            float y = float.Parse(TbWys.Text);


            if (CbTyp.SelectedItem == ru)
            {
                if (CbKlasa.SelectedItem == standard) ProcessRu(x, y, 0);
                else ProcessRu(x, y, 1);
            }
            if (CbTyp.SelectedItem == roz)
            {
                if (CbKlasa.SelectedItem == standard) ProcessRoz(x, y, 0);
                else ProcessRoz(x, y, 1);
            }
            if (CbTyp.SelectedItem == sru)
            {
                if (CbKlasa.SelectedItem == standard) ProcessSru(x, y, 0);
                else ProcessSru(x, y, 1);
            }
            if (CbTyp.SelectedItem == uch)
            {
                if (CbKlasa.SelectedItem == standard) ProcessUch(x, y, 0);
                else ProcessUch(x, y, 1);
            }
        }
        private void FillFirstList(string inputFileName)
        {
            TbOkucia.Text += "\n\nNr art.\t\tNazwa\t\t\tIlość\n\n";
            using (var inputFileStream = new FileStream(inputFileName, FileMode.Open))
            {
                using (var inputStreamReader = new StreamReader(inputFileStream))
                {
                    TbOkucia.Text += inputStreamReader.ReadToEnd();
                }
            }
        }

        private void Cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CbKlasa.SelectedItem == null || CbTyp.SelectedItem == null)
            {
                LbSzer.Content = "";
                LbWys.Content = "";
            }
            else
            {
                if (CbTyp.SelectedItem == ru || CbTyp.SelectedItem == roz || CbTyp.SelectedItem == sru)
                {

                    if (CbKlasa.SelectedItem == standard)
                    {
                        smin = 365;
                        wmin = 550;
                        smax = 1600;
                        wmax = 2600;
                    }
                    else
                    {
                        smin = 500;
                        wmin = 770;
                        smax = 1600;
                        wmax = 2600;
                    }
                }
                else
                {
                    if (CbKlasa.SelectedItem == standard)
                    {
                        smin = 400;
                        wmin = 450;
                        smax = 2100;
                        wmax = 1200;
                    }
                    else
                    {
                        smin = 500;
                        wmin = 450;
                        smax = 2100;
                        wmax = 1200;
                    }
                }
                LbSzer.Content = $"({smin} - {smax})";
                LbWys.Content = $"({wmin} - {wmax})";
            }
        }
        private bool CheckAll()
        {
            if (CbKlient.SelectedItem == null || CbTyp.SelectedItem == null || CbKlasa.SelectedItem == null || LbErr2.Content.ToString() != "" || TbZam.Foreground == Brushes.Red || int.Parse(TbSzer.Text) < smin || int.Parse(TbSzer.Text) > smax || int.Parse(TbWys.Text) < wmin || int.Parse(TbWys.Text) > wmax)
            {
                return false;
            }
            else return true;

        }
        private string SetInputFileName()
        {
            string name = "ref/";
            if (CbTyp.SelectedItem == ru)
            {
                name += "ru_";
            }
            else
            {
                if (CbTyp.SelectedItem == roz)
                {
                    name += "roz_";
                }
                else
                {
                    if (CbTyp.SelectedItem == sru)
                    {
                        name += "sru_";
                    }
                    else
                    {
                        if (CbTyp.SelectedItem == uch)
                        {
                            name += "uch_";
                        }
                    }
                }
            }
            if (CbKlasa.SelectedItem == standard)
            {
                name += "standard.sc";
            }
            else
            {
                name += "rc2.sc";
            }
            return name;
        }
        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            string outputFileName = CbKlient.Text + "_" + TbZam.Text;
            SaveFileDialog saveWindow = new SaveFileDialog();
            saveWindow.Filter = "Plik tekstowy (*.txt) | *.txt";
            saveWindow.FileName = outputFileName;
            saveWindow.DefaultExt = ".txt";
            saveWindow.Title = "Zapisywanie jako...";
            saveWindow.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (saveWindow.ShowDialog() == true)
            {
                using (var fileStream = new FileStream(saveWindow.FileName, FileMode.CreateNew))
                {
                    using (var streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine($"Klient:\t\t{CbKlient.Text}\n");
                        streamWriter.WriteLine($"Numer zamówienia:\t{TbZam.Text}\n");
                        streamWriter.WriteLine($"Wymiar:\t{TbSzer.Text} x {TbWys.Text} mm\n");
                        streamWriter.WriteLine($"Typ okucia:\t {CbTyp.Text}\n");
                        streamWriter.WriteLine($"Klasa odporności:\t {CbKlasa.Text}\n\n");
                        streamWriter.WriteLine("\n-----------------------------------------------\n");
                        streamWriter.WriteLine(TbOkucia.Text);
                        streamWriter.WriteLine("\n-----------------------------------------------\n");
                        streamWriter.WriteLine(TbProfil.Text);
                    }
                }
            }
        }
        private void ProcessRu(float x, float y, int z)
        {
            //SPRAWDZANIE KLASY - TU DLA STANDARD
            if (z == 0)
            {
                int nar = 0;
                float pop1 = y / 2 - 195;

                TbProfil.Text += $"VL59\tPopychacz\tPod zamkiem\t{pop1}\n";
                TbProfil.Text += $"VL59\tPopychacz\tNad zamkiem\t{pop1}\n";
                if (x > 600)
                {
                    float pop3 = 0;
                    if (x > 1020)
                    {
                        pop3 = x - 664;
                        TbOkucia.Text += "\n857076\t\tRozwórka dodatkowa\t1";
                        if (x > 1250)
                        {
                            float pop5 = x / 2 - 172;
                            nar++;
                            TbProfil.Text += $"VL59\tPopychacz\tSkrzydło dół\t{pop5}\n";

                        }
                    }
                    else
                    {
                        pop3 = x - 506;
                    }
                    TbProfil.Text += $"VL59\tPopychacz\tSkrzydło góra\t{pop3}\n";
                    TbOkucia.Text += "\nMSKK0300\tRozwórka\t\t1";

                }
                else
                {
                    float pop3 = x - 338;
                    TbProfil.Text += $"VL59\tPopychacz\tSkrzydło góra\t{pop3}\n";
                    TbOkucia.Text += "\nMSKK0290\tRozwórka\t\t1";
                }

                if (y > 1250)
                {
                    if (y > 1800)
                    {
                        float pop4 = 2 * y / 3;
                        nar++;

                        TbProfil.Text += $"VL59\tPopychacz\tStrona zawiasu\t{Math.Round(pop4, 1)}\n";
                        TbOkucia.Text += "\n317556\t\tDodatkowy zaczep\t1\n";
                    }
                    else
                    {
                        float pop4 = y / 2 - 230;
                        nar++;
                        TbProfil.Text += $"VL59\tPopychacz\tStrona zawiasu\t{pop4}\n";
                    }

                }


                if (nar > 0)
                {
                    TbOkucia.Text += $"857045\t\tPrzeniesienie napędu\t{nar}";
                }
            }
            //DLA RC2
            else
            {
                int vr = 4;
                float pop1 = y / 2 - 234;
                float pop2 = y / 2 - 238;
                float pop3 = 0;
                float pop4 = y - 302;
                TbProfil.Text += $"VL59\tPopychacz\tPod zamkiem\t{pop1}\t\n";
                TbProfil.Text += $"VL59\tPopychacz\tNad zamkiem\t{pop2}\t\n";
                if (x > 600)
                {


                    if (x > 1020)
                    {
                        pop3 = x - 664;
                        TbOkucia.Text += "\n857076\t\tRozwórka dodatkowa\t1";
                    }
                    else
                    {
                        pop3 = x - 506;
                    }

                    TbOkucia.Text += "\nMSKK0300\tRozwórka\t\t1";

                    if (x > 900)
                    {
                        float pop5 = x / 2 - 214;
                        vr += 2;
                        TbProfil.Text += $"VL59\tPopychacz\tSkrzydło dół\t{pop5}\n";

                    }
                }
                else
                {
                    pop3 = x - 338;

                    TbOkucia.Text += "\nMSKK0290\tRozwórka\t\t1";
                }
                if (y > 1400)
                {
                    TbOkucia.Text += "\n317556\t\tDodatkowy zaczep\t1";
                    vr += 2;
                    if (y > 2000)
                    {
                        vr += 2;
                    }
                }
                TbProfil.Text += $"VL59\tPopychacz\tSkrzydło góra\t{pop3}\n";
                TbProfil.Text += $"VL59\tPopychacz\tStrona zawiasu\t{pop4}\n";
                TbOkucia.Text += $"\nMSVR0050\tZacz. antywłamaniowy\t{vr}";


            }
        }
        private void ProcessRoz(float x, float y, int z)
        {
            if (z == 0)
            {
                float pop1 = y / 2 - 161;
                int bm = 0;
                int nab = 0;
                if (y > 1250)
                {
                    bm++;
                    nab++;
                    if (y > 1800)
                    {
                        bm++;
                        nab++;
                    }
                    TbOkucia.Text += $"857052\t\tDocisk skrzydła\t\t\t{bm}\n";
                    TbOkucia.Text += $"317556\t\tDodatkowy zaczep\t\t{nab}\n";

                }
                TbProfil.Text += $"VL59\tPopychacz\tPod zamkiem\t{pop1}\n";
                TbProfil.Text += $"VL59\tPopychacz\tNad zamkiem\t{pop1}\n";
            }
            else
            {
                ProcessRu(x, y, 1);
            }
        }
        private void ProcessSru(float x, float y, int z)
        {
            float pop1 = y / 2 - 234;
            float pop2 = y / 2 - 238;
            float pop3 = 0;
            float pop4 = y - 302;
            float pop5 = 0;
            float pop6 = 0;
            float pop7 = 0;
            float pop8 = 0;
            float pop10 = 135;
            if (z == 0)
            {
                ProcessRu(x, y, 0);
                if (y > 1250)
                {
                    pop6 = y / 2 - 415;
                    pop7 = y / 2 - 468;
                    TbProfil.Text += $"VL161\tPopychacz\tSłupek dół\t{pop6}\n";
                    TbProfil.Text += $"VL161\tPopychacz\tSłupek góra\t{pop6}\n";
                }


            }
            else
            {
                pop6 = y / 2 - 110;
                pop7 = y / 2 - 110;

                int vr = 6;

                if (x > 730)
                {
                    pop8 = x - 630;
                    TbOkucia.Text += "\nMSKK0300\tRozwórka\t\t1";

                }
                else
                {
                    pop8 = x - 462;
                    TbOkucia.Text += "\nMSKK0290\tRozwórka\t\t1";
                }

                if (x > 900)
                {

                    pop5 = x / 2 - 214;
                    vr += 4;
                    TbProfil.Text += $"VL59\tPopychacz\tSkrzydło dół\t{pop5}\n";
                }
                if (y > 1400)
                {
                    TbOkucia.Text += "\n317556\t\tDodatkowy zaczep\t2";
                    vr += 3;
                    if (y > 2000)
                    {
                        vr += 3;
                    }
                }
                if (x > 600)
                {


                    if (x > 1020)
                    {
                        pop3 = x - 664;
                        TbOkucia.Text += "\n857076\t\tRozwórka dodatkowa\t1";
                    }
                    else
                    {
                        pop3 = x - 506;
                    }

                    TbOkucia.Text += "\nMSKK0300\tRozwórka\t\t1";

                }
                else
                {
                    pop3 = x - 338;

                    TbOkucia.Text += "\nMSKK0290\tRozwórka\t\t1";
                }
                TbProfil.Text += $"VL59\tPopychacz\tPod zamkiem RU\t{pop1}\n";
                TbProfil.Text += $"VL59\tPopychacz\tNad zamkiem RU\t{pop2}\n";
                TbProfil.Text += $"VL59\tPopychacz\tSkrzydło RU góra\t{pop3}\n";
                TbProfil.Text += $"VL59\tPopychacz\tStr. zawiasu RU\t{pop4}\n";
                TbProfil.Text += $"VL59\tPopychacz\tPod zamkiem R\t{pop6}\n";
                TbProfil.Text += $"VL59\tPopychacz\tNad zamkiem R\t{pop7}\n";
                TbProfil.Text += $"VL59\tPopychacz\tSkrzydło R góra\t{pop8}\n";
                TbProfil.Text += $"VL59\tPopychacz\tStr. zawiasu R\t{pop4}\n";
                TbProfil.Text += $"VL59\tPopychacz\tSkrzydło R dół\t{pop10}\n";
                TbOkucia.Text += $"\nMSVR0050\tZacz. antywłamaniowy\t{vr}";

            }
        }
        private void ProcessUch(float x, float y, int z)
        {
            float pop1 = 0;
            int zaw = 2;
            if (z == 0)
            {
                if (x > 840)
                {
                    if (x < 1020)
                    {
                        pop1 = 30;
                    }
                    else
                    {
                        pop1 = x / 2 - 232;
                    }
                    if (x > 1250)
                    {
                        zaw++;
                        if (x > 1800)
                        {
                            zaw++;
                            TbOkucia.Text += "317556\t\tDodatkowy zaczep\t\t2\n";
                        }
                    }
                    TbOkucia.Text += "MMVS0330\tZest. zacz. ryglujących\t\t1";
                    TbProfil.Text += $"VL59\tPopychacz\tStrona zamka\t{pop1}\n";
                    TbProfil.Text += $"VL59\tPopychacz\tStrona zamka\t{pop1}\n";
                }
                else
                {
                    TbOkucia.Text += "859322\t\tZaczep\t\t\t\t1\n";
                }
                TbOkucia.Text += $"\nMMKB0020\tZawias uchyłu\t\t\t{zaw}\n";
            }
            else
            {
                int vr = 2;
                int zr = 0;
                pop1 = x / 2 - 110;
                TbProfil.Text += $"VL59\tPopychacz\tStrona zamka\t{pop1}\n";
                TbProfil.Text += $"VL59\tPopychacz\tStrona zamka\t{pop1}\n";

                if (y > 660)
                {
                    float pop2 = y / 2 - 258;
                    TbProfil.Text += $"VL59\tPopychacz\tLewa strona\t{pop2}\n";
                    TbProfil.Text += $"VL59\tPopychacz\tPrawa strona\t{pop2}\n";
                    zr++;
                    TbOkucia.Text += "\nzeMMVS0330\tZest. zacz. ryglujących\t1";
                    vr += 2;
                }
                if (x > 1200)
                {
                    if (y < 660)
                    {
                        TbOkucia.Text += "MMVS0330\tZest. zacz. ryglujących\t1";
                        TbOkucia.Text += "MMZV0030\tZestaw narożnika\t1";
                    }
                    zaw++;
                    if (x > 1800)
                    {
                        zaw++;
                        vr += 2;
                    }
                }
                TbOkucia.Text += $"\nMSVR0050\tZacz. antywłamaniowy\t{vr}";
                TbOkucia.Text += $"\nMMKB0020\tZawias uchyłu\t\t{zaw}\n";
            }
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            var msg = MessageBox.Show("Czy jesteś pewien?", "Uwaga", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (msg == MessageBoxResult.Yes)
            {
                TbKlient.Clear();
                TbZam.Clear();
                LbSzer.Content = "";
                LbWys.Content = "";
                Min.Content = "";
                TbSzer.Clear();
                TbWys.Clear();
                TbOkucia.Clear();
                TbProfil.Clear();
                CbTyp.SelectedItem = null;
                CbKlasa.SelectedItem = null;
                CbKlient.SelectedItem = null;
                TbOkucia.Text = "Lista okuć";
                TbOkucia.Foreground = Brushes.DimGray;
                TbProfil.Text = "Lista profili";
                TbProfil.Foreground = Brushes.DimGray;
                SaveFileButton.IsEnabled = false;
            }
        }
        private void BtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            StpAddClient.Visibility = Visibility.Visible;
            Application.Current.MainWindow.Height = 500;
        }

        private void BtnAddNewClient_Click(object sender, RoutedEventArgs e)
        {
            bool isCorrect = true;
            foreach (char z in TbKlient.Text)
            {
                if (!Char.IsWhiteSpace(z))
                {
                    isCorrect = true;
                    break;
                }
                else
                {
                    isCorrect = false;
                }
            }
            if (TbKlient.Text.Length != 0 && isCorrect)
            {
                CbKlient.Items.Add(TbKlient.Text);
                CbKlient.SelectedIndex = CbKlient.Items.Count-1;
                TbKlient.Clear();
            }
            else MessageBox.Show("Nieprawidłowy format nazwy!", "Błąd!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void BtnHideAddClient_Click(object sender, RoutedEventArgs e)
        {
            TbKlient.Clear();
            StpAddClient.Visibility = Visibility.Hidden;
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            string tekst = "Siegenia Calc v1.1\n\n";
            tekst += "Program jest projektem zaliczeniowym z Podstaw Programowania na APSL. Służy do wyliczania okuć do okien aluminiowych.\n\n";
            tekst += "Wszystkie metody obliczeniowe zostały zaczerpnięte z ogólnodostęonych katalogów okuć marki SIEGENIA.\n\n";
            tekst += "©2021-2022 Maciej Janta-Lipiński";
            MessageBox.Show(tekst, "O programie", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
