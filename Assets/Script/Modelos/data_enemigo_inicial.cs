using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class data_enemigo_inicial
    {
        public int id { get; set; }
        public int id_posicion { get; set; }
        public int coleccionable { get; set; }

        public data_enemigo_inicial(int id_posicion, int id,int coleccionable)
        {
            this.id_posicion = id_posicion;
            this.id = id;
            this.coleccionable = coleccionable;
        }

    }
}
