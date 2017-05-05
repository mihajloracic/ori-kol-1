using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Lavirint
{
    class ADepthSearch
    {
        public State search(State pocetnoStanje)
        {
            List<State> stanjaNaObradi = new List<State>();
            Hashtable predjeniPut = new Hashtable();
            State saPokupljenimKutijama = null;  
            stanjaNaObradi.Add(pocetnoStanje);
            while (stanjaNaObradi.Count > 0)
            {
                State naObradi = stanjaNaObradi[stanjaNaObradi.Count - 1];

                if (!predjeniPut.ContainsKey(naObradi.GetHashCode()))
                {
                    Main.allSearchStates.Add(naObradi);
                    if (naObradi.jePokupioKutije())
                    {
                        saPokupljenimKutijama = naObradi;
                        break;
                    }
                    
                    predjeniPut.Add(naObradi.GetHashCode(), null);
                    List<State> mogucaSledecaStanja = naObradi.mogucaSledecaStanja();
                    foreach (State sledeceStanje in mogucaSledecaStanja)
                    {
                        stanjaNaObradi.Add(sledeceStanje);
                    }
                }
                stanjaNaObradi.Remove(naObradi);
            }

            if(saPokupljenimKutijama!=null){

                AStarSearch astar = new AStarSearch();
                return astar.search(saPokupljenimKutijama);
                
            }

            return null;
        }
    }
}
