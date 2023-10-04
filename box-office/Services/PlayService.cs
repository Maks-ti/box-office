

using AutoMapper;


namespace box_office.Services;

public class PlayService : ServiceBase<DataBase.Models.Play>
{
    public PlayService(IServiceProvider serviceProvider, ILogger<PlayService> logger)
        : base(serviceProvider, logger)
    {
        this.Mapper = new MapperConfiguration(cfg =>
        {
            #region Base To ViewModel

            #endregion


            #region ViewModel To Base

            #endregion
        }).CreateMapper();
    }



}
