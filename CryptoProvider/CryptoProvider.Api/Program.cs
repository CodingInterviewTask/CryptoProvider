using CryptoProvider.Api.Services;
using CryptoProvider.Api.WebSocket;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://localhost:4200")
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = false,
            RequireAudience = false,
            ValidateAudience = false,
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            RequireSignedTokens = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
            {
                var publicKey = GetPublicKey();
                return new[] { publicKey };
            },
        };
    });

builder.Services.AddSignalR();

builder.Services.AddHostedService<CryptoHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.MapHub<CryptoHub>($"/{nameof(CryptoHub)}");

app.Run();

RsaSecurityKey GetPublicKey()
{
    HttpClient httpClient = new HttpClient();
    using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/api/Auth/GetPublicKey");
    var response = httpClient.Send(request);
    var publicKeyString = response.Content.ReadAsStringAsync().Result;

    var rsa = RSA.Create();
    rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKeyString!), out _);
    var publicKey = new RsaSecurityKey(rsa);

    return publicKey;
}