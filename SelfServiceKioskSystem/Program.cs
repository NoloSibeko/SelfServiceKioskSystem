using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelfServiceKioskSystem.Data;
using SelfServiceKioskSystem.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ... configs (controllers, CORS, Swagger)

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// --- CONTEXT ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- SEED SUPERUSER ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Users.Any(u => u.Email == "bsibeko@singular.co.za"))
    {
        var wallet = new Wallet { Balance = 0 };
        var superuser = new User
        {
            Name = "Bonolo",
            Surname = "Sibeko",
            Email = "bsibeko@singular.co.za",
            ContactNumber = "0620912838",
            AccountStatus = "Active",
            Password = BCrypt.Net.BCrypt.HashPassword("BonoloSibeko123#"),
            Wallet = wallet,
            RoleID = 2
        };

        context.Users.Add(superuser);
        context.SaveChanges();
    }
}

// --- MIDDLEWARE OUTSIDE SCOPE ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("ReactApp");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();

app.Run();
