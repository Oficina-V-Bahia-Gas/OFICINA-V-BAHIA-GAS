using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MapaDefinitivo");
    }

    public void LoadGame()
    {
        Debug.Log("Carregar Jogo: Fun��o ainda n�o implementada.");
    }

    public void QuitGame()
    {
        Debug.Log("Sair do Jogo");
        Application.Quit();
    }
}