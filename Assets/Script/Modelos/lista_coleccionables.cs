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
        private enum coleccionables {ninguno = -1 , choclo=0,oca_1=1,papa_1=2, papa_2=3,papa_3=4,papa_4=5};
        

        public lista_coleccionables()
        {

        }

        public int get_coleccionable()
        {
            int cantidad = Enum.GetNames(typeof(coleccionables)).Length;
            Random rnd = new Random();
            int tipo = rnd.Next(-1, cantidad-1);

            return tipo;
        }

        public List<string> get_colecionables_string()
        {
            return new List<string>(){ "Choclo", "Oca", "Papa", "Papa 2", "Papa 3", "Papa 4 " };
        }

    }
}
