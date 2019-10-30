using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class lista_coleccionables
    {
        private enum coleccionables {ninguno = 0 , oca_1=1,papa_1=2, papa_2=3,papa_3=4,papa_4=5};
        

        public lista_coleccionables()
        {

        }

        public int get_coleccionable()
        {
            int cantidad = Enum.GetNames(typeof(coleccionables)).Length;
            Random rnd = new Random();
            int tipo = rnd.Next(0, cantidad);

            return tipo;
        }

    }
}
