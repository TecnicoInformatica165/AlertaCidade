using AlertaCidade.Core.Exceptions;

namespace AlertaCidade.Core.Entidades;

public class Localizacao
{
    protected Localizacao()
    {
    }

    public Localizacao(decimal latitude, decimal longitude, decimal precisao)
    {
        ValidarLatitude(latitude);
        ValidarLongitude(longitude);
        ValidarPrecisao(precisao);
    }

    public decimal Latitude { get; private set; }
    public decimal Longitude { get; private set; }
    public decimal Precisao { get; private set; }

    private void ValidarPrecisao(decimal precisao)
    {
        if (decimal.IsNegative(precisao))
            throw new CoreException($"A precisão informada ({precisao}) é inválida. O valor não pode ser negativo.");

        Precisao = precisao;
    }

    private void ValidarLongitude(decimal longitude)
    {
        if (longitude is > 180 or < -180)
            throw new CoreException(
                $"A longitude informada ({longitude}) é inválida. O valor deve estar entre -180 e 180 graus.");

        Longitude = longitude;
    }

    private void ValidarLatitude(decimal latitude)
    {
        if (latitude is > 90 or < -90)
            throw new CoreException(
                $"A latitude informada ({latitude}) é inválida. O valor deve estar entre -90 e 90 graus.");

        Latitude = latitude;
    }
}