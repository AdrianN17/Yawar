using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_chat
    {
        public int id { get; set; }
        public string texto { get; set; }

        public data_chat(int id, string texto)
        {
            this.id = id;
            this.texto = texto;
        }
    }
}
