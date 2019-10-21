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
        public Vector3 pos;
        public int id;
        
        public data_enemigo_inicial(Vector3 pos, int id)
        {
            this.pos = pos;
            this.id = id;
        }

    }
}
