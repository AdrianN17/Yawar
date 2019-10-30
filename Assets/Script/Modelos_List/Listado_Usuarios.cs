using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class Listado_Usuarios
    {
        public int id { get; set; }
        public List<data_inicial> lista { get; set; }

        public Listado_Usuarios(int id, List<data_inicial> lista)
        {
            this.id = id;
            this.lista = lista;
        }
    }
}
