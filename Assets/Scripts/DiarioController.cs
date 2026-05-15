using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controla o Diário de Campo: abre/fecha com botão B (mão direita)
/// ou Y (mão esquerda) no Meta Quest, e exibe o conteúdo do problema atual.
///
/// Coloque este script em um GameObject vazio filho do HUD_Root.
///
/// No InputActionReference, arraste a action:
///   XRI RightHand / Secondary Button  (botão B)
///   ou
///   XRI LeftHand / Secondary Button   (botão Y)
/// </summary>
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
        if (textoDescricao  != null) textoDescricao.text  = descricao;
        if (textoFerramenta != null) textoFerramenta.text = ferramenta;
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private void Toggle(InputAction.CallbackContext ctx)
    {
        if (canvasDiario == null) return;
        canvasDiario.SetActive(!canvasDiario.activeSelf);
    }
}