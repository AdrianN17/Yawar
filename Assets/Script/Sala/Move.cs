using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;

    public float x, y;
    public float velocidad;
    public float salto;
    public bool es_controlable;

    private int id;
    public enum movimiento_Horizontal { A, D, Ninguno }
    public enum movimiento_Vertical { W, S, Ninguno }
    public bool pisando_tierra = false;

    public movimiento_Horizontal mover_player_horizontal = movimiento_Horizontal.Ninguno;
    public movimiento_Vertical mover_player_vertical = movimiento_Vertical.Ninguno;


    public GameObject objeto_host;
    private Server_script script;

    void Start()
    {
        
        Debug.Log(id);
        rb.freezeRotation = true;

        script = objeto_host.GetComponent<Server_script>();
    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (es_controlable)
        {
            teclas_presionada(dt);
            tecla_soltada(dt);
        }
    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime;

        {
            Vector3 vec;

            if (mover_player_horizontal == movimiento_Horizontal.A)
            {
                vec = new Vector3(0, velocidad * dt * -1, 0);
                transform.Rotate(vec);
                rb.MoveRotation(transform.rotation);

                y = disminuir_mov(y,dt);

            }
            else if (mover_player_horizontal == movimiento_Horizontal.D)
            {
                vec = new Vector3(0, velocidad * dt * 1, 0);
                transform.Rotate(vec);
                rb.MoveRotation(transform.rotation);

                y = aumentar_mov(y, dt);
            }
        }

        if (mover_player_vertical == movimiento_Vertical.W)
        {
            rb.AddForce(transform.forward* velocidad * dt);

            x = aumentar_mov(x, dt);
        }
        else if (mover_player_vertical == movimiento_Vertical.S)
        {
            rb.AddForce(transform.forward * -velocidad * dt);

            x = disminuir_mov(x, dt);
        }

        if(mover_player_horizontal == movimiento_Horizontal.Ninguno && y!=0)
        {
            y= desacelerar_mov(y, dt);
        }

        if(mover_player_vertical == movimiento_Vertical.Ninguno && x != 0)
        {
            x = desacelerar_mov(x, dt);
        }

        anim.SetFloat("VelX", y);
        anim.SetFloat("VelY", x);


    }

    private void teclas_presionada(float dt)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mover_player_horizontal = movimiento_Horizontal.A;

            script.server.SendToAll("movimiento_horizontal",new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "A" }
            });


        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            mover_player_horizontal = movimiento_Horizontal.D;

            script.server.SendToAll("movimiento_horizontal", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "D" }
            });

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            mover_player_vertical = movimiento_Vertical.W;

            script.server.SendToAll("movimiento_vertical", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "W" }
            });

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            mover_player_vertical = movimiento_Vertical.S;

            script.server.SendToAll("movimiento_vertical", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "S" }
            });

        }

        if(Input.GetKeyDown(KeyCode.Space) && pisando_tierra)
        {
            rb.AddForce(Vector3.up*salto);

            script.server.SendToAll("movimiento_vertical", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "S" }
            });

            pisando_tierra = false;
        }


    }
    private void tecla_soltada(float dt)
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            mover_player_horizontal = movimiento_Horizontal.Ninguno;

            script.server.SendToAll("movimiento_horizontal", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "Ninguno" }
            });
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            mover_player_horizontal = movimiento_Horizontal.Ninguno;

            script.server.SendToAll("movimiento_horizontal", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "Ninguno" }
            });
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            mover_player_vertical = movimiento_Vertical.Ninguno;

            script.server.SendToAll("movimiento_vertical", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "Ninguno" }
            });
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            mover_player_vertical = movimiento_Vertical.Ninguno;

            script.server.SendToAll("movimiento_vertical", new Dictionary<string, dynamic>()
                { { "id" , id},{"tecla" , "Ninguno" }
            });
        }
    }
    public void movimiento_horizontal_cambio(string enu)
    {
        if (enu.Equals("Ninguno"))
        {
            mover_player_horizontal = movimiento_Horizontal.Ninguno;
        }
        else if (enu.Equals("A"))
        {
            mover_player_horizontal = movimiento_Horizontal.A;
        }
        else if (enu.Equals("D"))
        {
            mover_player_horizontal = movimiento_Horizontal.D;
        }
    }
    public void movimiento_vertical_cambio(string enu)
    {
        if (enu.Equals("Ninguno"))
        {
            mover_player_vertical = movimiento_Vertical.Ninguno;
        }
        else if (enu.Equals("W"))
        {
            mover_player_vertical = movimiento_Vertical.W;
        }
        else if (enu.Equals("S"))
        {
            mover_player_vertical = movimiento_Vertical.S;
        }
    }

    

    public void mover_bola_posicion(string vec)
    {
        //rb.MovePosition(StringToVector3(vec));
    }

    public int GetID()
    {
        return this.id;
    }

    public void SetID(int id)
    {
        this.id = id;
    }

    private float aumentar_mov(float value,float dt)
    {
        if(value<1)
        {
            value = value + dt*2;
        }

        return value;
    }

    private float disminuir_mov(float value, float dt)
    {
        if (value > -1)
        {
            value = value - dt*2;
        }

        return value;
    }

    private float desacelerar_mov(float value, float dt)
    {
        
        int signo = Math.Sign(value);
        float resultado = Math.Abs(value);

        resultado = resultado - dt * 2;

        if(resultado>0.01)
        {
            return resultado * signo;
        }
        else
        {
            return 0;
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Terreno"))
        {
            pisando_tierra = true;
        }
    }


}
