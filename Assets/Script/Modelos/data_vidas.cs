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
        public int id;
        public int vidas;

        public data_vidas(int id, int vidas)
        {
            this.id = id;
            this.vidas = vidas;
        }
    }
}
