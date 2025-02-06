using UnityEngine;
using UnityEditor;

public class GameTestEditor : EditorWindow
{
    GameManager gameManager;

    [MenuItem("Tools/Game Test")]
    public static void ShowWindow()
    {
        GetWindow<GameTestEditor>("Testes do Jogo");
    }

    void OnGUI()
    {
        GUILayout.Label("Testes de Pontuação e Tempo", EditorStyles.boldLabel);

        if (GUILayout.Button("Definir Pontuação: 100 (1 Estrela)"))
        {
            SetScore(100);
        }

        if (GUILayout.Button("Definir Pontuação: 200 (2 Estrelas)"))
        {
            SetScore(200);
        }

        if (GUILayout.Button("Definir Pontuação: 300 (3 Estrelas)"))
        {
            SetScore(300);
        }

        GUILayout.Space(10);
        GUILayout.Label("Tempo Restante", EditorStyles.boldLabel);

        if (GUILayout.Button("Definir Tempo para 2 segundos"))
        {
            SetTimeRemaining(2);
        }
    }

    void SetScore(float score)
    {
        FindGameManager();
        if (gameManager != null)
        {
            gameManager.ForceSetScore(score);
            Debug.Log($"Pontuação definida para: {score}");
        }
        else
        {
            Debug.LogError("GameManager não encontrado!");
        }
    }

    void SetTimeRemaining(float time)
    {
        FindGameManager();
        if (gameManager != null)
        {
            gameManager.ForceSetTime(time);
            Debug.Log($"Tempo restante definido para: {time} segundos");
        }
        else
        {
            Debug.LogError("GameManager não encontrado!");
        }
    }

    void FindGameManager()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }
}