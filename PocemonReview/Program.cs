using Microsoft.EntityFrameworkCore;
using PocemonReview.Data;
using PocemonReview.IRepository;
using PocemonReview.Repository;
using PokemonReview;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddRouting();
builder.Services.AddControllers();
builder.Services.AddTransient<Seed>();
builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<ICategorrRepository,CategoryRepository>();
builder.Services.AddScoped<ICountryRepository,CountryRepository>();
builder.Services.AddScoped<IOwnerRepository,OwnerRepository>();
builder.Services.AddScoped<IReviwRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewerRepository, ReviewerRepository>();

var app = builder.Build();

var routeBuilder = new RouteBuilder(app);

routeBuilder.MapRoute("controller", async context =>
{
    await context.Response.WriteAsync("{controller} template");
});

routeBuilder.MapRoute("{controller}/{action}", async context =>
{
    await context.Response.WriteAsync("{controller}/{action} template");
});

app.UseRouting();

if (args.Length == 1 && args[0].ToLower()=="seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        service.SeedDataContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
