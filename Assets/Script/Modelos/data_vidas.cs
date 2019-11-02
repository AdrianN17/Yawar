using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_vidas
    {
        public int id { get; set; }
        public int vidas { get; set; }

        public data_vidas(int id, int vidas)
        {
            this.id = id;
            this.vidas = vidas;
        }
    }
}
