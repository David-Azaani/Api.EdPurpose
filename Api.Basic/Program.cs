var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


//Ordering doesn't matter!

// Configure the HTTP request pipeline.
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

//    endpoints.MapControllers(); // empty means we have to set routes on actions!
//});

app.MapControllers(); // route management



//app.Run(async (context) =>
//{

//    await context.Response.WriteAsync("Hi Api!");

//});





app.Run();
