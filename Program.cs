var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//rota
string connStr = "Server = localhost; Database = bd_loja; User = root"; // Password = admin;
// connStr String de conexão

// Direcionar rota da view (páginas estáticas) e colocar ações do CRUD
app.UseStaticFiles(new StaticFileOptions{
    // (new StaticFileOptions) intância

});
app.Run();
