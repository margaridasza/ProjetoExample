var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");
//rota
string connStr = "Server = localhost; Database = bd_loja; User = root"; // Password = admin;
// connStr String de conex�o

// Direcionar rota da view (p�ginas est�ticas) e colocar a��es do CRUD
app.UseStaticFiles(new StaticFileOptions{
    // (new StaticFileOptions) int�ncia

});
app.Run();
