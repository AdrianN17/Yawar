using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemigo_1 : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    public Animator anim;
    public Collider collider;
    public float velocidad;
    public float angulo;
    public int id;
    public GameObject padre;
    public int punto_id;

    private List<get_center> lista_usuarios;
    public bool atacando;

    public acciones_compartidas script_compartido;
    public float max_counter_ahogo;
    private float counter_ahogo;
    public float nivel_agua_y;
    public int coleccionable;
    public bool es_servidor;

    public bool actualizado_envio;


    void Start()
    {
        lista_usuarios = new List<get_center>();

        rb.freezeRotation = true;
        angulo = 0f;

        atacando = false;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        if (lista_usuarios.Count != 0)
        {

            direccionar_angulo(limitar_distancia(), dt);
            atacando = true;
        }

        counter_ahogo = counter_ahogo + dt;

        if(counter_ahogo> max_counter_ahogo)
        {
            if (collider.bounds.center.y < nivel_agua_y)
            { 
                script_compartido.disminuir_vida_ahogamiento();     
            }

            counter_ahogo = 0;
        }


        atacando = false;

    }

    void FixedUpdate()
    {
        float dt = Time.deltaTime;

        if (lista_usuarios.Count != 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("Correr"))
        {
            rb.AddForce(transform.forward * velocidad * rb.mass * dt);
        }
    }

    public void nuevo_usuario(get_center obj)
    {
        if (lista_usuarios.Count == 0)
        {
            lista_usuarios.Add(obj);

            anim.SetBool("Perseguir", true);

        }
        else
        {
            foreach (var player in lista_usuarios)
            {
                if (player == obj)
                {
                    return;
                }
            }

            lista_usuarios.Add(obj);
        }


    }

    public void eliminar_usuario(get_center obj)
    {
        if (lista_usuarios.Count != 0)
        {
            lista_usuarios.Remove(obj);

            if (lista_usuarios.Count == 0)
            {
                anim.SetBool("Perseguir", false);
            }

        }
    }

    public Vector3 limitar_distancia()
    {
        Vector3 direccion = new Vector3(0, 0);

        var center = collider.bounds.center;

        double distancia_min = 9999f;



        foreach (var player in lista_usuarios)
        {
            var vector = player.get_center_position();

            var distance = Vector3.Distance(collider.bounds.center, vector);

            if (distance < distancia_min)
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
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Ataque") && collision.gameObject.layer == LayerMask.NameToLayer("Personaje"))
        {
            anim.SetTrigger("Atacar");
        }
    }

    public void OnDestroy()
    {
        if(es_servidor)
        { 
            padre.GetComponent<creacion>().saber_muertes(this.id,this.gameObject,this.punto_id);
        }
        else
        {
            padre.GetComponent<crear_enemigo_cliente>().contar_muertes(this.gameObject);
        }

        var go = GameObject.Find("Objetos_Botados");

        go.GetComponent<coleccionable>().crear_nuevo_coleccionable(this.coleccionable, this.collider.bounds.center);

    }
}
