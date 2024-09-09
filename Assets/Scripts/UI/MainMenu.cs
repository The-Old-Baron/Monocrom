using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject PanelInicial;
    public GameObject Jogar;
    public GameObject Configuracao;
    public GameObject Extras;

    private void Start()
    {
        ShowPanel(PanelInicial);
    }
    
    private void ShowPanel(GameObject panel)
    {
        HideAll();
        panel.SetActive(true);
    }

    private void HideAll()
    {
        PanelInicial.SetActive(false);
        Jogar.SetActive(false);
        Configuracao.SetActive(false);
        Extras.SetActive(false);
    }

    public void ShowJogar()
    {
        ShowPanel(Jogar);
    }

    public void ShowConfiguracao()
    {
        ShowPanel(Configuracao);
    }

    public void ShowExtras()
    {
        ShowPanel(Extras);
    }

    public void ShowPanelInicial()
    {
        ShowPanel(PanelInicial);
    }
}
