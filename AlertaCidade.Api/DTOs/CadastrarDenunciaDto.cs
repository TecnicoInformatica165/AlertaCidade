namespace AlertaCidade.Api.DTOs;

public class CadastrarDenunciaDto
{
    public string Titulo { get; set; } = "";
    public string Descricao { get; set; } = "";
    public LocalizacaoDto Localizacao { get; set; } = new LocalizacaoDto();
    public int CategoriaId { get; set; }
}