using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Produtividade.Models;

public class ProdutividadeModel : PageModel
{
    private readonly ProdutividadeService _produtividadeService;

    // Propriedades para guardar os dados dos relat�rios
    public List<ProdutividadeSeparacao> DadosSeparacao { get; set; }
    public List<ProdutividadeConferencia> DadosConferencia { get; set; }

    // Propriedades para os filtros de data
    [BindProperty(SupportsGet = true)]
    public DateTime? DataInicio { get; set; }

    [BindProperty(SupportsGet = true)]
    public DateTime? DataFim { get; set; }

    public ProdutividadeModel(ProdutividadeService produtividadeService)
    {
        _produtividadeService = produtividadeService;
    }

    public void OnGet()
    {
        // Se os filtros n�o foram definidos, usa o m�s atual como padr�o
        if (DataInicio == null || DataFim == null)
        {
            DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DataFim = DateTime.Now;
        }

        // Chama o servi�o para buscar os dados com base nos filtros
        DadosSeparacao = _produtividadeService.BuscarDadosSeparacao(DataInicio.Value, DataFim.Value);
        DadosConferencia = _produtividadeService.BuscarDadosConferencia(DataInicio.Value, DataFim.Value);
    }
}