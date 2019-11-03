using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ControlConfiguracion : MonoBehaviour
{
    public Toggle pantallaCompleta;
    public Dropdown resolucionDropdown;
    public Resolution[] resoluciones;
    private Configuraciones ConfiguracionesJuego;
    public Button botonAplicar;

    private void OnEnable()
    {
        ConfiguracionesJuego = new Configuraciones();
        pantallaCompleta.onValueChanged.AddListener(delegate{ On_PantallaCompleta(); });
        resolucionDropdown.onValueChanged.AddListener(delegate { On_resoluciones(); });

        botonAplicar.onClick.AddListener(delegate { On_Aplicar(); });

        resoluciones = Screen.resolutions;
        foreach (Resolution resolution in resoluciones)
        {
            resolucionDropdown.options.Add( new Dropdown.OptionData(resolution.ToString()));
        }
    }
    public void On_PantallaCompleta()
    {
        ConfiguracionesJuego.pantallacompleta = Screen.fullScreen = pantallaCompleta.isOn;
    }
    public void On_resoluciones()
    {
        Screen.SetResolution(resoluciones[resolucionDropdown.value].width, resoluciones[resolucionDropdown.value].height, Screen.fullScreen);
    }

    public void On_Aplicar()
    {
        GuardarCambios();
    }

    public void GuardarCambios()
    {
        string jsonData = JsonUtility.ToJson(ConfiguracionesJuego, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsonData);
    }
}
