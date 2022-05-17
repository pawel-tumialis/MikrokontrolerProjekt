using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MicrocontrollerProject
{
    /// <summary>
    /// Logika interakcji dla klasy Mikrokontroler.xaml
    /// </summary>
    public partial class Mikrokontroler : Window
    {
        string[] Rejestry = { "A", "AL", "B", "BL", "C", "CL", "D", "DL" };
        public ObservableCollection<MainWindow.LiniaKodu> liniakodu = new ObservableCollection<MainWindow.LiniaKodu>();
        [StructLayout(LayoutKind.Explicit)]
        unsafe public struct Rejestr
        {
            [FieldOffset(0)] public ushort Short;
            [FieldOffset(0)] public byte ByteL;
            [FieldOffset(1)] public byte ByteH;
        }

        public struct Flagi {
            public bool Ef;
            public bool Lf;
        }

        public struct Wyjscia {
            public byte wyjscie;
        }

        Rejestr Ip = new Rejestr();
        Rejestr A = new Rejestr();
        Rejestr B = new Rejestr();
        Rejestr C = new Rejestr();
        Rejestr D = new Rejestr();
        Rejestr Stos1 = new Rejestr();
        Rejestr Stos2 = new Rejestr();
        Rejestr Stos3 = new Rejestr();
        Flagi Flaga = new Flagi();
        Wyjscia Wyjscie = new Wyjscia();

        public Mikrokontroler()
        {
            InitializeComponent();
        }

        unsafe private Rejestr* SprawdzRejestr(string nazwa) {
            int buffor = 0;
            for (int i = 0; i < Rejestry.Length; i++) {
                if (Rejestry[i] == nazwa) {
                    buffor = i;
                    i= Rejestry.Length;
                }
            }
            switch (buffor) { 
                case 0:
                    fixed (Rejestr* ptr = &A)
                    {
                        return ptr;
                    }
                case 1:
                    fixed (Rejestr* ptr = &A)
                    {
                        return ptr;
                    }
                case 2:
                    fixed (Rejestr* ptr = &B)
                    {
                        return ptr;
                    }
                case 3:
                    fixed (Rejestr* ptr = &B)
                    {
                        return ptr;
                    }
                case 4:
                    fixed (Rejestr* ptr = &C)
                    {
                        return ptr;
                    }
                case 5:
                    fixed (Rejestr* ptr = &C)
                    {
                        return ptr;
                    }
                case 6:
                    fixed (Rejestr* ptr = &D)
                    {
                        return ptr;
                    }
                case 7:
                    fixed (Rejestr* ptr = &D)
                    {
                        return ptr;
                    }
            }
            fixed (Rejestr* ptr = &Stos1)
            {
                return ptr;
            }

        }
        private string LiczbaNaString<T>(ref T wejscie)
        {
            T liczba = wejscie;
            string buffor= Convert.ToString((dynamic)liczba, 2);
            int ilosc = 8 * Marshal.SizeOf(liczba);
            buffor= buffor.PadLeft(ilosc, '0');
            return buffor;
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            while (Ip.Short < liniakodu.Count) {
                KodKrok();
            }          
        }
        public void WyczyscRejestry() {
            Ip.Short = 0;
            A.Short = 0;
            B.Short = 0;
            C.Short = 0;
            D.Short = 0;
            Stos1.Short = 0;
            Stos2.Short = 0;
            Stos3.Short = 0;
            Flaga.Ef = false;
            Flaga.Lf = false;
            Wyjscie.wyjscie = 0;
            AktualizujRejestry();
        }
        private void AktualizujRejestry() {
            IPH.Text = LiczbaNaString(ref Ip.ByteH);
            IPL.Text = LiczbaNaString(ref Ip.ByteL);
            AH.Text = LiczbaNaString(ref A.ByteH);
            AL.Text = LiczbaNaString(ref A.ByteL);
            BH.Text = LiczbaNaString(ref B.ByteH);
            BL.Text = LiczbaNaString(ref B.ByteL);
            CH.Text = LiczbaNaString(ref C.ByteH);
            CL.Text = LiczbaNaString(ref C.ByteL);
            DH.Text = LiczbaNaString(ref D.ByteH);
            DL.Text = LiczbaNaString(ref D.ByteL);
            SH1.Text = LiczbaNaString(ref Stos1.ByteH);
            SL1.Text = LiczbaNaString(ref Stos1.ByteL);
            SH2.Text = LiczbaNaString(ref Stos2.ByteH);
            SL2.Text = LiczbaNaString(ref Stos2.ByteL);
            SH3.Text = LiczbaNaString(ref Stos3.ByteH);
            SL3.Text = LiczbaNaString(ref Stos3.ByteL);
            OUT.Text = LiczbaNaString(ref Wyjscie.wyjscie);
            if (Flaga.Ef)
            {
                EF.Text = "1";
            }
            else {
                EF.Text = "0";
            }
            if (Flaga.Lf)
            {
                LF.Text = "1";
            }
            else
            {
                LF.Text = "0";
            }
        }
        private void Krok_Click(object sender, RoutedEventArgs e)
        {
            if (Ip.Short<liniakodu.Count) {
                KodKrok();
            }
        }
        private void KodKrok() {
            Kod.SelectedIndex=Ip.Short;
            string mnemonik = liniakodu[Ip.Short].Mn.ToString();
            switch (mnemonik)
            {
                case "mov":
                    ulong liczba;
                    if (UInt64.TryParse(liniakodu[Ip.Short].A2, out liczba))
                    {
                        Mov(liniakodu[Ip.Short].A1, (ushort)liczba);
                    }
                    else
                    {
                        Mov(liniakodu[Ip.Short].A1, liniakodu[Ip.Short].A2);
                    }
                    break;
                case "add":
                    Add(liniakodu[Ip.Short].A1, liniakodu[Ip.Short].A2);
                    break;
                case "sub":
                    Sub(liniakodu[Ip.Short].A1, liniakodu[Ip.Short].A2);
                    break;
                case "inc":
                    Inc(liniakodu[Ip.Short].A1);
                    break;
                case "dec":
                    Dec(liniakodu[Ip.Short].A1);
                    break;
                case "shl":
                    Shl(liniakodu[Ip.Short].A1);
                    break;
                case "shr":
                    Shr(liniakodu[Ip.Short].A1);
                    break;
                case "xor":
                    Xor(liniakodu[Ip.Short].A1, liniakodu[Ip.Short].A2);
                    break;
                case "and":
                    And(liniakodu[Ip.Short].A1, liniakodu[Ip.Short].A2);
                    break;
                case "or":
                    Or(liniakodu[Ip.Short].A1, liniakodu[Ip.Short].A2);
                    break;
                case "cmp":
                    Cmp(liniakodu[Ip.Short].A1, liniakodu[Ip.Short].A2);
                    break;
                case "jmp":
                    if (UInt64.TryParse(liniakodu[Ip.Short].A1, out liczba))
                    {
                        Jmp((ushort)liczba);
                    }
                    break;
                case "je":
                    if (UInt64.TryParse(liniakodu[Ip.Short].A1, out liczba))
                    {
                        Je((ushort)liczba);
                    }
                    break;
                case "jl":
                    if (UInt64.TryParse(liniakodu[Ip.Short].A1, out liczba))
                    {
                        Jl((ushort)liczba);
                    }
                    break;
                case "jle":
                    if (UInt64.TryParse(liniakodu[Ip.Short].A1, out liczba))
                    {
                        Jle((ushort)liczba);
                    }
                    break;
                case "pop":
                    Pop(liniakodu[Ip.Short].A1);
                    break;
                case "push":
                    Push(liniakodu[Ip.Short].A1);
                    break;
                case "int":
                    if (UInt64.TryParse(liniakodu[Ip.Short].A1, out liczba))
                    {
                        Int((ushort)liczba);
                    }
                    break;
                case "out":
                    Out(liniakodu[Ip.Short].A1);
                    break;
            }
            Ip.Short++;
            IPH.Text = LiczbaNaString(ref Ip.ByteH);
            IPL.Text = LiczbaNaString(ref Ip.ByteL);
        }
        unsafe private void Mov(string rejestr1, string rejestr2) {
            Rejestr *r1 = SprawdzRejestr(rejestr1);
            Rejestr *r2 = SprawdzRejestr(rejestr2);
            if (rejestr1.Length == 2)
            {
                r1->ByteL = (byte)r2->Short;
            }
            else {
                r1->Short= r2->Short;
            }
            AktualizujRejestry();
        }
        unsafe private void Mov(string rejestr1, ushort liczba)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            if (rejestr1.Length == 2)
            {
                r1->ByteL = (byte)liczba;
            }
            else
            {
                r1->Short = liczba;
            }
            AktualizujRejestry();
        }
        unsafe private void Add(string rejestr1, string rejestr2) {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Rejestr* r2 = SprawdzRejestr(rejestr2);
            if (rejestr1.Length == 2)
            {
                r1->ByteL += (byte)r2->Short;
            }
            else
            {
                r1->Short += r2->Short;
            }
            AktualizujRejestry();
        }
        unsafe private void Sub(string rejestr1, string rejestr2)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Rejestr* r2 = SprawdzRejestr(rejestr2);
            if (rejestr1.Length == 2)
            {
                r1->ByteL -= (byte)r2->Short;
            }
            else
            {
                r1->Short -= r2->Short;
            }
            AktualizujRejestry();
        }
        unsafe private void Inc(string rejestr1)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            if (rejestr1.Length == 2)
            {
                r1->ByteL++;
            }
            else
            {
                r1->Short++;
            }
            AktualizujRejestry();
        }
        unsafe private void Dec(string rejestr1)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            if (rejestr1.Length == 2)
            {
                r1->ByteL--;
            }
            else
            {
                r1->Short--;
            }
            AktualizujRejestry();
        }
        unsafe private void Shl(string rejestr1)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            if (rejestr1.Length == 2)
            {
                r1->ByteL= (byte)(r1->ByteL >> 1);
            }
            else
            {
                r1->Short = (ushort)(r1->Short >> 1);
            }
            AktualizujRejestry();
        }
        unsafe private void Shr(string rejestr1)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            if (rejestr1.Length == 2)
            {
                r1->ByteL = (byte)(r1->ByteL << 1);
            }
            else
            {
                r1->Short = (ushort)(r1->Short << 1);
            }
            AktualizujRejestry();
        }
        unsafe private void Xor(string rejestr1, string rejestr2)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Rejestr* r2 = SprawdzRejestr(rejestr2);
            if (rejestr1.Length == 2)
            {
                r1->ByteL = (byte)(r1->ByteL^(byte)r2->Short);
            }
            else
            {
                r1->Short = (ushort)(r1->Short ^ r2->Short);
            }
            AktualizujRejestry();
        }
        unsafe private void And(string rejestr1, string rejestr2)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Rejestr* r2 = SprawdzRejestr(rejestr2);
            if (rejestr1.Length == 2)
            {
                r1->ByteL = (byte)(r1->ByteL & (byte)r2->Short);
            }
            else
            {
                r1->Short = (ushort)(r1->Short & r2->Short);
            }
            AktualizujRejestry();
        }
        unsafe private void Or(string rejestr1, string rejestr2)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Rejestr* r2 = SprawdzRejestr(rejestr2);
            if (rejestr1.Length == 2)
            {
                r1->ByteL = (byte)(r1->ByteL | (byte)r2->Short);
            }
            else
            {
                r1->Short = (ushort)(r1->Short | r2->Short);
            }
            AktualizujRejestry();
        }
        unsafe private void Cmp(string rejestr1, string rejestr2)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Rejestr* r2 = SprawdzRejestr(rejestr2);
            int buffor;
            if (rejestr1.Length == 2)
            {
                buffor=r1->ByteL - (byte)r2->Short;
                if (buffor == 0)
                {
                    Flaga.Ef = true;
                    EF.Text = "1";
                }
                else {
                    Flaga.Ef = false;
                    EF.Text = "0";
                }
                if (buffor < 0)
                {
                    Flaga.Lf = true;
                    LF.Text = "1";
                }
                else
                {
                    Flaga.Lf = false;
                    LF.Text = "0";
                }
            }
            else
            {
                buffor = r1->Short - r2->Short;
                if (buffor == 0)
                {
                    Flaga.Ef = true;
                    EF.Text = "1";
                }
                else
                {
                    Flaga.Ef = false;
                    EF.Text = "0";
                }
                if (buffor < 0)
                {
                    Flaga.Lf = true;
                    LF.Text = "1";
                }
                else
                {
                    Flaga.Lf = false;
                    LF.Text = "0";
                }
            }
            AktualizujRejestry();
        }
        private void Jmp(ushort liczba) {
            Ip.Short = (ushort)(liczba-1);
            IPH.Text = LiczbaNaString(ref Ip.ByteH);
            IPL.Text = LiczbaNaString(ref Ip.ByteL);
        }
        private void Je(ushort liczba)
        {
            if (Flaga.Ef)
            {
                Ip.Short = (ushort)(liczba - 1);
                IPH.Text = LiczbaNaString(ref Ip.ByteH);
                IPL.Text = LiczbaNaString(ref Ip.ByteL);
            }
        }
        private void Jl(ushort liczba)
        {
            if (Flaga.Lf)
            {
                Ip.Short = (ushort)(liczba - 1);
                IPH.Text = LiczbaNaString(ref Ip.ByteH);
                IPL.Text = LiczbaNaString(ref Ip.ByteL);
            }
        }
        private void Jle(ushort liczba)
        {
            if (Flaga.Lf||Flaga.Ef)
            {
                Ip.Short = (ushort)(liczba - 1);
                IPH.Text = LiczbaNaString(ref Ip.ByteH);
                IPL.Text = LiczbaNaString(ref Ip.ByteL);
            }
        }
        unsafe private void Pop(string rejestr1)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            if (rejestr1.Length == 2)
            {
                r1->ByteL=Stos1.ByteL;
            }
            else
            {
                r1->Short = Stos1.Short;
            }
            Stos1.Short = Stos2.Short;
            Stos2.Short = Stos3.Short;
            Stos3.Short = 0;
            AktualizujRejestry();
        }
        unsafe private void Push(string rejestr1)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Stos3.Short = Stos2.Short;
            Stos2.Short = Stos1.Short;
            if (rejestr1.Length == 2)
            {
                Stos1.Short = r1->ByteL;
            }
            else
            {
                Stos1.Short = r1->Short;
            }
            SH1.Text = LiczbaNaString(ref Stos1.ByteH);
            SL1.Text = LiczbaNaString(ref Stos1.ByteL);
            SH2.Text = LiczbaNaString(ref Stos2.ByteH);
            SL2.Text = LiczbaNaString(ref Stos2.ByteL);
            SH3.Text = LiczbaNaString(ref Stos3.ByteH);
            SL3.Text = LiczbaNaString(ref Stos3.ByteL);
        }
        private void Int(ushort liczba)
        {
            if (liczba == 21)
            {
                switch (A.ByteL) {
                    case 1:
                        Int21H1();//wejscie z echem
                        break;
                    case 2:
                        Int21H2();//wyjscie
                        break;
                    case 3:
                        Int21H3();//status wejscie
                        break;
                    case 4:
                        Int21H4();//czyszczenie wejscia
                        break;
                    case 5:
                        Int21H5();//czyszczenie wyjscia
                        break;
                }
            }
            else if (liczba == 10) {
                switch (A.ByteL)
                {
                    case 1://zmiana koloru konsoli out
                        Int10H1();
                        break;
                    case 2://zmiana wielkosci czcionki konsoli out
                        Int10H2();
                        break;
                }
            } 
        }
        unsafe private void Out(string rejestr1)
        {
            Rejestr* r1 = SprawdzRejestr(rejestr1);
            Wyjscie.wyjscie = r1->ByteL;
            OUT.Text = LiczbaNaString(ref Wyjscie.wyjscie);
            if (OUT.Text[0] == '1')
            {
                D0.Fill = Brushes.Green;
            }
            else {
                D0.Fill = Brushes.Red;
            }
            if (OUT.Text[1] == '1')
            {
                D1.Fill = Brushes.Green;
            }
            else
            {
                D1.Fill = Brushes.Red;
            }
            if (OUT.Text[2] == '1')
            {
                D2.Fill = Brushes.Green;
            }
            else
            {
                D2.Fill = Brushes.Red;
            }
            if (OUT.Text[3] == '1')
            {
                D3.Fill = Brushes.Green;
            }
            else
            {
                D3.Fill = Brushes.Red;
            }
            if (OUT.Text[4] == '1')
            {
                D4.Fill = Brushes.Green;
            }
            else
            {
                D4.Fill = Brushes.Red;
            }
            if (OUT.Text[5] == '1')
            {
                D5.Fill = Brushes.Green;
            }
            else
            {
                D5.Fill = Brushes.Red;
            }
            if (OUT.Text[6] == '1')
            {
                D6.Fill = Brushes.Green;
            }
            else
            {
                D6.Fill = Brushes.Red;
            }
            if (OUT.Text[7] == '1')
            {
                D7.Fill = Brushes.Green;
            }
            else
            {
                D7.Fill = Brushes.Red;
            }
        }
        private void Int21H1() {
            if (KONSOLAIN.Text.Length == 0)
            {
                B.ByteL = 0;
            }
            else {
                B.ByteL = Convert.ToByte(KONSOLAIN.Text[0]);
                KONSOLAOUT.Text += (char)B.ByteL;
                KONSOLAIN.Text = KONSOLAIN.Text.Remove(0, 1);
            }
            BH.Text = LiczbaNaString(ref B.ByteH);
            BL.Text = LiczbaNaString(ref B.ByteL);
        }
        private void Int21H2()
        {
            if (B.ByteL != 0)
            {
                KONSOLAOUT.Text += (char)B.ByteL;
            }
        }
        private void Int21H3()
        {
            if (KONSOLAIN.Text.Length == 0)
            {
                B.ByteL = 0;
            }
            else {
                B.ByteL = 1;
            }
            BH.Text = LiczbaNaString(ref B.ByteH);
            BL.Text = LiczbaNaString(ref B.ByteL);
        }
        private void Int21H4()
        {
            KONSOLAIN.Text = "";
        }
        private void Int21H5()
        {
            KONSOLAOUT.Text = "";
        }
        private void Int10H1()
        {
            SolidColorBrush kolor = new SolidColorBrush(Color.FromArgb(255, B.ByteL, C.ByteL, D.ByteL));
            KONSOLAOUT.Foreground = kolor;
        }
        private void Int10H2()
        {
            if (B.ByteL != 0)
            {
                KONSOLAOUT.FontSize = B.ByteL;
            }
        }
    }
}
