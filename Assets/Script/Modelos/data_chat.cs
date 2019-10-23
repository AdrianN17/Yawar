using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    public class data_chat
    {
        int id;
        string texto;

        public data_chat(int id, string texto)
        {
            this.id = id;
            this.texto = texto;
        }
    }
}
