using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_periodo
    {
        public float tiempo { get; set; }
        public int periodo { get; set; }

        public data_periodo(float tiempo, int periodo)
        {
            this.tiempo = tiempo;
            this.periodo = periodo;
        }
    }

}
