using Assets.Script.Modelos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control_dia_noche : MonoBehaviour
{
    private float timer;
    public float time_24_6;

    public enum periodos { dia,tarde,noche}
    private periodos periodo_actual;

    public Material dia;
    public Material tarde;
    public Material noche;

    // Start is called before the first frame update
    void Start()
    {
        periodo_actual = periodos.dia;
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        timer = timer + dt;

        if(timer> time_24_6)
        {
            cambiar_skybox();
            timer = 0;
        }

    }

    public void cambiar_skybox()
    {
        switch(periodo_actual)
        {
            case periodos.dia:
                {
                    periodo_actual = periodos.tarde;
                    nueva_skybox(tarde);

                    break;
                }
            case periodos.tarde:
                {
                    periodo_actual = periodos.noche;

                    nueva_skybox(noche);

                    break;
                }
            case periodos.noche:
                {
                    periodo_actual = periodos.dia;
                    nueva_skybox(dia);

                    break;
                }
        }
    }

    public void cambiar_skybox_directo()
    {
        switch (periodo_actual)
        {
            case periodos.dia:
                {
                    nueva_skybox(dia);

                    break;
                }
            case periodos.tarde:
                {
                    nueva_skybox(tarde);

                    break;
                }
            case periodos.noche:
                {
                    nueva_skybox(noche);

                    break;
                }
        }
    }

    public void nueva_skybox(Material material)
    {
        RenderSettings.skybox = material;
        DynamicGI.UpdateEnvironment();
    }

    public data_periodo get_periodo()
    {
        return new data_periodo(timer, (int)periodo_actual);
    }

    public void set_periodo(data_periodo data)
    {
        this.timer = data.tiempo;
        this.periodo_actual = (periodos)data.periodo;

        cambiar_skybox_directo();

    }
}
