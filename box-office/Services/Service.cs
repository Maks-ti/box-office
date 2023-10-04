

using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace box_office.Services;

public partial class Service
{
    public Service(ILogger<Service> logger, IServiceProvider serviceProvider) 
    {
        ServiceProvider = serviceProvider;
        Logger = logger;

        Mapper = new MapperConfiguration(cfg => 
        {
            #region Base To ViewModel

            #endregion


            #region ViewModel To Base

            #endregion
        }).CreateMapper();
    }

    protected ILogger<Service> Logger { get; }
    private IMapper Mapper;
    private IServiceProvider ServiceProvider { get; }

    protected ContextType GetContext<ContextType>()
    where ContextType : DbContext
    {
        return ServiceProvider.GetRequiredService<ContextType>();
    }

}
