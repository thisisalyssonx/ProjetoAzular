using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controla o estado global do jogo: sequência de problemas,
/// pontuação, verificação de respostas e transições.
/// Coloque este script em um GameObject vazio chamado "GameManager" na cena.
/// </summary>
public class GameManager : MonoBehaviour
{
    // Singleton acessível por qualquer script
    public static GameManager Instance { get; private set; }

    // ── Configuração ──────────────────────────────────────────────────────────

    [Header("Problemas do Jogo")]
    [Tooltip("Lista de WallProblems na ordem em que serão apresentados")]
    public WallProblem[] problems;

    [Header("Configurações de Tempo")]
    [Tooltip("Segundos de espera antes de carregar o próximo problema")]
    public float delayBetweenProblems = 2f;

    // ── Eventos (outros scripts se inscrevem aqui) ─────────────────────────────

    [Header("Eventos")]
    public UnityEvent<WallProblem> onProblemLoaded;   // novo problema ativado
    public UnityEvent<bool> onAnswerChecked;           // true = acerto, false = erro
    public UnityEvent<int, int, int> onGameOver;       // acertos, erros, tentativas totais

    // ── Estado interno ────────────────────────────────────────────────────────

    public enum GameState { Waiting, Checking, Transitioning, GameOver }
    public GameState CurrentState { get; private set; } = GameState.Waiting;

    public WallProblem CurrentProblem => problems != null && _currentIndex < problems.Length
        ? problems[_currentIndex] : null;

    public int Score { get; private set; }  // acertos
    public int Errors { get; private set; }  // erros
    public int Attempts { get; private set; }  // tentativas totais
    public int TotalProblems => problems?.Length ?? 0;
    public int CurrentIndex => _currentIndex;

    private int _currentIndex = 0;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        if (problems == null || problems.Length == 0)
        {
            Debug.LogWarning("[GameManager] Nenhum WallProblem configurado!");
            return;
        }
        LoadProblem(0);
    }

    // ── API pública ───────────────────────────────────────────────────────────

    /// <summary>
    /// Chamado pelo ToolInteractable quando o jogador usa uma ferramenta na parede.
    /// </summary>
    public void SubmitAnswer(string toolName)
    {
        if (CurrentState != GameState.Waiting) return;

        CurrentState = GameState.Checking;
        Attempts++;

        bool isCorrect = IsCorrectTool(toolName);

        if (isCorrect)
            Score++;
        else
            Errors++;

        onAnswerChecked?.Invoke(isCorrect);

        if (isCorrect)
            StartCoroutine(AdvanceAfterDelay());
        else
            CurrentState = GameState.Waiting; // permite nova tentativa
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private bool IsCorrectTool(string toolName)
    {
        if (CurrentProblem == null) return false;
        return string.Equals(toolName.Trim(), CurrentProblem.correctToolName.Trim(),
                             System.StringComparison.OrdinalIgnoreCase);
    }

    private void LoadProblem(int index)
    {
        _currentIndex = index;
        CurrentState = GameState.Waiting;
        onProblemLoaded?.Invoke(CurrentProblem);
        Debug.Log($"[GameManager] Problema carregado: {CurrentProblem?.problemName}");
    }

    private IEnumerator AdvanceAfterDelay()
    {
        CurrentState = GameState.Transitioning;
        yield return new WaitForSeconds(delayBetweenProblems);

        int next = _currentIndex + 1;
        if (next < problems.Length)
            LoadProblem(next);
        else
            EndGame();
    }

    private void EndGame()
    {
        CurrentState = GameState.GameOver;
        Debug.Log($"[GameManager] Fim de jogo — Acertos: {Score} / Tentativas: {Attempts}");
        onGameOver?.Invoke(Score, Errors, Attempts);
    }
}