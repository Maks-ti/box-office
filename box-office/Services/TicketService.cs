


using AutoMapper;
using box_office.DataBase;
using box_office.Models;
using Microsoft.EntityFrameworkCore;

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

    public async Task SetSold(TicketIsSoldUpdateModel model)
    {
        await using var context = GetContext<DataBaseContext>();

        var dbSet = context.Set<DataBase.Models.Ticket>();

        var ticket = await dbSet.FirstOrDefaultAsync(t => t.Id == model.TicketId);
        if (ticket == null) throw new ArgumentException($"Ticket с id = {model.TicketId} не существует");

        ticket.IsSold = model.IsSold;

        await context.SaveChangesAsync();
    }

}
