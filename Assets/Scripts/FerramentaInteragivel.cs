using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Coloque este script em cada prefab de ferramenta do PolyRonin.
/// Quando o jogador PEGA a ferramenta (SelectEntered), avisa o GameManager.
///
/// No XRGrabInteractable de cada ferramenta:
///   Interactable Events > Select Entered > arraste AplicarFerramenta
///
/// IMPORTANTE: tipoFerramenta deve bater com ferramentaCorreta do ProblemData.
/// </summary>
public class FerramentaInteragivel : MonoBehaviour
{
    [Tooltip("Tipo desta ferramenta — deve bater com o ProblemData correspondente")]
    public ToolType tipoFerramenta;

    /// <summary>
    /// Conecte este método ao evento Select Entered do XRGrabInteractable no Inspector.
    /// </summary>
    public void AplicarFerramenta(SelectEnterEventArgs args)
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("[FerramentaInteragivel] GameManager não encontrado na cena!");
            return;
        }

        Debug.Log($"[FerramentaInteragivel] Ferramenta usada: {tipoFerramenta}");
        GameManager.Instance.OnFerramentaAplicada(tipoFerramenta);
    }
}