namespace AlertaCidade.Api.DTOs;

public class CadastrarDenunciaDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public LocalizacaoDto? Localizacao { get; set; }
    public int CategoriaId { get; set; }
}