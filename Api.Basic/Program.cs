using System.Reflection;
using System.Text;
using Api.Basic;
using Api.Basic.DbContexts;
using Api.Basic.Services;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
#region SeriLog

//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .WriteTo.Console()
//    .WriteTo.File("logs/cityinfo.txt", rollingInterval: RollingInterval.Day)
//    .CreateLogger(); 
#endregion

// before builder
var builder = WebApplication.CreateBuilder(args);

#region Serilog
//builder.Host.UseSerilog(); 
#endregion



#region Loge
// builder.Logging. access to log Config 
//builder.Logging.ClearProviders(); // Clearing Log compeletely
//builder.Logging.AddConsole(); // returning back! 
#endregion




// Add services to the container.

//builder.Services.AddControllers();  // default for getting json response
builder.Services.AddControllers(opt =>
{

    #region Note
    // opt.InputFormatters  or //  opt.OutputFormatters here is the place for config these 

    //opt.ReturnHttpNotAcceptable = true; // return 406 status code not acceptable response

    // before this if we had wanted xml we would have specific response and we would've  got json!
    // with this we accept just specific response otherwise we sent 406 response! 
    #endregion


}).AddNewtonsoftJson() // for using patch!
    .AddXmlDataContractSerializerFormatters();// config for getting xml response! and some useful configs!

builder.Services.AddEndpointsApiExplorer();
#region Note13
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; //Api.Basic.xml
    var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

    setupAction.IncludeXmlComments(xmlCommentsFullPath);
    #region Documenting for Authentication

    setupAction.AddSecurityDefinition("Api.BasicBearerAuth", new OpenApiSecurityScheme()
    {
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        Description = "Input a valid token to access this API put This Way Bearer{ }Token"
    });
    #endregion

    setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Api.BasicBearerAuth" }
            }, new List<string>() }
});
});
#endregion

builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); // for using content file! on file controller 

#region MailService
//builder.Services.AddTransient<IMailService, LocalMailService>();


#if DEBUG
builder.Services.AddTransient<IMailService, LocalMailService>(); //just this if we dont want the next!! ;-)
                                                                 // dont forget to change debug to release mode!
#else
builder.Services.AddTransient<IMailService, CloudMailService>();
#endif
#endregion
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});


#region Note6
builder.Services.AddSingleton<CitiesDataStore>();

#endregion


#region Note8

builder.Services.AddDbContext<CityInfoContext>(option =>
        option.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoDBConnectionString"]));


#endregion




#region Note9
builder.Services.AddScoped<ICityInfoRepository, CityInfoRepository>();
#endregion
#region Note10

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

#endregion
#region Note12
builder.Services.AddAuthentication("Bearer")
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
    };
}
);



builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MustIran", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("city", "Antwerp");
    });
});
#endregion



#region Note13: ApiVersioning
builder.Services.AddApiVersioning(setupAction =>
{
    setupAction.AssumeDefaultVersionWhenUnspecified = true;
    setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    setupAction.ReportApiVersions = true;
});

#endregion


var app = builder.Build();


#region Note
//Ordering doesn't matter!





//Configure the HTTP request pipeline.
//Ordering Matters!
//request pipeline

#endregion
if (app.Environment.IsDevelopment())
{
    //swagger middleware
    app.UseSwagger();
    // swagger doc
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseRouting();

#region Note12
app.UseAuthentication(); //first

app.UseAuthorization();
#endregion

#region Note
//app.UseEndpoints(endpoints =>
//{

//    endpoints.MapControllers(); // empty means we have to set routes on actions! without specific routes
//}); 
#endregion

app.MapControllers(); // route management



#region Note
//app.Run(async (context) =>
//{

//    await context.Response.WriteAsync("Hi Api!");

//});


#endregion



app.Run();
