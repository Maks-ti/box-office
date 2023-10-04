using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using box_office.DataBase.Models; // модели сущностей из базы
using System.Reflection; // for Assembly

namespace box_office.DataBase;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }

    // список DbSet`ов
    public DbSet<Play> Plays { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Конфигурации хранятся рядом с моделями сущностей базы (в том же файле)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}
