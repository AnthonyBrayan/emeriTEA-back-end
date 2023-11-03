using Data;
using emeriTEA_back_end.IServices;
using Entities;

namespace emeriTEA_back_end.Services
{
    public class AdministradorService : BaseContextService, IAdministradorService
    {
        public AdministradorService(ServiceContext serviceContext) : base(serviceContext)
        {
        }
        public int InsertAdministrador(Administrador administrador)
        {
            _serviceContext.Administrador.Add(administrador);
            _serviceContext.SaveChanges();
            return administrador.Id_Administrador;
        }




    }
}
