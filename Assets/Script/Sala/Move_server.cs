using Assets.Script.Modelos;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public bool escribiendo;

    public TextMesh texto;

    public acciones_compartidas acciones;

    public List<GameObject> lista_gameobject_armas;
    public Sonidos_Pj sound;

    void Start()
    {
        //lanza
        lista_gameobject_armas.Add(transform.GetChild(1).gameObject);
        //mazo
        lista_gameobject_armas.Add(transform.GetChild(2).gameObject);


        acciones.index_arma = 0;

        //Debug.Log(id);
        rb.freezeRotation = true;

        escribiendo = false;

        sound = GameObject.FindGameObjectWithTag("Sonido_main").GetComponent<Sonidos_Pj>();


    }

    void OnTriggerEnter(Collider arma)
    {
        if (arma.gameObject.layer == LayerMask.NameToLayer("Arma") && arma_actual == tipo_arma.ninguna)
        {

            switch(arma.gameObject.tag)
            {
                case "Mazo":
                    {
                        anim.SetBool("ConArma", true);

                        set_active_arma(1);

                        arma_actual = tipo_arma.mazo;
                        acciones.index_arma = 0;

                        break;
                    }
                case "Lanza":
                    {
                        anim.SetBool("ConArma", true);

                        set_active_arma(0);

                        arma_actual = tipo_arma.lanza;
                        acciones.index_arma = 1;

                        break;
                    }

            }

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

        if(!escribiendo)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Ataque01") && !anim.GetCurrentAnimatorStateInfo(0).IsName("AtaqueLanza") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Muerte_p") && !anim.GetCurrentAnimatorStateInfo(0).IsName("ahogar")
                 && !anim.GetCurrentAnimatorStateInfo(0).IsName("Pj_Dano 0") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Pj_Dano"))
            {
                teclas_presionada(dt);
                tecla_soltada(dt);

                if ((mover_player_horizontal != movimiento_Horizontal.Ninguno || mover_player_vertical != movimiento_Vertical.Ninguno) && pisando_tierra && collider.bounds.center.y>sound.nivel_agua)
                {
                    sound.moverse = true;
                }
                else
                {
                    sound.moverse = false;
                }

                atacar();

                no_arma();
            }
            else
            {
                mover_player_horizontal = movimiento_Horizontal.Ninguno;
                mover_player_vertical = movimiento_Vertical.Ninguno;

                sound.moverse = false;
            }
        }

        if (collider.bounds.center.y < sound.nivel_agua)
        {
            sound.agua = true;
        }
        else
        {
            sound.agua = false;
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

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "SPACE", "salto"));
            pisando_tierra = false;


            sound.emitir_salto();
            

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
                if (arma_actual == tipo_arma.mazo && pisando_tierra)
                {
                    acciones.index_arma = 0;
                    empezar_animacion_ataque(0);

                    mover_player_horizontal = movimiento_Horizontal.Ninguno;
                    mover_player_vertical = movimiento_Vertical.Ninguno;
                }
                else if (arma_actual == tipo_arma.lanza && pisando_tierra)
                {
                    acciones.index_arma = 1;
                    empezar_animacion_ataque(1);

                    mover_player_horizontal = movimiento_Horizontal.Ninguno;
                    mover_player_vertical = movimiento_Vertical.Ninguno;

                }
                break;

            case "no_arma":
                no_arma_funcion();
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
                empezar_animacion_ataque(0);

                mover_player_horizontal = movimiento_Horizontal.Ninguno;
                mover_player_vertical = movimiento_Vertical.Ninguno;

                server.server.SendToAll("movimiento", new data_tecla(GetID(), "X", "atacar"));

            }else if (arma_actual == tipo_arma.lanza && pisando_tierra)
            {
                empezar_animacion_ataque(1);

                mover_player_horizontal = movimiento_Horizontal.Ninguno;
                mover_player_vertical = movimiento_Vertical.Ninguno;

                server.server.SendToAll("movimiento", new data_tecla(GetID(), "2", "atacar"));
            }
        }
    }

    public void no_arma_funcion()
    {
        no_active_arma();

        arma_actual = tipo_arma.ninguna;

        anim.SetBool("ConArma", false);
        
    }

    public void no_arma()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            no_active_arma();

            server.server.SendToAll("movimiento", new data_tecla(GetID(), "G", "no_arma"));

            arma_actual = tipo_arma.ninguna;

            anim.SetBool("ConArma", false);
        }
    }

    public int get_arma_actual()
    {
        return (int)arma_actual;
    }

    public void set_arma_actual(int arma)
    {
        this.arma_actual = (tipo_arma)arma;

        if(arma_actual!=tipo_arma.ninguna)
        {
            anim.SetBool("ConArma", true);

            switch(arma_actual)
            {
                case tipo_arma.mazo:
                {
                        set_active_arma(1);
                        break;
                }
                case tipo_arma.lanza:
                {
                        set_active_arma(0);
                        break;
                }
            }
        }

    }

    public void set_active_arma(int i)
    {
        no_active_arma();

        lista_gameobject_armas[i].SetActive(true);
    }

    public void no_active_arma()
    {
        foreach (var obj in lista_gameobject_armas)
        {
            obj.SetActive(false);
        }
    }

    public void empezar_animacion_ataque(int i)
    {
        switch (i)
        {
            case 0:
                {
                    anim.SetBool("Ataque01", true);
                    Invoke("Terminar_anim",1f);

                    break;
                }
            case 1:
                {
                    anim.SetBool("Ataque02", true);
                    Invoke("Terminar_anim",1.5f);
                    break;
                }
        }

    }

    public void Terminar_anim()
    {
        anim.SetBool("Ataque01", false);
        anim.SetBool("Ataque02", false);
    }

}
