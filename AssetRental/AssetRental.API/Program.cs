using AssetRental.Application.Extensions;
using AssetRental.Application.Middlewares;
using AssetRental.Infrastructure.Contexts;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.RegisterDependencies(builder.Configuration);
builder.Services.AddSwaggerGen();

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<IdentityUser>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;
    opt.SignIn.RequireConfirmedEmail = true;
})
        .AddDefaultUI()
        .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options => { options.Cookie.SameSite = SameSiteMode.None; });

var app = builder.Build();

app.UseAuthorization();
app.MapIdentityApi<IdentityUser>();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.MapControllers();

//app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager) =>
//{
//    await signInManager.SignOutAsync().ConfigureAwait(false);
//}).RequireAuthorization(); // So that only authorized users can use this endpoint

app.Run();
