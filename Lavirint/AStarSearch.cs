﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Lavirint
{
    class AStarSearch
    {
        private List<State> sledecaStanja;
        public State search(State pocetnoStanje)
        {
            List<State> stanjaZaObradu = new List<State>();
            Hashtable predjeniPut = new Hashtable();
            stanjaZaObradu.Add(pocetnoStanje);

            while (stanjaZaObradu.Count > 0)
            {
                State naObradi = getBest(stanjaZaObradu);

                if (!predjeniPut.ContainsKey(naObradi.GetHashCode()))
                {
                    Main.allSearchStates.Add(naObradi);
                    if (naObradi.isKrajnjeStanje())
                    {
                        return naObradi;
                    }
                    predjeniPut.Add(naObradi.GetHashCode(), null);

                    if (naObradi.jePokupio)
                    {

                        sledecaStanja = naObradi.mogucaSledecaStanjaa();
                    }
                    sledecaStanja = naObradi.mogucaSledecaStanja();


                    //  List<State> sledecaStanja = naObradi.mogucaSledecaStanja();
                  
                    foreach (State s in sledecaStanja)
                    {
                        stanjaZaObradu.Add(s);
                    }
                }

                stanjaZaObradu.Remove(naObradi);
            }
            
            return null;
        }

        //funkcija odredjuje rastojanje
        public double heuristicFunction(State s)
        {
            return Math.Sqrt(Math.Pow(s.markI - Main.krajnjeStanje.markI, 2) + Math.Pow(s.markJ - Main.krajnjeStanje.markJ, 2))+s.cost;
        }

        public State getBest(List<State> stanja)
        {
            State rez = null;
            double min = Double.MaxValue;

            foreach (State s in stanja)
            {
                double h = heuristicFunction(s);
                if (h < min)
                {
                    min = h;
                    rez = s;
                }
            }
            return rez;
        }



    }
}
