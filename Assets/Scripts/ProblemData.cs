using UnityEngine;

/// <summary>
/// ScriptableObject que define um problema de conservação patrimonial.
/// Crie instâncias pelo menu: Assets > Create > Jogo > Problema
///
/// Problemas padrão do GDD:
///   Problema_Umidade    → Higrometro
///   Problema_Mofo       → Higrometro
///   Problema_Cupim      → Lupa ou CameraInspecao
///   Problema_Rachadura  → Lupa ou Espatula
/// </summary>
[CreateAssetMenu(menuName = "Jogo/Problema", fileName = "Novo Problema")]
public class ProblemData : ScriptableObject
{
    [Header("Identificação")]
    [Tooltip("Nome exibido na HUD central, ex: 'Umidade'")]
    public string nomeProblem;

    [Tooltip("Textura aplicada na parede quando este problema estiver ativo")]
    public Texture2D texturaParede;

    [Header("Diário de Campo")]
    [TextArea(2, 4)]
    [Tooltip("Texto de apoio exibido no Diário de Campo")]
    public string descricaoDiario;

    [Tooltip("Nome da ferramenta exibido no Diário como dica, ex: 'Higrômetro'")]
    public string nomeFerramentaDica;

    [Header("Solução")]
    [Tooltip("Ferramenta que resolve este problema")]
    public ToolType ferramentaCorreta;

    [Header("Mensagens de Feedback")]
    public string mensagemAcerto  = "Correto! Problema resolvido.";
    public string mensagemErro    = "Ferramenta incorreta. Tente outra.";
}