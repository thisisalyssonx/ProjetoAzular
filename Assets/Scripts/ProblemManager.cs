using UnityEngine;

/// <summary>
/// Responsável por atualizar o visual da parede quando um novo problema é carregado.
/// Coloque este script em um GameObject vazio "ProblemManager" e configure
/// a referência para o Renderer da parede.
/// 
/// Inscreve-se no evento onProblemLoaded do GameManager.
/// </summary>
public class ProblemManager : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("Renderer do plano/mesh que representa a parede do problema")]
    public Renderer wallRenderer;

    [Tooltip("Nome da propriedade de textura no material da parede (geralmente _BaseMap ou _MainTex)")]
    public string texturePropertyName = "_BaseMap";

    [Tooltip("Nome da propriedade de cor no material (geralmente _BaseColor)")]
    public string colorPropertyName = "_BaseColor";

    [Header("Estado resolvido")]
    [Tooltip("Textura exibida na parede quando o problema é resolvido corretamente")]
    public Texture2D solvedTexture;

    [Tooltip("Cor de tint quando o problema é resolvido")]
    public Color solvedTint = new Color(0.6f, 1f, 0.6f); // verde suave

    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();

        if (wallRenderer == null)
            Debug.LogError("[ProblemManager] wallRenderer não atribuído!");
    }

    private void OnEnable()
    {
        // Inscreve nos eventos do GameManager assim que ele existir
        // (Start é seguro pois GameManager.Instance já está no Awake)
    }

    private void Start()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.onProblemLoaded.AddListener(OnProblemLoaded);
        GameManager.Instance.onAnswerChecked.AddListener(OnAnswerChecked);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.onProblemLoaded.RemoveListener(OnProblemLoaded);
        GameManager.Instance.onAnswerChecked.RemoveListener(OnAnswerChecked);
    }

    // ── Callbacks ─────────────────────────────────────────────────────────────

    private void OnProblemLoaded(WallProblem problem)
    {
        if (problem == null || wallRenderer == null) return;

        wallRenderer.GetPropertyBlock(_propertyBlock);

        if (problem.problemTexture != null)
            _propertyBlock.SetTexture(texturePropertyName, problem.problemTexture);

        _propertyBlock.SetColor(colorPropertyName, problem.wallTint);

        wallRenderer.SetPropertyBlock(_propertyBlock);
    }

    private void OnAnswerChecked(bool isCorrect)
    {
        if (!isCorrect || wallRenderer == null) return;

        // Mostra estado resolvido brevemente antes de trocar o problema
        wallRenderer.GetPropertyBlock(_propertyBlock);

        if (solvedTexture != null)
            _propertyBlock.SetTexture(texturePropertyName, solvedTexture);

        _propertyBlock.SetColor(colorPropertyName, solvedTint);

        wallRenderer.SetPropertyBlock(_propertyBlock);
    }
}