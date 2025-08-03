using Microsoft.EntityFrameworkCore;
using HotelWebsiteBuilder.Models;
using HotelWebsiteBuilder.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add HttpClient
builder.Services.AddHttpClient();

// Add Entity Framework
// Always use in-memory database for development
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("HotelWebsiteBuilderDb"));

// Add Services
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddScoped<ITemplateService, TemplateService>();
builder.Services.AddScoped<IHtmlAnalysisService, HtmlAnalysisService>();
builder.Services.AddScoped<IHtmlUpdateService, HtmlUpdateService>();
// builder.Services.AddScoped<SiteCloneService>(); // Temporarily disabled - file not found
builder.Services.AddScoped<HotelSiteCloneService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
