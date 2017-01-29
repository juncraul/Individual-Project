using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Individual_Project
{
    public class chromo_typ
    {
        //the binary bit string is held in a std::string
        public string bits;

        public float fitness;

        public chromo_typ()
        {
            bits = "";
            fitness = 0;
        }

        public chromo_typ(string bts, float ftns)
        {
            bits = bts;
            fitness = ftns;
        }
    };
}
