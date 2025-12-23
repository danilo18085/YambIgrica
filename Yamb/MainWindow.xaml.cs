using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Yamb
{

    public partial class MainWindow : Window
    {
        public YambIgra igra;
        public Dictionary<(int, int), Border> mapa;

        public MainWindow()
        {
            InitializeComponent();

            igra = new YambIgra();
            mapa = new Dictionary<(int, int), Border>();
            
            NacrtajNebitneCelije();
        }


        private void NacrtajNebitneCelije()
        {
            //Crta ove nebitne celije
            for (int i = 1; i < 17; i++)
            {
                for (int j = 1; j < 12; j++)
                {
                    if (j == 11 && (i <= 7 || i == 10 || i == 16))
                        continue;
                    TextBlock textBlock = new TextBlock
                    {
                        Text = "",
                        FontSize = 20,
                        Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffffff")),
                        FontFamily = new FontFamily("Consolas"),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(1)
                    };
                    

                    Border border = new Border
                    {
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.LightGray,
                        BorderThickness = new Thickness(1),
                        Child = textBlock
                    };

                    if (i != 7 && i != 10 && i != 16 && j != 11)
                        border.MouseDown += Border_Je_Kliknut;

                    if (i == 7 || i == 10 || i == 16)
                        border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#14907e"));

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);

                    gridTabela.Children.Add(border);

                    mapa[(i, j)] = border;
                }
            }
        }

        private void Najavi_dugme(object sender, RoutedEventArgs e)
        {
            //Moze da se najavi samo u prvom bacanju
            if (int.Parse(bacanja_labela.Content.ToString()) == 1)
            {
                //Mora da postoji selektovano polje
                if (igra.Selektovano_polje[0] == null)
                    MessageBox.Show("Niste selektovali nijedno polje!");
                else
                {
                    //Polje koje je selektovano mora da bude najava (3)
                    if (igra.Selektovano_polje[1] != 3)
                        MessageBox.Show("Ne možete najaviti ovo polje!");
                    else
                    {
                        Button dugme = sender as Button;
                        dugme.IsEnabled = false;
                        igra.Najavljeno = true;
                    }
                }
            }
            else
                MessageBox.Show("Možete najaviti samo u prvom bacanju!");
        }

        private void Border_Je_Kliknut(object sender, MouseButtonEventArgs e)
        {
            if(!igra.Najavljeno || igra.Bacanje == 0)
            {
                Border bord = sender as Border;

                int i = Grid.GetRow(bord);
                int j = Grid.GetColumn(bord);

                //Ako je polje vec selektovano, onda ga deselektuj
                if (igra.Selektovano_polje[0] == i - 1 && igra.Selektovano_polje[1] == j - 1)
                {
                    bord.BorderBrush = Brushes.LightGray;
                    bord.BorderThickness = new Thickness(1);
                    igra.Selektovano_polje[0] = null;
                    igra.Selektovano_polje[1] = null;
                }
                //Ako nije onda ga selektuj i deselektuj vec postavljeno polje
                else
                {
                    //deselektovanje starog polja ako postoji
                    if (igra.Selektovano_polje[0] != null)
                    {
                        Border trazeniBorder = mapa[((int)igra.Selektovano_polje[0] + 1, (int)igra.Selektovano_polje[1] + 1)];
                        trazeniBorder.BorderBrush = Brushes.LightGray;
                        trazeniBorder.BorderThickness = new Thickness(1);
                        //Ne moramo da cistimo polja jer ce svakako proci kroz sledeci deo
                    }

                    //novo selektovanje
                    bord.BorderBrush = Brushes.Red;
                    bord.BorderThickness = new Thickness(3);
                    igra.Selektovano_polje[0] = i - 1;
                    igra.Selektovano_polje[1] = j - 1;
                }
            }
        }


        private void Kocka_je_kliknuta(object sender, MouseButtonEventArgs e) 
        {
            //--------------------------------------------------------
            Pusti_zvuk_selektovanja();
            //--------------------------------------------------------
            Border kocka = sender as Border;
            int broj = int.Parse(kocka.Name[kocka.Name.Length - 1].ToString());

            //Ako je kocka vec selektovana
            if (igra.Selektovane_kocke[broj])
            {
                //farbanje nazad u lighgray
                kocka.BorderBrush = Brushes.LightGray;
                kocka.BorderThickness = new Thickness(1);
                //deselektovano u igri
                igra.Selektovane_kocke[broj] = false;
            }
            else
            {
                if (igra.Selektovane_kocke.Count(v => v) == 5)
                    MessageBox.Show("Ne možete selektovati više od 5 kockica!");
                else
                {
                    //farbanje bordera u crveno
                    kocka.BorderBrush = Brushes.Yellow;
                    kocka.BorderThickness = new Thickness(3);
                    //selektovano u igri
                    igra.Selektovane_kocke[broj] = true;
                }
            }
        }

        private void Nacrtaj_kocke(int bacanja)
        {
            for (int i = 0; i < 6; i++)
            {
                string ime = $"slika_{i}";
                Image img = (Image)this.FindName(ime);
                if(bacanja == 0 || igra.Selektovane_kocke[i])
                    img.Source = new BitmapImage(new Uri($"Images/kocka_{igra.Kocke[i]}.png", UriKind.Relative));
                else
                    Promeni_kocku_animacija(img, i);
            }

        }

        private void Promeni_kocku_animacija(Image img, int broj)
        {
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));

            fadeOut.Completed += (s, e) =>
            {
                img.Source = new BitmapImage(new Uri($"Images/kocka_{igra.Kocke[broj]}.png", UriKind.Relative));

                DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
                img.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            };

            img.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

        private void Baci_kocke_dugme(object sender, RoutedEventArgs e)
        {
            //--------------------------------------------------------
            Pusti_zvuk_bacanja();
            //--------------------------------------------------------
            int bacanja = int.Parse(bacanja_labela.Content.ToString());
            //--------------------------------------------------------
            if (bacanja != 0) //Prvera za rucnu bool
                if (igra.Selektovane_kocke.Count(v => v) != 0)
                    igra.Rucna_je_dozvoljena = false;
                else
                    igra.Rucna_je_dozvoljena = true;
            //--------------------------------------------------------
            igra.Baci_kocke();
            Nacrtaj_kocke(bacanja);

            if (bacanja == 0)
                gridKocke.Visibility = Visibility.Visible;

            if(bacanja == 2)
            {
                Button dugme = sender as Button;
                dugme.IsEnabled = false;
            }

            bacanja_labela.Content = (bacanja + 1).ToString();
            igra.Bacanje = bacanja + 1;
        }

        private void Upisi_dugme(object sender, RoutedEventArgs e)
        {
            //Ako nije selektovano polje, nema upisivanja
            if (igra.Selektovano_polje[0] == null)
                MessageBox.Show("Niste selektovali nijedno polje!");
            else
            {
                //Da li je vec upisano u to polje
                if (igra.Da_li_je_selektovano_polje_puno())
                    MessageBox.Show("Polje koje ste selektovali vec ima upisanu vrednost");
                else
                {
                    //Da se ispita da li je moguće upisati tu kolonu zbog redosleda
                    if(!igra.Redosled_je_ok())
                        MessageBox.Show("Nije moguće upisati ovo polje!");
                    else
                    {
                        //Ako je min-max, onda mora da bude selektovano 5 kockica
                        if ((igra.Selektovano_polje[0] == 7 || igra.Selektovano_polje[0] == 8) && igra.Selektovane_kocke.Count(v => v) != 5)
                            MessageBox.Show("Nije moguće upisati!");
                        else
                        {
                            //Ako je ovde uslo, onda je upis sigurno dozvoljen!
                            int rez = igra.Upis();
                            Upisi_rezultat(rez);
                            Pusti_zvuk_upis();
                            int povratna = igra.Update(rez);
                            
                            //U odnosu na povratnu, refresh-ovati dva odre]ena polje
                            Refresh_rezultate(povratna);
                            
                            //povecati broj upisanih polja
                            igra.Broj_upisanih_polja++;

                            //Vratiti na pocetno stanje
                            Spremi_novu_model();
                            igra.Spremi_novu_logika();

                            //Proveriti da li je kraj
                            if (igra.Jeste_kraj())
                                Kraj();
                        }
                    }
                }
            }
        }

        private void Kraj()
        {
            gridKocke.Visibility = Visibility.Hidden;

            najavi_button.IsEnabled = false;
            baci_kocke_button.IsEnabled = false;
            upisi_button.IsEnabled = false;

            rezultat_labela.Visibility = Visibility.Visible;
            rezultat_labela.Content += (igra.Sume[0] + igra.Sume[1] + igra.Sume[2]).ToString();
        }

        private void Spremi_novu_model()
        {
            gridKocke.Visibility = Visibility.Hidden;
            bacanja_labela.Content = "0";

            najavi_button.IsEnabled = true;
            baci_kocke_button.IsEnabled = true;

            //deselekcija odigranog bordera
            Border trazeniBorder = mapa[((int)igra.Selektovano_polje[0] + 1, (int)igra.Selektovano_polje[1] + 1)];
            trazeniBorder.BorderBrush = Brushes.LightGray;
            trazeniBorder.BorderThickness = new Thickness(1);

            //Vracanje bordera na default svih kocki
            for (int i = 0; i < 6; i++)
            {
                string ime = $"poz_{i}";
                Border bor = (Border)this.FindName(ime);

                bor.BorderBrush = Brushes.LightGray;
                bor.BorderThickness = new Thickness(1);
            }
        }

        private void Refresh_rezultate(int povratna)
        {
            TextBlock tb;
            if (povratna == 0) // 0 = prva suma, 1 = druga suma, 2 = treca suma, 3 = i prva i druga suma, -1 = nista
            {
                tb = mapa[(7, (int)igra.Selektovano_polje[1] + 1)].Child as TextBlock;
                tb.Text = igra.Celije[6, (int)igra.Selektovano_polje[1]].ToString(); //lokalni rez

                tb = prvi_rez.Child as TextBlock;
                tb.Text = igra.Sume[0].ToString(); //glavni rez
                            }
            else if (povratna == 1)
            {
                tb = mapa[(10, (int)igra.Selektovano_polje[1] + 1)].Child as TextBlock;
                tb.Text = igra.Celije[9, (int)igra.Selektovano_polje[1]].ToString(); //lokalni rez

                tb = drugi_rez.Child as TextBlock;
                tb.Text = igra.Sume[1].ToString(); //glavni rez
            }
            else if (povratna == 2)
            {
                tb = mapa[(16, (int)igra.Selektovano_polje[1] + 1)].Child as TextBlock;
                tb.Text = igra.Celije[15, (int)igra.Selektovano_polje[1]].ToString(); //lokalni rez

                tb = treci_rez.Child as TextBlock;
                tb.Text = igra.Sume[2].ToString(); //glavni rez
            }
            else if(povratna == 3)
            {
                tb = mapa[(7, (int)igra.Selektovano_polje[1] + 1)].Child as TextBlock;
                tb.Text = igra.Celije[6, (int)igra.Selektovano_polje[1]].ToString(); //lokalni rez

                tb = prvi_rez.Child as TextBlock;
                tb.Text = igra.Sume[0].ToString(); //glavni rez

                tb = mapa[(10, (int)igra.Selektovano_polje[1] + 1)].Child as TextBlock;
                tb.Text = igra.Celije[9, (int)igra.Selektovano_polje[1]].ToString(); //lokalni rez

                tb = drugi_rez.Child as TextBlock;
                tb.Text = igra.Sume[1].ToString(); //glavni rez
            }

            //DODATO JE OVO OVDE NOVO YA GLAVNI REZULTAT

            najglavniji_rezultat_labela.Content = "Rezultat: ";
            najglavniji_rezultat_labela.Content += (igra.Sume[0] + igra.Sume[1] + igra.Sume[2]).ToString();
        }

        private void Upisi_rezultat(int rez)
        {
            //Upisuje rezultat i deselektuje border
            Border trazeniBorder = mapa[((int)igra.Selektovano_polje[0] + 1, (int)igra.Selektovano_polje[1] + 1)];

            trazeniBorder.BorderBrush = Brushes.LightGray;
            trazeniBorder.BorderThickness = new Thickness(1);

            TextBlock tb = trazeniBorder.Child as TextBlock;

            if (rez == 0)
                tb.Foreground = Brushes.Red;

            tb.Text = rez.ToString();
        }

        private void Pusti_zvuk_upis()
        {
            SoundPlayer plejer = new SoundPlayer("Sounds/upis_zvuk.wav");
            plejer.Load();
            plejer.Play();
        }

        private void Pusti_zvuk_bacanja()
        {
            SoundPlayer plejer = new SoundPlayer("Sounds/bacanje_zvuk.wav");
            plejer.Load();
            plejer.Play();
        }

        private void Pusti_zvuk_selektovanja()
        {
            SoundPlayer plejer = new SoundPlayer("Sounds/select_zvuk.wav");
            plejer.Load();
            plejer.Play();
        }

    }
}
