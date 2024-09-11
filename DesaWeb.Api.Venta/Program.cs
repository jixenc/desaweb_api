using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Configuración de JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = "https://myapp.com",
            //ValidAudience = "https://myapp.com/audience",
            //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("dGhpc2lzYW5leGFtcGxlb2Zhc2VjdXJlYXV0aGVudGljYXRpb25rZXk="))
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("t3mpUs1apiW3b3dt@n0V=?"))
        };
    });


// Añadir autenticación al pipeline
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
