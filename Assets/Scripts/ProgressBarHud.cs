using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Barra de progresso visual composta por marcadores individuais.
/// Um marcador por problema — fica verde ao acertar.
///
/// Coloque este script no Panel_ProgressBar dentro de Canvas_ProblemaCentral.
/// Crie um prefab simples de marcador: Image 36x6px, cor branca, sprite redondo.
/// </summary>
public class ProgressBarHUD : MonoBehaviour
{
    [Header("Prefab e Container")]
    [Tooltip("Prefab de uma barrinha/bolinha de progresso (Image simples)")]
    public GameObject marcadorPrefab;

    [Tooltip("Transform pai onde os marcadores são instanciados (Horizontal Layout Group)")]
    public Transform container;

    [Header("Cores")]
    public Color corAtiva = new Color(0.52f, 0.94f, 0.67f); // verde claro #86EFAC
    public Color corInativa = new Color(1f, 1f, 1f, 0.2f);    // branco transparente

    private Image[] _marcadores;

    /// <summary>
    /// Inicializa a barra criando 'total' marcadores. Chame no Start do GameManager.
    /// </summary>
    public void Inicializar(int total)
    {
        // Limpa marcadores antigos (caso reinicie)
        foreach (Transform filho in container)
            Destroy(filho.gameObject);

        _marcadores = new Image[total];

        for (int i = 0; i < total; i++)
        {
            var obj = Instantiate(marcadorPrefab, container);
            _marcadores[i] = obj.GetComponent<Image>();

            if (_marcadores[i] == null)
            {
                Debug.LogError("[ProgressBarHUD] Prefab do marcador não tem componente Image!");
                return;
            }

            _marcadores[i].color = corInativa;
        }
    }

    /// <summary>
    /// Atualiza cores: marcadores até 'acertos' ficam verdes, o restante fica inativo.
    /// </summary>
    public void Atualizar(int acertos)
    {
        if (_marcadores == null) return;

        for (int i = 0; i < _marcadores.Length; i++)
            _marcadores[i].color = i < acertos ? corAtiva : corInativa;
    }
}