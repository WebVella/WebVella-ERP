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
in ConfigureServices method change auth to be 

 services.AddAuthentication(options =>
{
    options.DefaultScheme = "JWT_OR_COOKIE";
    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = "erp_auth_base";
    options.LoginPath = new PathString("/login");
    options.LogoutPath = new PathString("/logout");
    options.AccessDeniedPath = new PathString("/error?access_denied");
    options.ReturnUrlParameter = "returnUrl";
})
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
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
    })
    .AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
    {
        options.ForwardDefaultSelector = context =>
        {
            string authorization = context.Request.Headers[HeaderNames.Authorization];
            if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
                return JwtBearerDefaults.AuthenticationScheme;

            return CookieAuthenticationDefaults.AuthenticationScheme;
        };
    });

 in Configure method add 
 
 app.UseJwtMiddleware();

 =========================================================================
 