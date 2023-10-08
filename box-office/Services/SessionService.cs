

using AutoMapper;
using box_office.DataBase;
using box_office.Models;
using Microsoft.EntityFrameworkCore;

namespace box_office.Services;

public class SessionService : ServiceBase<DataBase.Models.Session>
{
    public SessionService(IServiceProvider serviceProvider, ILogger<PlayService> logger)
        : base(serviceProvider, logger)
    {
        this.Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;

            #region Base To ViewModel
            cfg.CreateMap<DataBase.Models.Session, Session>();
            cfg.CreateMap<DataBase.Models.Play, Play>();
            cfg.CreateMap<DataBase.Models.Hall, Hall>();
            #endregion

            #region ViewModel To Base
            cfg.CreateMap<Session, DataBase.Models.Session>();
            cfg.CreateMap<SessionCreateModel, DataBase.Models.Session>();
            cfg.CreateMap<SessionUpdateModel, DataBase.Models.Session>();
            #endregion
        }).CreateMapper();
    }

    public override async Task<IEnumerable<DataBase.Models.Session>> GetAllAsync()
    {
        await using var context = GetContext<DataBaseContext>();

        var dbSet = context.Set<DataBase.Models.Session>();

        return await dbSet.Include(s => s.Play).Include(s => s.Hall).ToListAsync();
    }

    public async Task<List<Session>> GetAll()
    {
        var baseResult = (await GetAllAsync()).ToList();

        return Mapper.Map<List<Session>>(baseResult);
    }

    public async Task<Session> GetByIdAsync(int id)
    {
        var baseResult = await base.GetByIdAsync(id);

        return Mapper.Map<Session>(baseResult);
    }

    public override async Task<DataBase.Models.Session> AddAsync(DataBase.Models.Session session)
    {
        if (session.DateTo < session.DateFrom)
            throw new ArgumentException($"DateTo не может быть меньше DateFrom");

        await using var context = GetContext<DataBaseContext>();

        var dbSet = context.Set<DataBase.Models.Session>();

        // перед созданием сеанса необходимо проверить что нет сеанса на данный промежуток времени в данном зале
        if (
            context.Sessions.Any(s => (s.DateFrom <= session.DateFrom && s.DateTo >= session.DateFrom
                                    || s.DateFrom <= session.DateTo && s.DateTo >= session.DateTo)
                                    && s.HallId == session.HallId)
            ) 
        {
            throw new ArgumentException($"В данном интерале времени залл с Id = {session.HallId} занят");
        }
        
        dbSet.Add(session);
        await context.SaveChangesAsync();

        // при создании сеанса нужно автоматически создать на него билеты
        // получаем все места для зала в котором будет сеанс
        var places = await context.Places.Where(place => place.HallId == session.HallId).ToListAsync();

        // для каждого места создаём билет на данный сеанс
        foreach (var place in places)
        {
            var ticket = new DataBase.Models.Ticket
            {
                Id = 0,
                SessionId = session.Id,
                PlaceId = place.Id,
                IsSold = false
            };

            context.Tickets.Add(ticket);
        }

        await context.SaveChangesAsync();

        return session;
    }

    public async Task<Session> CreateAsync(SessionCreateModel model)
    {
        var session = Mapper.Map<DataBase.Models.Session>(model);

        var baseResult = await AddAsync(session);

        return Mapper.Map<Session>(baseResult);
    }
}
