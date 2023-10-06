

using AutoMapper;
using box_office.DataBase;
using box_office.Models;
using Microsoft.EntityFrameworkCore;

namespace box_office.Services;

public class PlayService : ServiceBase<DataBase.Models.Play>
{
    public PlayService(IServiceProvider serviceProvider, ILogger<PlayService> logger)
        : base(serviceProvider, logger)
    {
        this.Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;

            #region Base To ViewModel
            cfg.CreateMap<DataBase.Models.Play, Play>();
            #endregion

            #region ViewModel To Base
            cfg.CreateMap<Play, DataBase.Models.Play>();
            #endregion
        }).CreateMapper();
    }

    public async Task<List<Play>> GetAllAsync()
    {
        var baseResult = (await base.GetAllAsync()).ToList();

        return Mapper.Map<List<Play>>(baseResult);
    } 

    public async Task<Play> GetByIdAsync(int id)
    {
        var baseResult = await base.GetByIdAsync(id);

        return Mapper.Map<Play>(baseResult);
    }

    public async Task<Play> CreateAsync(IFormFile pictureFile, string name, string description)
    {
        if (pictureFile == null || pictureFile.Length == 0) { throw new FileNotFoundException("файл пуст"); }

        // Чтение потока данных из IFormFile в массив байт
        using var ms = new MemoryStream();
        await pictureFile.CopyToAsync(ms);
        var pictureData = ms.ToArray();

        // Извлечение расширения файла
        var extension = Path.GetExtension(pictureFile.FileName)?.TrimStart('.');

        var play = new DataBase.Models.Play
        {
            Id = 0,
            Name = name,
            Description = description,
            PictureData = pictureData,
            PictureExtension = extension
        };

        var baseResult = await base.AddAsync(play);

        return Mapper.Map<Play>(baseResult);
    }

    public override async Task<DataBase.Models.Play> UpdateAsync(DataBase.Models.Play basePlay)
    {
        await using var context = GetContext<DataBaseContext>();

        DbSet<DataBase.Models.Play> dbSet = context.Set<DataBase.Models.Play>();

        var oldPlay = await dbSet.FirstOrDefaultAsync(p => p.Id == basePlay.Id);

        if (oldPlay == null) return null;

        if (basePlay.PictureData != null && basePlay.PictureExtension != null)
        {
            oldPlay.PictureData = basePlay.PictureData;
            oldPlay.PictureExtension = basePlay.PictureExtension;
        }
        oldPlay.Name = basePlay.Name;
        oldPlay.Description = basePlay.Description;

        await context.SaveChangesAsync();

        return oldPlay;
    }

    public async Task<Play> UpdateAsync(IFormFile pictureFile, string name, string description)
    {
        bool fileIsEmpty = pictureFile == null || pictureFile.Length == 0;

        string extension = null;
        byte[] pictureData = null;

        if (!fileIsEmpty) 
        {
            // Чтение потока данных из IFormFile в массив байт
            using var ms = new MemoryStream();
            await pictureFile.CopyToAsync(ms);
            pictureData = ms.ToArray();

            // Извлечение расширения файла
            extension = Path.GetExtension(pictureFile.FileName)?.TrimStart('.');
        }

        var play = new DataBase.Models.Play
        {
            Id = 0,
            Name = name,
            Description = description,
            PictureData = pictureData,
            PictureExtension = extension
        };

        var baseResult = await UpdateAsync(play);

        return Mapper.Map<Play>(baseResult);
    }
}
