namespace Produtividade.Models
{
    public class ProdutividadeSeparacao
    {
        public string DataSeparacao { get; set; }
        public int CodigoFuncionario { get; set; }
        public string NomeFuncionario { get; set; }
        public int TipoOS { get; set; }
        public string DescricaoTipoOS { get; set; }
        public int QuantidadeOS { get; set; }
        public decimal Volume { get; set; }
        public int Apanhas { get; set; }
        public decimal Valor { get; set; }
    }
}
