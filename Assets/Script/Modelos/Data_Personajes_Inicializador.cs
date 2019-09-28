using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    class Data_Personajes_Inicializador
    {
        public Vector3 posicion;
        public int id;

        public Data_Personajes_Inicializador(Vector3 posicion, int id)
        {
            this.posicion = posicion;
            this.id = id;
        }
    }
}
