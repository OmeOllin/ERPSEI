using ERPSEI.Data.Entities.SAT;

namespace ERPSEI.Data.Managers.SAT
{
    public interface IPrefacturaManager : IRWCatalogoManager<Prefactura>
    {
		public Task<List<Prefactura>> GetAllAsync(
			DateTime? fechaInicio = null,
			DateTime? fechaFin = null,
			string? serie = null,
			int? monedaId = null,
			int? formaPagoId = null,
			int? metodoPagoId = null,
			int? usoCFDIId = null,
			string? usuarioCreadorId = null,
			string? usuarioTimbradorId = null,
			bool deshabilitado = false
		);

		public Task<Prefactura?> GetByIdWithAdicionalesAsync(int id);

	}
}