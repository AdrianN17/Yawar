using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    [Serializable]
    public class data_botar_objetos
    {
        public int id { get; set; }
        public data_vector posicion { get; set; }
        public Dictionary<int,int> objetos { get; set; }

        public data_botar_objetos(int id, data_vector posicion, Dictionary<int,int> objetos, int x)
        {
            this.id = id;
            this.posicion = posicion;
            this.objetos = objetos;
        }
    }
}
