using System;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    public class State
    {
        public static int[,] lavirint;
        public static List<Portal> portali;
        public const int jacinaVatre = 2;
        public List<Kutija> plaveKutije = new List<Kutija>();
        public List<Kutija> narandzasteKutije = new List<Kutija>();

        State parent;
        public int markI, markJ; //vrsta i kolona
        public double cost;
        public int Level { get; set; }
        private int[,] moves1 = { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };    //obicni koraci
        private int[,] moves = { { 2, 1 }, { 1, 2 }, { -2, 1 }, { -1, 2 }, 
                               { -2, -1 }, { 1, -2 }, { 2, -1 }, { -1, -2 }};  //konjski galop

        //private int[,] moves = { { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }, { 6, 0 }, { 7, 0 }, { 8, 0 }, { 9, 0 }, 
        //                         { 0, -1 }, { 0, -2 }, { 0, -3 }, { 0, -4 }, { 0, -5 }, { 0, -6 }, { 0, -7 }, { 0, -8 }, { 0, -9 },
        //                         { -1, 0 }, { -2, 0 }, { -3, 0 }, { -4, 0 }, { -5, 0 }, { -6, 0 }, { -7, 0 }, { -8, 0 }, { -9, 0 }, 
        //                         { 0, 1 }, { 0, 2 }, { 0, 3 }, { 0, 4 }, { 0, 5 }, { 0, 6 }, { 0, 7 }, { 0, 8 }, { 0, 9 }};//top
        
        public bool jePokupio=false;

        public State sledeceStanje(int markI, int markJ,bool pokupio,double dodatnaCena)
        {
            State rez = new State();
            rez.markI = markI;
            rez.markJ = markJ;
            rez.parent = this;
            rez.cost = this.cost + 1 + dodatnaCena;
            rez.Level = this.Level + 1;
            foreach (Kutija temp in plaveKutije)
            {
                rez.plaveKutije.Add(new Kutija(temp));
            }

            foreach (Kutija temp in narandzasteKutije)
            {
                rez.narandzasteKutije.Add(new Kutija(temp));
            }
            rez.jePokupio = pokupio;
            return rez;
        }

        
        public List<State> mogucaSledecaStanja()
        {
            List<State> rez = new List<State>();

            if (isOnPortal())
            {
                foreach (Portal portal in portali)
                {
                    if (!(portal.X == markJ && portal.Y == markI))
                    {
                        int newI = portal.Y;
                        int newJ = portal.X;

                        bool pokupio = jePokupio || jePokupioSve(markI, markJ);
                        rez.Add(sledeceStanje(newI, newJ, pokupio,0));
                    }
                }

            }

            if (jePokupio)
            {

                for (int ind = 0; ind < moves.GetLength(0); ind++)
                {
                    int newI = this.markI + moves[ind, 0];
                    int newJ = this.markJ + moves[ind, 1];

                    if (isAllowedState(newI, newJ))
                    {
                        bool pokupio = jePokupio || jePokupioSve(markI, markJ);
                        double dodatnaCena = prelazPrekoVatre(newI, newJ);
                        rez.Add(sledeceStanje(newI, newJ, pokupio, dodatnaCena));
                    }
                }
            }else
            {



                for (int ind = 0; ind < moves1.GetLength(0); ind++)
                {
                    int newI = this.markI + moves1[ind, 0];
                    int newJ = this.markJ + moves1[ind, 1];

                    if (isAllowedState(newI, newJ))
                    {
                        bool pokupio = jePokupio || jePokupioSve(markI, markJ);
                        double dodatnaCena = prelazPrekoVatre(newI, newJ);
                        rez.Add(sledeceStanje(newI, newJ, pokupio, dodatnaCena));
                    }
                }



            }

          
            return rez;
        }



        public List<State> mogucaSledecaStanjaa()
        {
            List<State> rez = new List<State>();

            if (isOnPortal())
            {
                foreach (Portal portal in portali)
                {
                    if (!(portal.X == markJ && portal.Y == markI))
                    {
                        int newI = portal.Y;
                        int newJ = portal.X;

                        bool pokupio = jePokupio || jePokupioSve(markI, markJ);
                        rez.Add(sledeceStanje(newI, newJ, pokupio, 0));
                    }
                }

            }


            for (int ind = 0; ind < moves1.GetLength(0); ind++)
            {
                int newI = this.markI + moves1[ind, 0];
                int newJ = this.markJ + moves1[ind, 1];

                if (isAllowedState(newI, newJ))
                {
                    bool pokupio = jePokupio || jePokupioSve(markI, markJ);
                    double dodatnaCena = prelazPrekoVatre(newI, newJ);
                    rez.Add(sledeceStanje(newI, newJ, pokupio, dodatnaCena));
                }
            }


            return rez;
        }


        private double prelazPrekoVatre(int newI, int newJ)
        {
            double cena = 0;
            foreach (Kutija vatra in Main.vatre)
            {
                double rastojanje = Math.Sqrt(Math.Pow(newI - vatra.Y, 2) + Math.Pow(newJ - vatra.X, 2));
                if (rastojanje == 0)
                {
                    cena += 15000;
                }
                if (rastojanje < jacinaVatre)
                {
                    cena += 150 * jacinaVatre - rastojanje;
                }
                else if (rastojanje < 2 * jacinaVatre)
                {
                    cena += 20 * jacinaVatre - rastojanje;
                }
                else if (rastojanje < 3*jacinaVatre)
                {
                    cena += 10 * jacinaVatre - rastojanje;
                }
            }

            return cena;
        }

        private bool isOnPortal()
        {
            foreach (Portal portal in portali)
            {
                if (portal.X == markJ && portal.Y == markI)
                    return true;
            }
            return false;
        }

        private bool jePokupioSve(int I, int J)
        {
            int indZaBrisanje = -1;
            for (int i = 0; i < plaveKutije.Count; i++)
            {
                if (plaveKutije[i].X == J && plaveKutije[i].Y == I)
                {
                    indZaBrisanje = i;
                    break;
                }
            }

            if (indZaBrisanje != -1)
            {
                plaveKutije.RemoveAt(indZaBrisanje);
            }

            if (plaveKutije.Count == 0) //tek kad nadje sve plave, trazi naradzaste
            {

                indZaBrisanje = -1;
                for (int i = 0; i < narandzasteKutije.Count; i++)
                {
                    if (narandzasteKutije[i].X == J && narandzasteKutije[i].Y == I)
                    {
                        indZaBrisanje = i;
                        break;
                    }
                }

                if (indZaBrisanje != -1)
                {
                    narandzasteKutije.RemoveAt(indZaBrisanje);
                }
                 
            }

            return plaveKutije.Count == 0 && narandzasteKutije.Count == 0;
        }

        private bool isAllowedState(int newI, int newJ)
        {
            if (newI < 0 || newJ < 0)
            {
                return false;
            }

            if (newI >= Main.brojVrsta || newJ >= Main.brojKolona)
            {
                return false;
            }

            if (lavirint[newI, newJ] == 1)
            {
                return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            if (plaveKutije.Count != 0 ) 
            {
                return 100 * markI + markJ + (plaveKutije[0].ID + 1) * 10000;

            }else if (narandzasteKutije.Count != 0) //ukoliko ima naranzastih
                    return 100 * markI + markJ + (narandzasteKutije[0].ID + 1) * 10000;
            else // samo ako nema ni jednih ni drugih
                return 100 * markI + markJ;
           
        }
        public bool jePokupioKutije()
        {
            //return plaveKutije[0].X == markJ && plaveKutije[0].Y == markI;//radi samo kada ima jedna plava


            //radi za svaki slucaj i prati prioritet da se prvo pokupe plave pa tek onda narandzasta
            //vazi za slucaj kad se trazi da se pokupe kutije i koristi se samo u ADepthSearch-u
           
            if (narandzasteKutije.Count == 0 && plaveKutije.Count == 1)
            {
                return plaveKutije[0].X == markJ && plaveKutije[0].Y == markI;
            }
            else if (narandzasteKutije.Count == 1 && plaveKutije.Count == 0)
            {
                return narandzasteKutije[0].X == markJ && narandzasteKutije[0].Y == markI;
            }


            return jePokupio; //isto kao return plaveKutije.Count == 0 && narandzasteKutije.Count == 0;
            
            
        }
        public bool isKrajnjeStanje()
        {
            return Main.krajnjeStanje.markI == markI && Main.krajnjeStanje.markJ == markJ && jePokupio;
        }

        public List<State> path()
        {
            List<State> putanja = new List<State>();
            State tt = this;
            while (tt != null)
            {
                putanja.Insert(0, tt);
                tt = tt.parent;
            }
            return putanja;
        }

        
    }
}
