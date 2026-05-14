using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Exibe feedback visual (texto + ícone) na cena VR após cada tentativa.
/// Coloque em um World Space Canvas posicionado acima/ao lado da parede.
/// 
/// Estrutura sugerida do GameObject:
///   FeedbackCanvas (Canvas em World Space)
///     └── FeedbackUI (este script + CanvasGroup)
///         ├── IconText  (TMP_Text — exibe ✓ ou ✗)
///         └── MessageText (TMP_Text — exibe a mensagem)
/// </summary>
public class FeedbackUI : MonoBehaviour
{
    [Header("Referências UI")]
    public TMP_Text iconText;
    public TMP_Text messageText;

    [Header("Cores")]
    public Color correctColor = new Color(0.2f, 0.85f, 0.3f);
    public Color wrongColor = new Color(0.9f, 0.2f, 0.2f);

    [Header("Ícones")]
    public string correctIcon = "✓";
    public string wrongIcon = "✗";

    [Header("Tempo")]
    [Tooltip("Quanto tempo o feedback fica visível (segundos)")]
    public float displayDuration = 1.8f;

    [Header("Pontuação")]
    [Tooltip("Texto que exibe acertos / total em tempo real")]
    public TMP_Text scoreText;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();

        _canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.onAnswerChecked.AddListener(ShowFeedback);
        GameManager.Instance.onProblemLoaded.AddListener(_ => UpdateScore());
        GameManager.Instance.onGameOver.AddListener((s, e, a) => UpdateScore());
    }

    private void OnDestroy()
    {
        if (GameManager.Instance == null) return;

        GameManager.Instance.onAnswerChecked.RemoveListener(ShowFeedback);
    }

    // ── API pública ───────────────────────────────────────────────────────────

    public void ShowFeedback(bool isCorrect)
    {
        StopAllCoroutines();

        string icon = isCorrect ? correctIcon : wrongIcon;
        Color color = isCorrect ? correctColor : wrongColor;
        string message = isCorrect
            ? GameManager.Instance.CurrentProblem?.successMessage ?? "Correto!"
            : GameManager.Instance.CurrentProblem?.failMessage ?? "Tente outra ferramenta.";

        if (iconText != null) { iconText.text = icon; iconText.color = color; }
        if (messageText != null) { messageText.text = message; messageText.color = color; }

        UpdateScore();
        StartCoroutine(ShowAndHide());
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private void UpdateScore()
    {
        if (scoreText == null || GameManager.Instance == null) return;
        scoreText.text = $"{GameManager.Instance.Score} / {GameManager.Instance.TotalProblems}";
    }

    private IEnumerator ShowAndHide()
    {
        // Fade in
        float t = 0f;
        while (t < 0.15f)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Clamp01(t / 0.15f);
            yield return null;
        }
        _canvasGroup.alpha = 1f;

        yield return new WaitForSeconds(displayDuration);

        // Fade out
        t = 0f;
        while (t < 0.3f)
        {
            t += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Clamp01(1f - t / 0.3f);
            yield return null;
        }
        _canvasGroup.alpha = 0f;
    }
}