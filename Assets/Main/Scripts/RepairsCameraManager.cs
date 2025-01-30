using UnityEngine;

public class RepairsCameraManager : MonoBehaviour
{
    private Transform target;
    public float followSpeed = 5f;
    public float rotationSpeed = 5f;

    /// <summary>
    /// Define o Transform do novo alvo e ativa a câmera.
    /// </summary>
    public void SetTargetTransform(Transform newTarget)
    {
        Debug.LogWarning($"SetTargetTransform chamado com o alvo: {(newTarget != null ? newTarget.name : "null")}");

        target = newTarget;

        if (target != null)
        {
            ActivateCamera();
            Debug.Log($"Câmera ativada e configurada para o alvo: {target.name}");
        }
        else
        {
            Debug.LogWarning("Nenhum alvo válido foi definido para a câmera.");
            ClearTarget();
        }
    }

    /// <summary>
    /// Ativa a câmera e posiciona no alvo.
    /// </summary>
    public void ActivateCamera()
    {
        Debug.LogWarning($"Câmera ativada em posição: {transform.position}, rotação: {transform.rotation}");

        if (target != null)
        {
            gameObject.SetActive(true);
            transform.position = target.position;
            transform.rotation = target.rotation;

            Debug.Log($"Câmera posicionada em {target.position} e alinhada com {target.rotation}");
        }
        else
        {
            Debug.LogWarning("Tentativa de ativar a câmera sem um alvo definido.");
        }
    }

    /// <summary>
    /// Limpa o alvo e desativa a câmera.
    /// </summary>
    public void ClearTarget()
    {
        target = null;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Atualiza a posição e rotação da câmera para seguir o alvo.
    /// </summary>
    private void Update()
    {
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotationSpeed * Time.deltaTime);
        }
        else if (gameObject.activeSelf)
        {
            ClearTarget();
        }
    }
}
