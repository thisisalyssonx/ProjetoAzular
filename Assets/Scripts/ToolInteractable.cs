using UnityEngine;


/// <summary>
/// Coloque este script em cada prefab de ferramenta (Brush, Shovel, Hammer, etc.).
/// Detecta quando a ferramenta, enquanto segurada, colide com a parede de problema.
/// 
/// Requisitos:
///   - O GameObject da ferramenta precisa de um XRGrabInteractable
///   - A parede precisa ter a tag "ProblemWall" e um Collider
///   - A ferramenta precisa de um Collider marcado como Trigger (isTrigger = true)
/// </summary>
[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable))]
public class ToolInteractable : MonoBehaviour
{
    [Header("Identificação")]
    [Tooltip("Deve bater EXATAMENTE com o campo correctToolName do WallProblem")]
    public string toolName;

    [Header("Configurações")]
    [Tooltip("Segundos de cooldown entre usos (evita spam de colisão)")]
    public float useCooldown = 1.5f;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grabInteractable;
    private float _lastUseTime = -99f;
    private bool IsGrabbed => _grabInteractable.isSelected;

    private void Awake()
    {
        _grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (string.IsNullOrEmpty(toolName))
            Debug.LogWarning($"[ToolInteractable] '{gameObject.name}' sem toolName definido!");
    }

    // ── Detecção de colisão com a parede ─────────────────────────────────────

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("ProblemWall")) return;
        if (!IsGrabbed) return;
        if (Time.time - _lastUseTime < useCooldown) return;

        _lastUseTime = Time.time;
        Use();
    }

    // Alternativa: colisão física normal (caso o trigger não seja ideal)
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("ProblemWall")) return;
        if (!IsGrabbed) return;
        if (Time.time - _lastUseTime < useCooldown) return;

        _lastUseTime = Time.time;
        Use();
    }

    // ── Ação principal ────────────────────────────────────────────────────────

    private void Use()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[ToolInteractable] GameManager não encontrado na cena!");
            return;
        }

        Debug.Log($"[ToolInteractable] Ferramenta usada: '{toolName}'");
        GameManager.Instance.SubmitAnswer(toolName);
    }
}