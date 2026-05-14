using UnityEngine;
[CreateAssetMenu(menuName = "ProjetoAzular/Wall Problem", fileName = "NewWallProblem")]
public class WallProblem : ScriptableObject
{
    [Header("Identificação")]
    [Tooltip("Nome exibido na UI e no Diário de Campo")]
    public string problemName;

    [TextArea(2, 4)]
    [Tooltip("Descrição do problema para o Diário de Campo")]
    public string description;

    [Header("Visual da Parede")]
    [Tooltip("Textura aplicada na parede para representar o problema")]
    public Texture2D problemTexture;

    [Tooltip("Cor de tint aplicada sobre a textura (deixe branco para sem tint)")]
    public Color wallTint = Color.white;

    [Header("Solução")]
    [Tooltip("Nome exato do prefab/ferramenta que resolve este problema")]
    public string correctToolName;

    [Tooltip("Descrição da ferramenta correta (usada no Diário de Campo)")]
    [TextArea(1, 3)]
    public string correctToolDescription;

    [Header("Feedback")]
    [Tooltip("Mensagem exibida ao acertar")]
    public string successMessage = "Correto! Problema resolvido.";

    [Tooltip("Mensagem exibida ao errar")]
    public string failMessage = "Ferramenta incorreta. Tente outra.";
}