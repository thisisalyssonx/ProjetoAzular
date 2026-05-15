using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FerramentaInteragivel : MonoBehaviour
{
    [Tooltip("Tipo desta ferramenta — deve bater com o ProblemData correspondente")]
    public ToolType tipoFerramenta;


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