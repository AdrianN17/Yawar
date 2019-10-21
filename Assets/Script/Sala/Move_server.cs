using Assets.Script.Modelos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_server : MonoBehaviour
{
    public Rigidbody rb;
    public Animator anim;
    public Collider collider;

    public float x, y;
    public float velocidad;
    public float salto;
    public float giro;

    private int id;
    public enum movimiento_Horizontal { A, D, Ninguno }
    public enum movimiento_Vertical { W, S, Ninguno }
    public bool pisando_tierra = false;

    public movimiento_Horizontal mover_player_horizontal = movimiento_Horizontal.Ninguno;
    public movimiento_Vertical mover_player_vertical = movimiento_Vertical.Ninguno;

    public GameObject server_manager;
    public Server_script server;

    private const float distancia_raycast= 0.8f;

    private enum tipo_arma { ninguna, mazo, lanza}
    private tipo_arma arma_actual = tipo_arma.ninguna;

   
    void Start()
    {
        Debug.Log(id);
        rb.freezeRotation = true;
        
    }

    void OnTriggerEnter(Collider arma)
    {
        if (arma.gameObject.layer == LayerMask.NameToLayer("Arma"))
        {
            anim.SetBool("ConArma", true);

            GameObject go = transform.GetChild(1).gameObject;
            go.SetActive(true);
            arma_actual = tipo_arma.mazo;

        }
    }

    void Update()
    {
        float dt = Math.Min( 1/60,Time.deltaTime);

        var center = collider.bounds.center;
        var extends = collider.bounds.extents;

        var x = center.x;
        var y = (center.y - extends.y) + distancia_raycast;
        var z = center.z;

        Ray ray = new Ray(new Vector3(x,y,z), Vector3.down);

        //int layerMask =  LayerMask.NameToLayer("Terreno");
        

        Debug.DrawRay(ray.origin,ray.direction, Color.red);

        RaycastHit hit;
       
        if (Physics.Raycast(ray.origin,ray.direction, out hit,1))
        {
            pisando_tierra = true;
            //Debug.LogWarning(hit.collider.gameObject.name);
        }
        else
        {
            pisando_tierra = false;
            //Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
            //Debug.LogWarning("Did not Hit");
        }


        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Ataque01"))
        { 
            teclas_presionada(dt);
            tecla_soltada(dt);

            atacar();
        }

        
    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime;

        {
            Vector3 vec;

            if (mover_player_horizontal == movimiento_Horizontal.A)
            {
                vec = new Vector3(0, dt * -1 * giro, 0);
                transform.Rotate(vec);
                rb.MoveRotation(transform.rotation);

                x = disminuir_mov(x, dt);

            }
            else if (mover_player_horizontal == movimiento_Horizontal.D)
            {
                vec = new Vector3(0, dt * 1 * giro, 0);
                transform.Rotate(vec);
                rb.MoveRotation(transform.rotation);

                x = aumentar_mov(x, dt);
            }
        }

        if (mover_player_vertical == movimiento_Vertical.W)
        {
            rb.AddForce(transform.forward * velocidad * rb.mass * dt);

            y = aumentar_mov(y, dt);
        }
        else if (mover_player_vertical == movimiento_Vertical.S)
        {
            rb.AddForce(transform.forward * -velocidad * rb.mass * dt);

            y = disminuir_mov(y, dt);
        }

        if (mover_player_horizontal == movimiento_Horizontal.Ninguno && x != 0)
        {
            x = desacelerar_mov(x, dt);
        }

        if (mover_player_vertical == movimiento_Vertical.Ninguno && y != 0)
        {
            y = desacelerar_mov(y, dt);
        }
        

        anim.SetBool("Pisando_tierra", pisando_tierra);
        anim.SetFloat("VelX", x);
        anim.SetFloat("VelY", y);

    }

    private void teclas_presionada(float dt)
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            mover_player_horizontal = movimiento_Horizontal.A;

            server.server.SendToAll("movimiento",new data_tecla(GetID(),"A","horizontal"));


        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            mover_player_horizontal = movimiento_Horizontal.D;

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "D", "horizontal"));

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            mover_player_vertical = movimiento_Vertical.W;

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "W", "vertical"));

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            mover_player_vertical = movimiento_Vertical.S;

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "S", "vertical"));

        }

        if(Input.GetKeyDown(KeyCode.Space) && pisando_tierra)
        {
            rb.AddForce(Vector3.up*salto* rb.mass );

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "SPACE", "Salto"));
            pisando_tierra = false;

            anim.SetTrigger("Saltar");
            
            
        }
    }
    private void tecla_soltada(float dt)
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            mover_player_horizontal = movimiento_Horizontal.Ninguno;

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "Ninguno", "horizontal"));
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            mover_player_horizontal = movimiento_Horizontal.Ninguno;

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "Ninguno", "horizontal"));
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            mover_player_vertical = movimiento_Vertical.Ninguno;

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "Ninguno", "vertical"));
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            mover_player_vertical = movimiento_Vertical.Ninguno;

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "Ninguno", "vertical"));
        }
    }

    public void movimiento_cambio(string enu,string orientacion)
    {
        switch(orientacion)
        {
            case "horizontal":

                switch(enu)
                {
                    case "A":
                        mover_player_horizontal = movimiento_Horizontal.A;
                        break;
                    case "D":
                        mover_player_horizontal = movimiento_Horizontal.D;
                        break;
                    case "Ninguno":
                        mover_player_horizontal = movimiento_Horizontal.Ninguno;
                        break;
                }

                break;
            case "vertical":

                switch (enu)
                {
                    case "W":
                        mover_player_vertical = movimiento_Vertical.W;
                        break;
                    case "S":
                        mover_player_vertical = movimiento_Vertical.S;
                        break;
                    case "Ninguno":
                        mover_player_vertical = movimiento_Vertical.Ninguno;
                        break;
                }

                break;

            case "salto":

                rb.AddForce(Vector3.up * salto * rb.mass);
                pisando_tierra = false;

                break;
            case "atacar":
                atacar();
                break;
        }
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

    public float Remap( float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void normalizado(Vector3 posicion, Quaternion radio)
    {
        transform.position = posicion;
        transform.rotation = radio;
        rb.MoveRotation(transform.rotation);
    }

    public void atacar()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (arma_actual == tipo_arma.mazo && pisando_tierra)
            {
                anim.SetTrigger("Ataque01");

                mover_player_horizontal = movimiento_Horizontal.Ninguno;
                mover_player_vertical = movimiento_Vertical.Ninguno;

                server.server.SendToAll("movimiento", new data_tecla(GetID(), "X", "atacar"));
            }
        }
    }

    


}
