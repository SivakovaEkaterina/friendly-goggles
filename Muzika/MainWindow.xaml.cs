using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Text.RegularExpressions;

namespace Muzika
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int i = 0;
        public string[] mus = new string[] {};
        public bool peremesh = false;
        public string[] mushhh = new string[] { };
        public string[] musf = new string[] { };
        public int vrema = 0, minvrem = 0, rabota = -1;
        public MainWindow()
        {
            InitializeComponent();
            media.Volume = 0.7;
            Thread slid = new Thread(_ =>
            {

                while (true)
                {
                    this.Dispatcher.Invoke(new Action(() => Polzunok.Value = media.Position.Ticks));
                    //значение слайдера = позиции музыки в тиках
                    if (rabota == 1)
                    {
                        vrema++;
                        if (vrema == 60)
                        {
                            minvrem++;
                        }
                        string min = "", sec = "";
                        if (vrema < 10)
                        {
                            sec = "0" + Convert.ToString(vrema);
                        }
                        else { sec = Convert.ToString(vrema); }
                        if (minvrem < 10)
                        {
                            min = "0" + Convert.ToString(minvrem);
                        }
                        else { min = Convert.ToString(minvrem); }
                        this.Dispatcher.Invoke(new Action(() => vrem.Text = min + ":" + sec));
                    }
                    Thread.Sleep(1000);
                }
            });
            slid.Start();
        }


        private void Noway_Dlina(object sender, RoutedEventArgs e)
        {
            Polzunok.Maximum = media.NaturalDuration.TimeSpan.Ticks;
        }

        private void Izmenenie_vremeni(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(Polzunok.Value));
        }

        private void Proigrish(object sender, RoutedEventArgs e)
        {
            if (mus.Length > 0)
            {
                media.Source = new Uri(mus[i]);
                rabota = 1;
                media.Play();
                vrema = 0;
                minvrem = 0;
            }
        }

        private void Pauses(object sender, RoutedEventArgs e)
        {
            if (mus.Length > 0)
            {
                media.Source = new Uri(mus[i]);
                media.Pause();
                rabota = -1;
            }
        }

        private void Beforplay(object sender, RoutedEventArgs e)
        {
            if (mus.Length > 0)
            {
                if (i == 0)
                {
                    i = mus.Length - 1;
                }
                else
                {
                    i--;
                }
                media.Source = new Uri(mus[i]);
                vrema = 0;
                minvrem = 0;
                rabota = 1;
            }
        }

        private void Nextplay(object sender, RoutedEventArgs e)
        {
            if (mus.Length > 0)
            {
                if (i == mus.Length - 1)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
                media.Source = new Uri(mus[i]);
                vrema = 0;
                minvrem = 0;
                rabota = 1;
            }
        }

        private void Papochka_s_musikoy(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog diolog = new CommonOpenFileDialog { IsFolderPicker = true};
            var dil = diolog.ShowDialog();
            List<string> list = new List<string>();
            if (dil == CommonFileDialogResult.Ok)
            {
                /*Regex regex = new Regex(@"(\w*).mp3");*/
                string[] ppp = Directory.GetFiles(diolog.FileName);
                foreach (string s in ppp)
                {
                    if (s.EndsWith("mp3"))
                    {
                        list.Add(s);
                    }
                    /*
                    MatchCollection match = regex.Matches(s);
                    if (match.Count > 0)
                    {
                        foreach (Match m in match)
                        {
                            list.Add(m.Value);
                        }
                    }*/
                }
                string[] pp = list.ToArray();
                mus = pp;
                musf = pp;
                Spisok.ItemsSource = mus;
            }
        }

        private void Vibor(object sender, SelectionChangedEventArgs e)
        {
            if (Spisok.SelectedItem != null)
            {
                int y = 0;
                string seleck = "";
                seleck = Spisok.SelectedItem.ToString();
                foreach (var ti in mus)
                {
                    if (seleck == ti)
                    {
                        i = y;
                    }
                    y++;
                }

                media.Source = new Uri(mus[i]);
                media.Play();
            }
            
        }
        private int Randomchisl(int shet)
        {
            Random random = new Random();
            int chet = random.Next(0, shet);
            return chet;
        }
        private void Peremeshat(object sender, RoutedEventArgs e)
        {
            if (peremesh == false)
            {
                List<int> list = new List<int>();
                string[] muss = mus;
                int n = 0;
                if (mus.Length>0)
                {
                    int y = mus.Length;
                    int chet = Randomchisl(y);
                    int j = 0;
                    while (n < mus.Length)
                    {
                        PROVERKARABOTI.Text =Convert.ToString(chet)+" ";
                        foreach (int u in list)
                        {
                            if (u == chet)
                            {
                                j++;
                            }
                        }
                        if (j > 0)
                        {
                            j = 0;
                            chet = Randomchisl(y);
                        }
                        else
                        {
                            list.Add(chet);
                            n++;
                        }
                    }
                    int nn = 0;
                    foreach (int u in list)
                    {
                        muss[nn] = mus[u];
                        nn++;
                    }
                    mus = muss;
                }
                peremesh = true;
                Spisok.ItemsSource = mus;
            }
            else
            {
                mus = musf;
                peremesh = false;
            }
            
        }
        
    }
}
