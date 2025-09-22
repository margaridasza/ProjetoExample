using Microsoft.Extensions.FileProviders;// Qual diret�rio ele vai direcionar (pasta view)
using MySql.Data.MySqlClient;
using ProjetoExample.Model;//usa a var no aspnet recebendo o valor

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();// cria��o e configura��o do app, modelo minimalista do aspnet core
//app.MapGet("/", () => "Hello World!");
//rota
string connStr = "Server = localhost; Database = bd_loja; User = root"; // Password = admin;
// connStr String de conex�o

// Direcionar rota da view (p�ginas est�ticas) e colocar a��es do CRUD
app.UseStaticFiles(new StaticFileOptions{
    // (new StaticFileOptions) int�ncia
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Views")),//dois par�metros e um m�todo
    RequestPath = ""
});

// direcionar pro index
app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapGet("/produtos",()=>{ //referenciar para qual objeto a gente vai usar, rota get, lista que ele vai retornar
    //conectar com o banco de dados atrav�s de uma string de conex�o
    var produtos = new List<Produto>();// lista � parecida com vetor
    using var conn = new MySqlConnection(connStr);//control espa�o
    conn.Open();// abre conex�o do BD
    var cmd = new MySqlCommand("SELECT * FROM tb_produtos", conn);// comando sql
    var reader = cmd.ExecuteReader();// executa o comando
    while(reader.Read()){//enquanto tiver dados ele vai ler
        produtos.Add(new Produto
        {
            Id = reader.GetInt32("id"),//pega o id do banco de dados
            Nome = reader.GetString("nome"),//pega o nome do banco de dados
            Preco = reader.GetDouble("preco"),//pega o pre�o do banco de dados
            Categoria = reader.GetString("categoria")//pega a categoria do banco de dados

        });//adiciona o produto na lista
    }
    return produtos;
    });

app.MapDelete("/produtos/{id}", (int id) => 
{
    using var conn = new MySqlConnection(connStr);
    conn.Open();
    var cmd = new MySqlCommand("DELETE FROM tb_produtos WHERE id = @id", conn);
    cmd.Parameters.AddWithValue("@id", id);
    cmd.ExecuteNonQuery();//Quando faz read retorna colunas com valor (reader) com par�metro, neste caso n�o tem retorno (update, delete, create)
    conn.Close();
    return Results.Ok();
});

app.Run();
