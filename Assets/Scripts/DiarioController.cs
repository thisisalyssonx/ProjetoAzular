using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiarioController : MonoBehaviour
{
    [Header("Canvas do Diário")]
    [Tooltip("Canvas_Diario — começa desativado")]
    public GameObject canvasDiario;

    [Header("Input")]
    [Tooltip("Action do botão B (mão direita) ou Y (mão esquerda)")]
    public InputActionReference toggleAction;

    [Header("Textos do Diário")]
    public TMP_Text textoDescricao;
    public TMP_Text textoFerramenta;

    private void OnEnable()
    {
        if (toggleAction != null)
            toggleAction.action.performed += Toggle;
    }

    private void OnDisable()
    {
        if (toggleAction != null)
            toggleAction.action.performed -= Toggle;
    }

    private void Start()
    {
        if (canvasDiario != null)
            canvasDiario.SetActive(false);
    }

    // ── API pública (chamada pelo GameManager) ────────────────────────────────

    /// <summary>
    /// Atualiza o conteúdo exibido no diário com os dados do problema atual.
    /// </summary>
    public void AtualizarConteudo(string descricao, string ferramenta)
    {
        if (textoDescricao != null) textoDescricao.text = descricao;
        if (textoFerramenta != null) textoFerramenta.text = ferramenta;
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private void Toggle(InputAction.CallbackContext ctx)
    {
        if (canvasDiario == null) return;
        canvasDiario.SetActive(!canvasDiario.activeSelf);
    }
}