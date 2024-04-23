using ERPSEI.Data.Entities.SAT;

namespace ERPSEI.Data.Managers.SAT
{
    public interface IPrefacturaManager : IRWCatalogoManager<Prefactura>
    {
		public Task<List<Prefactura>> GetAllAsync(
			DateTime? fechaInicio = null,
			DateTime? fechaFin = null,
			int? serie = null,
			int? monedaId = null,
			int? formaPagoId = null,
			int? metodoPagoId = null,
			int? usoCFDIId = null
		);

	}
}