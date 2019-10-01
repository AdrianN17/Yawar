using Assets.Script.Modelos;
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
    public float giro;
    public bool es_controlable;

    private int id;
    public enum movimiento_Horizontal { A, D, Ninguno }
    public enum movimiento_Vertical { W, S, Ninguno }
    public bool pisando_tierra = false;

    public movimiento_Horizontal mover_player_horizontal = movimiento_Horizontal.Ninguno;
    public movimiento_Vertical mover_player_vertical = movimiento_Vertical.Ninguno;

    public GameObject client_manager;
    public Client_script client;

    void Start()
    {

        if (es_controlable)
        { 
            client = client_manager.GetComponent<Client_script>();
        }

        Debug.Log(id);
        rb.freezeRotation = true;
        
    }

    void Update()
    {
        float dt = Math.Min( 1/60,Time.deltaTime);


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
                vec = new Vector3(0,  dt * -1* giro, 0);
                transform.Rotate(vec);
                rb.MoveRotation(transform.rotation);

                x = disminuir_mov(x,dt);

            }
            else if (mover_player_horizontal == movimiento_Horizontal.D)
            {
                vec = new Vector3(0,  dt * 1* giro, 0);
                transform.Rotate(vec);
                rb.MoveRotation(transform.rotation);

                x = aumentar_mov(x, dt);
            }
        }

        if (mover_player_vertical == movimiento_Vertical.W)
        {
            rb.AddForce(transform.forward* velocidad * rb.mass * dt);

            y = aumentar_mov(y, dt);
        }
        else if (mover_player_vertical == movimiento_Vertical.S)
        {
            rb.AddForce(transform.forward * -velocidad * rb.mass * dt);

            y = disminuir_mov(y, dt);
        }

        if(mover_player_horizontal == movimiento_Horizontal.Ninguno && x != 0)
        {
            x= desacelerar_mov(x, dt);
        }

        if(mover_player_vertical == movimiento_Vertical.Ninguno && y != 0)
        {
            y = desacelerar_mov(y, dt);
        }

        if (!pisando_tierra)
        {
            //Debug.Log(Remap(rb.velocity.y,6,-4,-1,1));

            var mov_salto = Remap(rb.velocity.y, 6, -4, -1, 1);
            anim.SetBool("Pisando_tierra", pisando_tierra);
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

            client.client.Send("movimiento", new data_tecla(GetID(), "A", "horizontal"));


        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            mover_player_horizontal = movimiento_Horizontal.D;

            client.client.Send("movimiento", new data_tecla(GetID(), "D", "horizontal"));

        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            mover_player_vertical = movimiento_Vertical.W;

            client.client.Send("movimiento", new data_tecla(GetID(), "W", "vertical"));

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            mover_player_vertical = movimiento_Vertical.S;

            client.client.Send("movimiento", new data_tecla(GetID(), "S", "vertical"));

        }

        if (Input.GetKeyDown(KeyCode.Space) && pisando_tierra)
        {
            rb.AddForce(Vector3.up * salto * rb.mass);

            client.client.Send("movimiento", new data_tecla(GetID(), "SPACE", "Salto"));

            pisando_tierra = false;
        }
    }
    private void tecla_soltada(float dt)
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            mover_player_horizontal = movimiento_Horizontal.Ninguno;

            client.client.Send("movimiento", new data_tecla(GetID(), "Ninguno", "horizontal"));
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            mover_player_horizontal = movimiento_Horizontal.Ninguno;

            client.client.Send("movimiento", new data_tecla(GetID(), "Ninguno", "horizontal"));
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            mover_player_vertical = movimiento_Vertical.Ninguno;

            client.client.Send("movimiento", new data_tecla(GetID(), "Ninguno", "vertical"));
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            mover_player_vertical = movimiento_Vertical.Ninguno;

            client.client.Send("movimiento", new data_tecla(GetID(), "Ninguno", "vertical"));
        }
    }

    public void movimiento_cambio(string enu, string orientacion)
    {
        switch (orientacion)
        {
            case "horizontal":

                switch (enu)
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

    private float aumentar_mov(float value, float dt)
    {
        if (value < 1)
        {
            value = value + dt * 2;
        }

        return value;
    }

    private float disminuir_mov(float value, float dt)
    {
        if (value > -1)
        {
            value = value - dt * 2;
        }

        return value;
    }

    private float desacelerar_mov(float value, float dt)
    {

        int signo = Math.Sign(value);
        float resultado = Math.Abs(value);

        resultado = resultado - dt * 2;

        if (resultado > 0.01)
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
        if (col.gameObject.layer == LayerMask.NameToLayer("Terreno"))
        {
            pisando_tierra = true;
        }
    }

    public float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void normalizado(Vector3 posicion)
    {
        transform.position = posicion;
        
    }


}
