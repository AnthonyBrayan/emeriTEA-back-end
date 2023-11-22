using Data;
using emeriTEA_back_end.IServices;
using Entities;

namespace emeriTEA_back_end.Services
{
    public class GuestCardService : BaseContextService, IGuestCardService
    {
        public GuestCardService(ServiceContext serviceContext) : base(serviceContext)
        {
        }

        public void AddGuestCart(GuestCart guestCart)
        {
            // Guarda el nuevo objeto GuestCart en la base de datos
            _serviceContext.GuestCart.Add(guestCart);
            _serviceContext.SaveChanges();
        }


    }
}
