using System.Text.Json.Serialization;
using Api.Middleware;
using Infrastructure;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDistributedMemoryCache();
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString,
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
    
var dbConfig = new DatabaseConfig();
builder.Configuration.GetSection("DatabaseConfig").Bind(dbConfig);
builder.Services.AddSingleton(dbConfig);


builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IPasswordHashRepository, PasswordHashRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IBlogRepository, BlogRepository>();
builder.Services.AddSingleton<ICommentRepository, CommentRepository>();


builder.Services.AddSingleton<AccountService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<CommentService>();





builder.Services.AddCors(options =>

{
    options.AddPolicy("DevCorsPolicy", builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });

    options.AddPolicy("ProdCorsPolicy", builder =>
    {
        builder.WithOrigins("https://blogwebappremake.web.app")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var frontEndRelativePath = "./../frontend/www";
builder.Services.AddSpaStaticFiles(conf => conf.RootPath = frontEndRelativePath);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCorsPolicy");
}
else
{
    app.UseCors("ProdCorsPolicy");
}
app.UseSecurityHeaders();

app.UseSession();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCorsPolicy");
}
else
{
    app.UseCors("ProdCorsPolicy");
}

app.UseAuthentication();
app.UseAuthorization();

app.UseSpa(conf =>
{
    conf.Options.SourcePath = frontEndRelativePath;
});

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandler>();

app.Run();

