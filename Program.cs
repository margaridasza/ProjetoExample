using Microsoft.Extensions.FileProviders;// Qual diretório ele vai direcionar (pasta view)
using MySql.Data.MySqlClient;
using ProjetoExample.Model;//usa a var no aspnet recebendo o valor

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();// criação e configuração do app, modelo minimalista do aspnet core
//app.MapGet("/", () => "Hello World!");
//rota
string connStr = "Server = localhost; Database = bd_loja; User = root"; // Password = admin;
// connStr String de conexão

// Direcionar rota da view (páginas estáticas) e colocar ações do CRUD
app.UseStaticFiles(new StaticFileOptions{
    // (new StaticFileOptions) intância
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Views")),//dois parâmetros e um método
    RequestPath = ""
});

// direcionar pro index
app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapGet("/produtos",()=>{ //referenciar para qual objeto a gente vai usar, rota get, lista que ele vai retornar
    //conectar com o banco de dados através de uma string de conexão
    var produtos = new List<Produto>();// lista é parecida com vetor
    using var conn = new MySqlConnection(connStr);//control espaço
    conn.Open();// abre conexão do BD
    var cmd = new MySqlCommand("SELECT * FROM tb_produtos", conn);// comando sql
    var reader = cmd.ExecuteReader();// executa o comando
    while(reader.Read()){//enquanto tiver dados ele vai ler
        produtos.Add(new Produto
        {
            Id = reader.GetInt32("id"),//pega o id do banco de dados
            Nome = reader.GetString("nome"),//pega o nome do banco de dados
            Preco = reader.GetDouble("preco"),//pega o preço do banco de dados
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
    cmd.ExecuteNonQuery();//Quando faz read retorna colunas com valor (reader) com parâmetro, neste caso não tem retorno (update, delete, create)
    conn.Close();
    return Results.Ok();
});

app.Run();
