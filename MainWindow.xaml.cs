using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MicrocontrollerProject
{
    public partial class MainWindow : Window
    {
        string[] Rejestry = { "A", "AL", "B", "BL", "C", "CL", "D", "DL"};
        string[] Mnemoniki = { "mov", "add", "sub", "inc", "dec", "shl", "shr", "xor", "and", "or", "cmp", "jmp", "je", "jl", "jle", "pop", "push", "int", "out"};
        Mikrokontroler mikrokontroler = new Mikrokontroler();
        ObservableCollection<LiniaKodu> liniakodu = new ObservableCollection<LiniaKodu>();
        int IP = 0;
        public MainWindow()
        {
            InitializeComponent();
            mikrokontroler.Show();
            Mnemonik.ItemsSource =Mnemoniki;
            CalyKod.ItemsSource = liniakodu;
        }

        private void Zamknij_Click(object sender, RoutedEventArgs e)
        {
            mikrokontroler.Close();
            this.Close();
        }

        private void Wczytaj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "plik txt (*.txt)|*.txt";
            if (openFileDialog.ShowDialog() == true)
            {
                StreamReader sr = new StreamReader(openFileDialog.FileName);
                string linia;
                liniakodu.Clear();
                IP = 0;
                while ((linia = sr.ReadLine()) != null)
                {
                    try
                    {
                        LiniaKodu buffor = new LiniaKodu();
                        liniakodu.Add(buffor);
                        liniakodu[IP].Nr = Int32.Parse(linia.Substring(0, linia.IndexOf('\t')));
                        linia = linia.Remove(0, linia.IndexOf('\t') + 1);
                        liniakodu[IP].Mn = linia.Substring(0, linia.IndexOf('\t'));
                        linia = linia.Remove(0, linia.IndexOf('\t') + 1);
                        liniakodu[IP].A1 = linia.Substring(0, linia.IndexOf('\t'));
                        linia = linia.Remove(0, linia.IndexOf('\t') + 1);
                        liniakodu[IP].A2 = linia;
                        IP++;
                    }
                    catch {
                        MessageBox.Show("Blad wczytywania pliku");
                        break;
                    }
                }
                CalyKod.Items.Refresh();
            }
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                if (liniakodu.Count != 0)
                {
                    if (!saveFileDialog.FileName.Contains(".txt")) {
                        saveFileDialog.FileName += ".txt";
                    }
                    StreamWriter sw = File.CreateText(saveFileDialog.FileName);
                    for (int i = 0; i < liniakodu.Count; i++)
                    {
                        sw.Write(liniakodu[i].Nr);
                        sw.Write('\t');
                        sw.Write(liniakodu[i].Mn);
                        sw.Write('\t');
                        sw.Write(liniakodu[i].A1);
                        sw.Write('\t');
                        sw.WriteLine(liniakodu[i].A2);
                    }
                    sw.Close();
                }
                else {
                    MessageBox.Show("Pole kodu jest puste!");
                }                
            }
        }

        private void Mnemonik_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Arg1.IsEditable = false;
            Arg2.IsEditable = false;
            Arg2.IsEnabled = true;
            switch (Mnemonik.SelectedIndex) {
                case 0: //mov
                    Arg1.ItemsSource = Rejestry;
                    Arg2.ItemsSource = Rejestry;
                    Arg2.IsEditable = true;
                    break;
                case 1: //add
                    Arg1.ItemsSource = Rejestry;
                    Arg2.ItemsSource = Rejestry;
                    break;
                case 2: //sub
                    Arg1.ItemsSource = Rejestry;
                    Arg2.ItemsSource = Rejestry;
                    break;
                case 3: //inc
                    Arg1.ItemsSource = Rejestry;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    break;
                case 4: //dec
                    Arg1.ItemsSource = Rejestry;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    break;
                case 5: //shl
                    Arg1.ItemsSource = Rejestry;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    break;
                case 6: //shr
                    Arg1.ItemsSource = Rejestry;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    break;
                case 7: //xor
                    Arg1.ItemsSource = Rejestry;
                    Arg2.ItemsSource = Rejestry;
                    break;
                case 8: //and
                    Arg1.ItemsSource = Rejestry;
                    Arg2.ItemsSource = Rejestry;
                    break;
                case 9: //or
                    Arg1.ItemsSource = Rejestry;
                    Arg2.ItemsSource = Rejestry;
                    break;
                case 10: //cmp
                    Arg1.ItemsSource = Rejestry;
                    Arg2.ItemsSource = Rejestry;
                    Arg2.IsEditable = true;
                    break;
                case 11: //jmp
                    Arg1.ItemsSource = null;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    Arg1.IsEditable = true;
                    break;
                case 12: //je
                    Arg1.ItemsSource = null;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    Arg1.IsEditable = true;
                    break;
                case 13: //jl
                    Arg1.ItemsSource = null;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    Arg1.IsEditable = true;
                    break;
                case 14: //jle
                    Arg1.ItemsSource = null;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    Arg1.IsEditable = true;
                    break;
                case 15: //pop
                    Arg1.ItemsSource = Rejestry;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    break;
                case 16: //push
                    Arg1.ItemsSource = Rejestry;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    break;
                case 17: //int
                    Arg1.ItemsSource = null;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    Arg1.IsEditable = true;
                    break;
                case 18: //out
                    Arg1.ItemsSource = Rejestry;
                    Arg2.IsEnabled = false;
                    Arg2.Text = "";
                    break;
            }
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Mnemonik.Text) && !string.IsNullOrEmpty(Arg1.Text) && (!string.IsNullOrEmpty(Arg2.Text)||!Arg2.IsEnabled))
            {
                LiniaKodu buffor = new LiniaKodu();
                buffor.Mn = Mnemonik.SelectedItem.ToString();
                buffor.Nr = IP;
                if (Arg1.IsEditable == true)
                {
                    buffor.A1 = Arg1.Text;
                }
                else if (Arg1.IsEnabled == false)
                {
                    buffor.A1 = "";
                }
                else
                {
                    buffor.A1 = Arg1.SelectedItem.ToString();

                }
                if (Arg2.IsEditable == true)
                {
                    buffor.A2 = Arg2.Text;
                }
                else if (Arg2.IsEnabled == false)
                {
                    buffor.A2 = "";
                }
                else
                {
                    buffor.A2 = Arg2.SelectedItem.ToString();

                }
                liniakodu.Add(buffor);
                IP++;
            }
        }
        public class LiniaKodu
        {
            public int Nr { get; set; }
            public string Mn { get; set; }
            public string A1 { get; set; }
            public string A2 { get; set; }

        }
        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            if (CalyKod.SelectedItem != null) {
                LiniaKodu buffor = new LiniaKodu();
                buffor = (LiniaKodu)CalyKod.SelectedItem;
                liniakodu.Remove(liniakodu.Where(i => i.Nr == buffor.Nr).Single());
                IP--;
                for (int i = buffor.Nr; i < liniakodu.Count; i++) {
                    liniakodu[i].Nr--;
                }
                CalyKod.Items.Refresh();
            }
        }

        private void Zaprogramuj_Click(object sender, RoutedEventArgs e)
        {
            if (liniakodu.Count != 0) {
                mikrokontroler.liniakodu = new ObservableCollection<LiniaKodu>(liniakodu);
                mikrokontroler.Kod.ItemsSource = mikrokontroler.liniakodu;
                mikrokontroler.WyczyscRejestry();
                mikrokontroler.KONSOLAOUT.Text = "";
                mikrokontroler.KONSOLAIN.Text = "";
                mikrokontroler.KONSOLAOUT.Foreground = Brushes.Black;
                mikrokontroler.KONSOLAOUT.FontSize = 18;
                mikrokontroler.Kod.SelectedIndex = -1;
            }
        }
    }
}
