using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemigo_1 : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Animator anim;
    public Collider collider;
    public float velocidad;
    public float angulo;
    public int id;
    public int id_creador;

    private List<get_center> lista_usuarios;

    void Start()
    {
        lista_usuarios = new List<get_center>();

        rb.freezeRotation = true;
        angulo = 0f;

        anim.SetBool("ConArma", true);
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        if (lista_usuarios.Count!=0)
        {
            direccionar_angulo(limitar_distancia(), dt);
        }
    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime;

        if (lista_usuarios.Count != 0)
        {
            rb.AddForce(transform.forward * velocidad * rb.mass * dt);
        }
    }

    public void nuevo_usuario(get_center obj)
    {
        if(lista_usuarios.Count==0)
        {
            lista_usuarios.Add(obj);
        }
        else
        {
            foreach(var player in lista_usuarios)
            {
                if(player==obj)
                {
                    return;
                }
            }

            lista_usuarios.Add(obj);
        }
    }

    public void eliminar_usuario(get_center obj)
    {
        if (lista_usuarios.Count == 0)
        {
        }
        else
        {
            lista_usuarios.Remove(obj);
        }
    }

    public Vector3 limitar_distancia()
    {
        Vector3 direccion = new Vector3(0,0);

        var center = collider.bounds.center;

        double distancia_min = 9999f;



        foreach(var player in lista_usuarios)
        {
            var vector = player.get_center_position();

            var distance = Vector3.Distance(collider.bounds.center, vector);

            if(distance< distancia_min)
            {
                distancia_min = distance;

                direccion = vector;

            }

        }


        return direccion;
    }

    public void direccionar_angulo(Vector3 vector, float dt)
    {
        Vector3 direction = (vector - collider.bounds.center).normalized;
        Quaternion rotate = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, dt * 5f);
        rb.MoveRotation(transform.rotation);
    }

    public void OnCollisionStay(Collision collision)
    {
        if (!anim.GetBool("Ataque01") && collision.gameObject.layer == LayerMask.NameToLayer("Personaje"))
        {
            anim.SetTrigger("Ataque01");
        }
    }



}
