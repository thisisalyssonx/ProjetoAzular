using TMPro;
using UnityEngine;


/// <summary>
/// Diário de Campo — exibe informações sobre o problema atual e a ferramenta correta.
/// 
/// O diário é um World Space Canvas que abre/fecha ao interagir com o objeto livro na cena.
/// 
/// Estrutura sugerida:
///   DiaryBook (XRSimpleInteractable — o objeto físico do livro)
///     └── DiaryCanvas (Canvas World Space, inativo por padrão)
///         ├── ProblemNameText  (TMP_Text)
///         ├── DescriptionText  (TMP_Text)
///         └── ToolHintText     (TMP_Text)
/// 
/// Coloque este script no DiaryBook.
/// </summary>
public class FieldDiary : MonoBehaviour
{
    [Header("Referências")]
    [Tooltip("O Canvas do diário (começa inativo)")]
    public GameObject diaryCanvas;

    [Tooltip("Texto do nome do problema")]
    public TMP_Text problemNameText;

    [Tooltip("Texto da descrição do problema")]
    public TMP_Text descriptionText;

    [Tooltip("Dica sobre a ferramenta correta")]
    public TMP_Text toolHintText;

    [Header("Comportamento")]
    [Tooltip("Se verdadeiro, abre automaticamente quando um novo problema é carregado")]
    public bool autoOpenOnProblem = false;

    private bool _isOpen = false;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable _interactable;

    private void Awake()
    {
        _interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>();

        if (diaryCanvas != null)
            diaryCanvas.SetActive(false);
    }

    private void Start()
    {
        if (_interactable != null)
            _interactable.selectEntered.AddListener(_ => Toggle());

        if (GameManager.Instance != null)
            GameManager.Instance.onProblemLoaded.AddListener(RefreshContent);
    }

    private void OnDestroy()
    {
        if (_interactable != null)
            _interactable.selectEntered.RemoveAllListeners();

        if (GameManager.Instance != null)
            GameManager.Instance.onProblemLoaded.RemoveListener(RefreshContent);
    }

    // ── API pública ───────────────────────────────────────────────────────────

    public void Toggle()
    {
        if (_isOpen) Close();
        else Open();
    }

    public void Open()
    {
        _isOpen = true;
        RefreshContent(GameManager.Instance?.CurrentProblem);

        if (diaryCanvas != null)
            diaryCanvas.SetActive(true);
    }

    public void Close()
    {
        _isOpen = false;

        if (diaryCanvas != null)
            diaryCanvas.SetActive(false);
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private void RefreshContent(WallProblem problem)
    {
        if (problem == null) return;

        if (problemNameText != null)
            problemNameText.text = problem.problemName;

        if (descriptionText != null)
            descriptionText.text = problem.description;

        if (toolHintText != null)
            toolHintText.text = $"Ferramenta recomendada:\n{problem.correctToolDescription}";

        if (autoOpenOnProblem && !_isOpen)
            Open();
    }
}