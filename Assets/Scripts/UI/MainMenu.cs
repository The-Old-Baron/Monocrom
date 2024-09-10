using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject Initial;
    public GameObject DLC;
    public GameObject Configuracao;
    public GameObject Creditos;
    public GameObject Programacao;
    public GameObject Conquistas;

    private void Start()
    {
        ShowInitial();
    }
    public void ShowDLC()
    {
        HideAll();
        DLC.SetActive(true);
    }
    public void ShowConfiguracao()
    {
        HideAll();
        Configuracao.SetActive(true);
    }
    public void ShowCreditos()
    {
        HideAll();
        Creditos.SetActive(true);
    }
    public void ShowProgramacao()
    {
        HideAll();
        Programacao.SetActive(true);
    }
    public void ShowConquistas()
    {
        HideAll();
        Conquistas.SetActive(true);
    }
    public void ShowInitial()
    {
        HideAll();
        Initial.SetActive(true);
    }
    public void HideAll()
    {
        Initial.SetActive(false);
        DLC.SetActive(false);
        Configuracao.SetActive(false);
        Creditos.SetActive(false);
        Programacao.SetActive(false);
        Conquistas.SetActive(false);
    }
}
