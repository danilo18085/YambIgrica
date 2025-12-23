using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yamb
{
    public class YambIgra
    {
        public int[] Kocke { get; set; } // tacno 6 kocki

        public int?[,] Celije { get; set; } //16 x 10
        
        public int[] Sume { get; set; } //Ima ih 3 za sva 3 odseka

        public int Bacanje { get; set; } //od nultog do treceg

        public bool[] Selektovane_kocke {  get; set; } //true ili false za svaku kocku

        public int?[] Selektovano_polje { get; set; } //2 vrednosti za 2 koordinate

        public bool Najavljeno { get; set; } // Jeste ili nije najvaljeno nesto 

        public int Broj_upisanih_polja  { get; set; } //Dozvoljava pristup poslednjoj koloni

        public bool Rucna_je_dozvoljena {  get; set; } //Da li dozvoljava upis u rucnu


        public YambIgra()
        {
            Kocke = new int[6];
            Celije = new int?[16,10];
            for(int i = 6; i < 16; i++)
            {
                if (i == 6 || i == 9 || i == 15)
                    for (int j = 0; j < 10; j++)
                        Celije[i, j] = 0;
                else
                    continue;
            }
            Sume = new int[3];
            Bacanje = 0;
            Selektovane_kocke = new bool[6];
            Selektovano_polje = new int?[2];
            Najavljeno = false;
            Broj_upisanih_polja = 0;
            Rucna_je_dozvoljena = true;
        }


        public void Spremi_novu_logika()
        {
            this.Bacanje = 0;
            for (int i = 0; i < 6; i++)
                this.Selektovane_kocke[i] = false;

            this.Selektovano_polje[0] = null;
            this.Selektovano_polje[1] = null;
            this.Najavljeno = false;
            this.Rucna_je_dozvoljena = true;
        }

        public void Baci_kocke()
        {
            Random rand = new Random();
            for (int i = 0; i < 6; i++)
                if (!Selektovane_kocke[i])
                    Kocke[i] = rand.Next(1, 7);
        }

        public bool Da_li_je_selektovano_polje_puno()
        {
            if (this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] != null)
                return true;
            else
                return false;
        }

        public bool Redosled_je_ok() //Redosled kolone
        {
            //Smatra se da je slektovano polje sigurno prazno

            switch (this.Selektovano_polje[1])
            {
                case 0: //Kolona (dole)
                    {
                        if (this.Selektovano_polje[0] == 0)
                            return true;
                        else
                        {
                            if (this.Selektovano_polje[0] == 7 || this.Selektovano_polje[0] == 10)
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] - 2, 0] == null) //Da li je polje iznad selektovanog prazno * 2
                                    return false;

                                return true;
                            }
                            else
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] - 1, 0] == null) //Da li je polje iznad selektovanog prazno
                                    return false;

                                return true;
                            }
                        }
                    }
                case 2: //Kolona (gore)
                    {
                        if (this.Selektovano_polje[0] == 14)
                            return true;
                        else
                        {
                            if(this.Selektovano_polje[0] == 8 || this.Selektovano_polje[0] == 5)
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] + 2, 2] == null) //Da li je polje ispod selektovanog prazno * 2
                                    return false;

                                return true;
                            }
                            else
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] + 1, 2] == null) //Da li je polje ispod selektovanog prazno
                                    return false;

                                return true;
                            }
                        }
                    }
                case 3: //Najava
                    {
                        if (this.Najavljeno == true)
                            return true;

                        return false;
                    }
                case 6: //Od sredine ka krajevima (jebem ti logiku mrtvu)
                    {
                        if (this.Selektovano_polje[0] == 7 || this.Selektovano_polje[0] == 8)
                                return true;
                        else if (this.Selektovano_polje[0] == 5)
                        {
                            if (this.Celije[7, 6] == null)
                                return false;

                            return true;
                        }
                        else if (this.Selektovano_polje[0] == 10)
                        {
                            if (this.Celije[8,6] == null)
                                return false;

                            return true;
                        }
                        else
                        {
                            if (this.Selektovano_polje[0] < 7)
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] + 1, 6] == null) //Da li je polje ispod selektovanog prazno
                                    return false;
                                else
                                    return true;
                            }
                            else // Jedino preostalo je da je vece od 8, pa ne mora da se naglasava
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] - 1, 6] == null) //Da li je polje iznad selektovanog prazno
                                    return false;
                                else
                                    return true;
                            }
                        }
                    }
                case 7: //Od krajeva ka sredini
                    {
                        if (this.Selektovano_polje[0] == 0 || this.Selektovano_polje[0] == 14)
                            return true;
                        else if (this.Selektovano_polje[0] == 7)
                        {
                            if (this.Celije[5, 7] == null)
                                return false;

                            return true;
                        }
                        else if (this.Selektovano_polje[0] == 8)
                        {
                            if (this.Celije[10, 7] == null)
                                return false;

                            return true;
                        }
                        else
                        {
                            if (this.Selektovano_polje[0] < 7)
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] - 1, 7] == null) //Da li je polje iznad selektovanog prazno
                                    return false;
                                else
                                    return true;
                            }
                            else // Jedino preostalo je da je vece od 8, pa ne mora da se naglasava
                            {
                                if (this.Celije[(int)this.Selektovano_polje[0] + 1, 7] == null) //Da li je polje ispot selektovanog prazno
                                    return false;
                                else
                                    return true;
                            }
                        }
                    }
                case 8: //Kolona "O"
                    {
                        if (this.Broj_upisanih_polja < 117)
                            return false;
                        else
                        {
                            if (this.Selektovano_polje[0] == 0)
                                return true;
                            else
                            {
                                if (this.Selektovano_polje[0] == 7 || this.Selektovano_polje[0] == 10)
                                {
                                    if (this.Celije[(int)this.Selektovano_polje[0] - 2, 8] == null) //Da li je polje iznad selektovanog prazno * 2
                                        return false;

                                    return true;
                                }
                                else
                                {
                                    if (this.Celije[(int)this.Selektovano_polje[0] - 1, 8] == null) //Da li je polje iznad selektovanog prazno
                                        return false;

                                    return true;
                                }
                            }
                        }
                    }
                default: //Kolonu D ne ispitujemo namerno, a i sve ostale
                    return true;
            }
        }

        public int Upis()
        {
            if (this.Selektovano_polje[1] == 4 && !this.Rucna_je_dozvoljena) //Provera za nedozvoljenu rucnu
            {
                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
            else
            {
                int[] niz_samo_selektovanih_kocki = this.Kocke
                            .Where((vr, indeks) => this.Selektovane_kocke[indeks])
                            .ToArray();


                if (this.Selektovano_polje[0] >= 0 && this.Selektovano_polje[0] <= 5)
                    return Upis_prostih(niz_samo_selektovanih_kocki);

                else if (this.Selektovano_polje[0] == 7 || this.Selektovano_polje[0] == 8)
                    return Upis_min_max(niz_samo_selektovanih_kocki);

                else
                {
                    switch (Selektovano_polje[0])
                    {
                        case 10: return Upisi_kentu(niz_samo_selektovanih_kocki);
                        case 11: return Upisi_triling(niz_samo_selektovanih_kocki);
                        case 12: return Upisi_ful(niz_samo_selektovanih_kocki);
                        case 13: return Upisi_kare(niz_samo_selektovanih_kocki);
                        case 14: return Upisi_yamb(niz_samo_selektovanih_kocki);
                        default: return 0;
                    }
                }
            }
        }

        public int Upis_prostih(int[] niz) //Upis od 1-6
        {
            int br = 0;
            for (int i = 0; i < niz.Length; i++) 
                if(niz[i] == this.Selektovano_polje[0] + 1)
                    br++;


            if (this.Selektovano_polje[1] == 9 && br != 5) //Provera za M kolonu
            {
                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }

            br *= (int)this.Selektovano_polje[0] + 1;
            this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = br;

            return br; 
        }

        public int Upis_min_max(int[] niz) 
        //Ako je doslo do ovde, onda niz ima sigurno 5 elementa, ne treba provera
        {
            int br = 0;
            for (int i = 0; i < niz.Length; i++)
                br += niz[i];

            if (this.Selektovano_polje[1] == 9) //Provera za M
            {
                if ((this.Selektovano_polje[0] == 7 && br != 30) || (this.Selektovano_polje[0] == 8 && br != 5))
                {
                    this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                    return 0;
                }
            }

            this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = br;
            return br;
        }

        public int Upisi_kentu(int[] niz)
        {
            if (niz.Length != 5)
            {
                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
            else
            {
                Array.Sort(niz);
                bool jeste_kenta = niz.SequenceEqual(new[] { 1, 2, 3, 4, 5 }) ||
                                   niz.SequenceEqual(new[] { 2, 3, 4, 5, 6 });

                if (jeste_kenta)
                {
                    int rez = 66 - 10 * (this.Bacanje - 1);

                    if (this.Selektovano_polje[1] == 9 && rez != 66) //Provera za M kolonu
                    {
                        this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                        return 0;
                    }

                    this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = rez;
                    return rez;
                }

                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
        }

        public int Upisi_ful(int[] niz)
        {
            if (niz.Length != 5)
            {
                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
            else
            {
                var grupisano = niz
                                .GroupBy(x => x)
                                .Select(g => g.Count())
                                .OrderByDescending(x => x)
                                .ToArray();

                bool jeste_ful = grupisano.Length == 2 && grupisano[0] == 3 && grupisano[1] == 2;

                if(jeste_ful)
                {
                    int rez = 30 + niz.Sum();

                    if (this.Selektovano_polje[1] == 9 && rez != 58) //Provera za M kolonu
                    {
                        this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                        return 0;
                    }

                    this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = rez;
                    return rez;
                }

                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
        }

        public int Upisi_triling(int[] niz)
        {
            if (niz.Length != 3)
            {
                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
            else
            {
                bool jeste_triling = true;
                for (int i = 1; i < 3; i++)
                    if (niz[i] != niz[i - 1])
                        jeste_triling = false;

                if(jeste_triling)
                {
                    int rez = 20 + niz[0] * 3;

                    if (this.Selektovano_polje[1] == 9 && rez != 38) //Provera za M kolonu
                    {
                        this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                        return 0;
                    }

                    this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = rez;
                    return rez;
                }

                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
        }

        public int Upisi_kare(int[] niz)
        {
            if (niz.Length != 4)
            {
                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
            else
            {
                bool jeste_kare = true;
                for (int i = 1; i < 4; i++)
                    if (niz[i] != niz[i - 1])
                        jeste_kare = false;

                if (jeste_kare)
                {
                    int rez = 40 + niz[0] * 4;

                    if (this.Selektovano_polje[1] == 9 && rez != 64) //Provera za M kolonu
                    {
                        this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                        return 0;
                    }

                    this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = rez;
                    return rez;
                }

                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
        }

        public int Upisi_yamb(int[] niz)
        {
            if (niz.Length != 5)
            {
                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
            else
            {
                bool jeste_yamb = true;
                for (int i = 1; i < 5; i++)
                    if (niz[i] != niz[i - 1])
                        jeste_yamb = false;

                if (jeste_yamb)
                {
                    int rez = 50 + niz[0] * 5;

                    if (this.Selektovano_polje[1] == 9 && rez != 80) //Provera za M kolonu
                    {
                        this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                        return 0;
                    }

                    this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = rez;
                    return rez;
                }

                this.Celije[(int)this.Selektovano_polje[0], (int)this.Selektovano_polje[1]] = 0;
                return 0;
            }
        }

        public int Update(int rez) //Biva pozvano nakon upisa!!
        {
            if (this.Selektovano_polje[0] >= 0 && this.Selektovano_polje[0] <= 5) //1-6
            {
                this.Celije[6, (int)this.Selektovano_polje[1]] += rez;
                this.Sume[0] += rez;

                bool sve_sest = true;
                for (int i = 0; i < 6; i++)
                {
                    if (this.Celije[i, (int)Selektovano_polje[1]] == null)
                    {
                        sve_sest = false;
                        break;
                    }
                }
                if (this.Celije[6, (int)this.Selektovano_polje[1]] >= 60 && sve_sest)
                {
                    this.Celije[6, (int)this.Selektovano_polje[1]] += 30;
                    this.Sume[0] += 30;
                }

                if (this.Selektovano_polje[0] == 0 && Sve_tri()) //Ako je selektovana jedinica, onda mora i provera "sva tri" jbg
                {
                    if (this.Celije[7, (int)this.Selektovano_polje[1]] != 0 && this.Celije[8, (int)this.Selektovano_polje[1]] != 0)
                    {
                        this.Celije[9, (int)this.Selektovano_polje[1]] =
                        this.Celije[0, (int)this.Selektovano_polje[1]] *
                        (this.Celije[7, (int)this.Selektovano_polje[1]] -
                        this.Celije[8, (int)this.Selektovano_polje[1]]);

                        this.Sume[1] += (int)this.Celije[9, (int)this.Selektovano_polje[1]];
                    }
                    else
                        this.Celije[9, (int)this.Selektovano_polje[1]] = 0;

                    return 3; //Flag da moraju dva puta da se sume refreshuju na canvasu
                }
                
                return 0;
                //dodali smo rez i u lokalnu sumu i u generalnu sumu i uzeli smo u obzir +30
            }
            else if(this.Selektovano_polje[0] == 7 || this.Selektovano_polje[0] == 8) //min-max
            {
                if (Sve_tri())
                {
                    if (this.Celije[7, (int)this.Selektovano_polje[1]] != 0 && this.Celije[8, (int)this.Selektovano_polje[1]] != 0)
                    {
                        this.Celije[9, (int)this.Selektovano_polje[1]] =
                        this.Celije[0, (int)this.Selektovano_polje[1]] *
                        (this.Celije[7, (int)this.Selektovano_polje[1]] -
                        this.Celije[8, (int)this.Selektovano_polje[1]]);

                        this.Sume[1] += (int)this.Celije[9, (int)this.Selektovano_polje[1]];
                    }
                    else
                        this.Celije[9, (int)this.Selektovano_polje[1]] = 0;

                    return 1;
                }
                else
                    return -1;
            }
            else //kompleksna polja
            {
                this.Celije[15, (int)this.Selektovano_polje[1]] += rez;
                this.Sume[2] += rez;
                return 2;
            }
        }

        public bool Sve_tri()
        {
            bool sve_tri = true;
            if (this.Celije[7, (int)this.Selektovano_polje[1]] == null ||
                this.Celije[8, (int)this.Selektovano_polje[1]] == null ||
                this.Celije[0, (int)this.Selektovano_polje[1]] == null)
                sve_tri = false;

            return sve_tri;
        }

        public bool Jeste_kraj()
        {
            if (this.Broj_upisanih_polja == 130)
                return true;

            return false;
        }

    }
}