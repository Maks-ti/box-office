
using AutoMapper;
using box_office.DataBase;
using box_office.Models;
using Microsoft.EntityFrameworkCore;

namespace box_office.Services;

public class HallService : ServiceBase<DataBase.Models.Hall>
{
    public HallService(IServiceProvider serviceProvider, ILogger<HallService> logger)
        : base(serviceProvider, logger)
    {
        this.Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;

            #region Base To ViewModel
            cfg.CreateMap<DataBase.Models.Hall, Hall>();
            #endregion

            #region ViewModel To Base
            cfg.CreateMap<Hall, DataBase.Models.Hall>();
            cfg.CreateMap<HallCreateModel, DataBase.Models.Hall>();
            #endregion
        }).CreateMapper();
    }

    public async Task<List<Hall>> GetAllAsync()
    {
        var baseResult = (await base.GetAllAsync()).ToList();

        return Mapper.Map<List<Hall>>(baseResult);
    }

    public async Task<Hall> GetByIdAsync(int id)
    {
        var baseResult = await base.GetByIdAsync(id);

        return Mapper.Map<Hall>(baseResult);
    }

    public override async Task<DataBase.Models.Hall> AddAsync(DataBase.Models.Hall model)
    {
        await using var context = GetContext<DataBaseContext>();

        DbSet<DataBase.Models.Hall> dbSet = context.Set<DataBase.Models.Hall>();

        // вместе с созданием зала нужно создать, связанные места
        dbSet.Add(model);

        await context.SaveChangesAsync();

        for (int i =0; i< model.Size; i++)
        {
            var place = new DataBase.Models.Place
            {
                Id = 0,
                HallId = model.Id,
                Name = $"place {i}"
            };

            context.Places.Add(place);
        }

        await context.SaveChangesAsync();

        return model;
    }

    public async Task<Hall> CreateAsync(HallCreateModel model)
    {
        var hall = Mapper.Map<DataBase.Models.Hall>(model);

        var baseResult = await AddAsync(hall);

        return Mapper.Map<Hall>(baseResult);
    }
}
