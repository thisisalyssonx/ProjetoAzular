using System.Collections;
using TMPro;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ── Dados ─────────────────────────────────────────────────────────────────

    [Header("Problemas")]
    public ProblemData[] problemas;

    [Header("Parede")]
    [Tooltip("MeshRenderer do Quad/Plane que exibe o problema")]
    public Renderer paredeRenderer;

    // ── HUD — Painel de Status ────────────────────────────────────────────────

    [Header("HUD — Status")]
    public TMP_Text textContador;    // ex: "3 / 5"
    public TMP_Text textAcertos;
    public TMP_Text textTentativas;

    // ── HUD — Painel Central ──────────────────────────────────────────────────

    [Header("HUD — Problema Central")]
    public TMP_Text textProblema;    // nome do problema atual
    public ProgressBarHUD progressBar;

    // ── Subsistemas ───────────────────────────────────────────────────────────

    [Header("Subsistemas")]
    public DiarioController diario;
    public FeedbackHUD feedback;

    [Header("Tela Final")]
    public GameObject canvasChef;
    public TMP_Text textoResultadoFinal;

    // ── Estado interno ────────────────────────────────────────────────────────

    private int _indiceAtual = 0;
    private int _acertos = 0;
    private int _tentativas = 0;
    private bool _jogoEncerrado = false;

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    private void Start()
    {
        if (problemas == null || problemas.Length == 0)
        {
            Debug.LogError("[GameManager] Nenhum ProblemData configurado no Inspector!");
            return;
        }

        if (canvasChef != null) canvasChef.SetActive(false);

        progressBar?.Inicializar(problemas.Length);
        CarregarProblema();
    }

    // ── API pública ───────────────────────────────────────────────────────────

    /// <summary>
    /// Chamado por FerramentaInteragivel quando o jogador usa uma ferramenta.
    /// </summary>
    public void OnFerramentaAplicada(ToolType ferramenta)
    {
        if (_jogoEncerrado) return;

        _tentativas++;

        var p = problemas[_indiceAtual];

        if (ferramenta == p.ferramentaCorreta)
        {
            _acertos++;
            feedback?.MostrarAcerto();
            _indiceAtual++;

            if (_indiceAtual >= problemas.Length)
                StartCoroutine(MostrarResultadoFinal());
            else
                CarregarProblema();
        }
        else
        {
            feedback?.MostrarErro();
        }

        AtualizarHUD();
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private void CarregarProblema()
    {
        var p = problemas[_indiceAtual];

        // Troca textura da parede
        if (paredeRenderer != null && p.texturaParede != null)
            paredeRenderer.material.SetTexture("_BaseMap", p.texturaParede);

        // Atualiza Diário de Campo
        diario?.AtualizarConteudo(p.descricaoDiario, p.nomeFerramentaDica);

        // Atualiza label central
        if (textProblema != null) textProblema.text = p.nomeProblem;

        AtualizarHUD();
    }

    private void AtualizarHUD()
    {
        int total = problemas.Length;
        int exibido = Mathf.Min(_indiceAtual + 1, total);

        if (textContador != null) textContador.text = $"{exibido} / {total}";
        if (textAcertos != null) textAcertos.text = _acertos.ToString();
        if (textTentativas != null) textTentativas.text = _tentativas.ToString();

        progressBar?.Atualizar(_acertos);
    }

    private IEnumerator MostrarResultadoFinal()
    {
        _jogoEncerrado = true;

        yield return new WaitForSeconds(1.5f);

        if (canvasChef != null) canvasChef.SetActive(true);

        int total = problemas.Length;
        int eficiencia = _tentativas > 0
            ? Mathf.RoundToInt((float)_acertos / _tentativas * 100)
            : 100;

        if (textoResultadoFinal != null)
        {
            textoResultadoFinal.text =
                $"Você acertou {_acertos} de {total} problemas.\n" +
                $"Foram necessárias {_tentativas} tentativas.\n" +
                $"Eficiência: {eficiencia}%";
        }

        Debug.Log($"[GameManager] Fim — Acertos: {_acertos}/{total} | Tentativas: {_tentativas} | Eficiência: {eficiencia}%");
    }
}