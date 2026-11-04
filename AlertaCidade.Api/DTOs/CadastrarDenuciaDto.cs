namespace AlertaCidade.Api.DTOs;

public class CadastrarDenunciaDto
{
    public string Titulo { get; set; } = default!;
    public string Descricao { get; set; } = default!;
    public LocalizacaoDto Localizacao { get; set; } = default!;
    public int CategoriaId { get; set; }
}