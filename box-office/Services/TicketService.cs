


using AutoMapper;

namespace box_office.Services;

public class TicketService : ServiceBase<DataBase.Models.Ticket>
{
    public TicketService(IServiceProvider serviceProvider, ILogger<TicketService> logger)
    : base(serviceProvider, logger)
    {
        this.Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;

            #region Base To ViewModel

            #endregion

            #region ViewModel To Base

            #endregion
        }).CreateMapper();
    }



}
