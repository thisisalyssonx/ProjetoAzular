using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Tela de resultado final — exibida quando todos os problemas são resolvidos.
/// 
/// Estrutura sugerida (World Space Canvas, inativo por padrão):
///   ResultCanvas
///     ├── TitleText       — "Resultado Final"
///     ├── ScoreText       — "Você acertou X de Y problemas"
///     ├── AttemptsText    — "em Z tentativas"
///     ├── ChefText        — mensagem personalizada do Chef
///     └── RestartButton   — XRSimpleInteractable ou Button
/// 
/// Coloque este script no ResultCanvas.
/// </summary>
public class ResultScreen : MonoBehaviour
{
    [Header("Referências UI")]
    public TMP_Text scoreText;
    public TMP_Text attemptsText;
    public TMP_Text chefText;

    [Header("Mensagens do Chef")]
    [Tooltip("Mensagem se o jogador acertar tudo")]
    [TextArea(1, 3)]
    public string perfectMessage = "Perfeito! Você demonstrou total domínio na conservação patrimonial!";

    [Tooltip("Mensagem se acertar mais da metade")]
    [TextArea(1, 3)]
    public string goodMessage = "Bom trabalho! Com mais prática, você se tornará um expert!";

    [Tooltip("Mensagem se acertar metade ou menos")]
    [TextArea(1, 3)]
    public string needsPracticeMessage = "Continue estudando o Diário de Campo. A conservação patrimonial exige prática!";

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.onGameOver.AddListener(ShowResult);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.onGameOver.RemoveListener(ShowResult);
    }

    // ── Callbacks ─────────────────────────────────────────────────────────────

    private void ShowResult(int score, int errors, int attempts)
    {
        gameObject.SetActive(true);

        int total = GameManager.Instance?.TotalProblems ?? (score + errors);

        if (scoreText != null)
            scoreText.text = $"Você acertou {score} de {total} problemas.";

        if (attemptsText != null)
            attemptsText.text = $"Foram necessárias {attempts} tentativas.";

        if (chefText != null)
            chefText.text = GetChefMessage(score, total);
    }

    // ── Botão de reinício (chame via UnityEvent no Inspector) ─────────────────

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private string GetChefMessage(int score, int total)
    {
        if (total == 0) return perfectMessage;

        float ratio = (float)score / total;

        if (ratio >= 1f) return perfectMessage;
        if (ratio >= 0.5f) return goodMessage;
        return needsPracticeMessage;
    }
}