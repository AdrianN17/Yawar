using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class data_botar_objetos
    {
        public int id;
        public Vector3 posicion;
        public Dictionary<int,int> objetos;

        public data_botar_objetos(int id, Vector3 posicion, Dictionary<int,int> objetos)
        {
            this.id = id;
            this.posicion = posicion;
            this.objetos = objetos;
        }
    }
}
