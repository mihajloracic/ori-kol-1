using System;
using System.Collections.Generic;
using System.Text;

namespace Lavirint
{
    public class Kutija
    {
        

        public Kutija(Kutija temp)
        {            
            this.X = temp.X;
            this.Y = temp.Y;
            this.ID = temp.ID;
        }

        public Kutija()
        {
          
        }
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }


    }
}
