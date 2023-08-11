using ASP__Music_Server.HubMusic;
using ASP__Music_Server.Repository;
using ASP__Music_Server.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using ASP__Music_Server.Services.Hash;
using ASP__Music_Server.Services.Kdf;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("MusicServer.json", false);//ϳ��������� ���� MusicServer.json
// ��� ������������� ���������������� ���������� SignalR,
// � ���������� ���������� ���������������� ��������������� �������
builder.Services.AddSignalR();
builder.Services.AddCors(); // ��������� ������� CORS

builder.Services.AddSingleton<IHashservice, Md5Hashservice>();
builder.Services.AddSingleton<IHashservice, Sh1Hashservice>();
builder.Services.AddSingleton<IKdfServise, HashBasedKdfService>();
builder.Services.AddCounterService();
// �������� ������ ����������� �� ����� ������������
String? connectionString = builder.Configuration.GetConnectionString("PlanetScale");//PlanetScale- ����� ����� ���������� � ���� azuresettings.json 
MySqlConnection connection = new(connectionString);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        connection,
        ServerVersion.AutoDetect(connection),
        serverOptions =>
            serverOptions
                .MigrationsHistoryTable(
                    tableName: HistoryRepository.DefaultTableName,
                    schema: DataContext.SchemaName)
                .SchemaBehavior(
                    MySqlSchemaBehavior.Translate, 
                    (schema, table) => $"{schema}_{table}")
));


var app = builder.Build();


app.UseCors(builder => builder
    .WithOrigins("https://musicsua.azurewebsites.net") //���� ����� ���������� �� ������� � ������  https://musicsua.azurewebsites.net
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<MusicHub>("/MusicHub");

app.Run();
