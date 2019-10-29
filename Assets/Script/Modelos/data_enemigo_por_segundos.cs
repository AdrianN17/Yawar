using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Modelos
{
    public class data_enemigo_por_segundos
    {
        public int id;
        public Vector3 pos;
        public int vida;

        public Vector3 radio;

        public data_enemigo_por_segundos(int id, Vector3 pos, int vida, Vector3 radio)
        {
            this.id = id;
            this.pos = pos;
            this.vida = vida;
            this.radio = radio;
        }
    }
}
