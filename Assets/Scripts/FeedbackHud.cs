using System.Collections;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Exibe feedback visual de acerto (✓ verde) e erro (✕ vermelho).
/// Canvas_Feedback deve usar Render Mode: Screen Space - Overlay.
///
/// Estrutura esperada:
///   Canvas_Feedback (Screen Space Overlay)
///     └── FeedbackHUD (este script)
///         ├── IconErro   (GameObject com CanvasGroup — ✕ vermelho)
///         └── IconAcerto (GameObject com CanvasGroup — ✓ verde)
/// </summary>
public class FeedbackHUD : MonoBehaviour
{
    [Header("Ícones de Feedback")]
    public CanvasGroup iconErro;
    public CanvasGroup iconAcerto;

    [Header("Tempo")]
    [Tooltip("Duração total do feedback em tela (segundos)")]
    public float duracao = 1.2f;

    [Header("Háptico (opcional)")]
    [Tooltip("Amplitude da vibração ao errar (0 = desativado)")]
    [Range(0f, 1f)]
    public float hapticAmplitude = 0.5f;

    [Tooltip("Duração da vibração em segundos")]
    public float hapticDuration = 0.3f;

    // ── API pública ───────────────────────────────────────────────────────────

    public void MostrarErro()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn(iconErro));

        if (hapticAmplitude > 0f)
            VibrarControle();
    }

    public void MostrarAcerto()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn(iconAcerto));
    }

    // ── Privados ──────────────────────────────────────────────────────────────

    private IEnumerator FadeIn(CanvasGroup grupo)
    {
        // Garante que outros ícones estejam ocultos
        OcultarTodos();

        grupo.gameObject.SetActive(true);
        grupo.alpha = 1f;

        // Mantém visível por 60% da duração
        yield return new WaitForSeconds(duracao * 0.6f);

        // Fade out nos 40% restantes
        float t = 0f;
        float fadeDuration = duracao * 0.4f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            grupo.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        grupo.gameObject.SetActive(false);
    }

    private void OcultarTodos()
    {
        if (iconErro   != null) iconErro.gameObject.SetActive(false);
        if (iconAcerto != null) iconAcerto.gameObject.SetActive(false);
    }

    private void VibrarControle()
    {
        var device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        device.SendHapticImpulse(0, hapticAmplitude, hapticDuration);
    }
}