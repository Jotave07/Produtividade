using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Produtividade.Models;
using System.Data;

public class ProdutividadeService
{
    private readonly string _connectionString;

    public ProdutividadeService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("OracleConnection");
    }

    public List<ProdutividadeSeparacao> BuscarDadosSeparacao(DateTime dataInicio, DateTime dataFim)
    {
        var listaSeparacao = new List<ProdutividadeSeparacao>();

        // Dentro do método BuscarDadosSeparacao
        string sqlQuery = @"
    SELECT
    --    TRUNC(m.dtfimseparacao) AS dtsep,
        m.codfuncos,
        e.nome,
        t.codigo AS tipoos,
        t.descricao,
        COUNT(DISTINCT m.numos) quant_os,
        COUNT(m.codendereco) apanhas,
        SUM(m.qt) volume,
        TRUNC(SUM(pedi.pvenda * m.qt), 2) valor
    FROM
        pcmovendpend m,
        pcempr e,
        pcprodut p,
        pctipoos t,
        pcest est,
        pcpedi pedi
    WHERE
        m.codfuncos = e.matricula
        AND p.codprod = est.codprod(+)
        AND m.codprod = p.codprod
        AND m.dtestorno IS NULL
        AND m.numped = pedi.numped(+)
        AND m.codprod = pedi.codprod(+)
        AND m.tipoos = t.codigo
        AND est.codfilial = 3
        AND m.codoper = 'S'
        AND m.dtfimseparacao >= :dtinicio
        AND m.dtfimseparacao < :dtfim
    GROUP BY
       
         --   TRUNC(m.dtfimseparacao),
            m.codfuncos,
            e.nome,
            t.codigo,
            t.descricao
        
    ORDER BY
        apanhas DESC, e.nome ASC ";

        using (var connection = new OracleConnection(_connectionString))
        {
            connection.Open();
            using (var command = new OracleCommand(sqlQuery, connection))
            {
                command.Parameters.Add(new OracleParameter("dtinicio", dataInicio));
                command.Parameters.Add(new OracleParameter("dtfim", dataFim.AddDays(1)));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaSeparacao.Add(new ProdutividadeSeparacao
                        {
                         //   DataSeparacao = reader["dtsep"].ToString(),
                            CodigoFuncionario = reader.IsDBNull(reader.GetOrdinal("codfuncos")) ? 0 : Convert.ToInt32(reader["codfuncos"]),
                            NomeFuncionario = reader["nome"].ToString(),
                            TipoOS = reader.IsDBNull(reader.GetOrdinal("tipoos")) ? 0 : Convert.ToInt32(reader["tipoos"]),
                            DescricaoTipoOS = reader["descricao"].ToString(),
                            QuantidadeOS = reader.IsDBNull(reader.GetOrdinal("quant_os")) ? 0 : Convert.ToInt32(reader["quant_os"]),
                            Apanhas = reader.IsDBNull(reader.GetOrdinal("apanhas")) ? 0 : Convert.ToInt32(reader["apanhas"]),
                            Volume = reader.IsDBNull(reader.GetOrdinal("volume")) ? 0 : Convert.ToDecimal(reader["volume"]),
                            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? 0 : Convert.ToDecimal(reader["valor"])
                        });
                    }
                }
            }
        }

        return listaSeparacao;
    }

    // Método para buscar os dados de Conferência
    public List<ProdutividadeConferencia> BuscarDadosConferencia(DateTime dataInicio, DateTime dataFim)
    {
        var listaConferencia = new List<ProdutividadeConferencia>();

        string sqlQuery = @"
    SELECT
      --  TRUNC(m.dtfimconferencia) AS dtconf,
        e.matricula,
        e.nome,
        e.funcao,
        COUNT(DISTINCT m.numos) quant_os,
        COUNT(m.codendereco) apanhas,
        SUM(m.QT) volume,
        TRUNC(SUM(pedi.pvenda * m.qt), 2) valor
    FROM
        PCMOVENDPEND M,
        PCEMPR E,
        PCPRODUT P,
        pcpedc pedc,
        pcpedi pedi
    WHERE
        M.codfuncconf = E.MATRICULA
        AND m.numped = pedi.numped
        AND m.codprod = pedi.codprod
        AND pedc.numped = pedi.numped
        AND M.CODPROD = P.CODPROD
        AND M.TIPOOS IN (13, 9, 10, 13, 22, 20, 41)
        AND M.DTESTORNO IS NULL
        AND m.posicao = 'C'
        AND m.dtfimconferencia >= :dtinicio
        AND m.dtfimconferencia < :dtfim
    GROUP BY
      
        --    TRUNC(m.dtfimconferencia),
            e.matricula,
            e.nome,
            e.funcao
        
    ORDER BY
       apanhas DESC, e.nome ASC
";

        using (var connection = new OracleConnection(_connectionString))
        {
            connection.Open();
            using (var command = new OracleCommand(sqlQuery, connection))
            {
                command.Parameters.Add(new OracleParameter("dtinicio", dataInicio));
                command.Parameters.Add(new OracleParameter("dtfim", dataFim.AddDays(1)));

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listaConferencia.Add(new ProdutividadeConferencia
                        {
                         //   DataConferencia = reader["dtconf"].ToString(),
                            MatriculaFuncionario = reader.IsDBNull(reader.GetOrdinal("matricula")) ? 0 : Convert.ToInt32(reader["matricula"]),
                            NomeFuncionario = reader["nome"].ToString(),
                            QuantidadeOS = reader.IsDBNull(reader.GetOrdinal("quant_os")) ? 0 : Convert.ToInt32(reader["quant_os"]),
                            Apanhas = reader.IsDBNull(reader.GetOrdinal("apanhas")) ? 0 : Convert.ToInt32(reader["apanhas"]),
                            Volume = reader.IsDBNull(reader.GetOrdinal("volume")) ? 0 : Convert.ToDecimal(reader["volume"]),
                            Valor = reader.IsDBNull(reader.GetOrdinal("valor")) ? 0 : Convert.ToDecimal(reader["valor"])
                        });
                    }
                }
            }
        }

        return listaConferencia;
    }
}