using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();  // default for getting json response
builder.Services.AddControllers(opt =>
{

    // opt.InputFormatters  or //  opt.OutputFormatters here is the place for config these 

    //opt.ReturnHttpNotAcceptable = true; // return 406 status code not acceptable response
    // before this if had wanted xml we would have specific response and we would've  got json!
    // with this we accept just specific response otherwise we sent 406 response!

}).AddXmlDataContractSerializerFormatters();// config for getting xml response! and some useful configs!

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<FileExtensionContentTypeProvider>(); // for using content file! on file controller 

var app = builder.Build();


//Ordering doesn't matter!





//Configure the HTTP request pipeline.
//Ordering Matters!
//request pipeline

if (app.Environment.IsDevelopment())
{
    //swagger middleware
    app.UseSwagger();
    // swagger doc
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseRouting();

app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{

//    endpoints.MapControllers(); // empty means we have to set routes on actions! without specific routes
//});

app.MapControllers(); // route management



//app.Run(async (context) =>
//{

//    await context.Response.WriteAsync("Hi Api!");

//});





app.Run();
