=========================================================================
1. add to web site project 
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="6.0.3" />


=========================================================================
2. config.json

add in settings section

"Jwt": {
	"Key": "ThisIsMySecretKey",
	"Issuer": "webvella-erp",
	"Audience": "webvella-erp"
}


=========================================================================
3. startup
in ConfigureServices method add

services.AddAuthentication(auth => { auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = Configuration["Settings:Jwt:Issuer"],
            ValidAudience = Configuration["Settings:Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Settings:Jwt:Key"]))
        };
    });

 in Configure method add 
 
 app.UseJwtMiddleware();

 =========================================================================
 