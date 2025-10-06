using Microsoft.Extensions.FileProviders;// Qual diretório ele vai direcionar (pasta view)
using MySql.Data.MySqlClient;//conexão com o banco de dados
using ProjetoExample.Model;//usa a var no aspnet recebendo o valor

var builder = WebApplication.CreateBuilder(args);//criação e configuração do builder, modelo minimalista do aspnet core
var app = builder.Build();//criação do app, configuração do app
//app.MapGet("/", () => "Hello World!");
//rota
string connStr = "Server = localhost; Database = bd_loja; User = root"; // Password = admin;
// connStr String de conexão

// Direcionar rota da view (páginas estáticas) e colocar ações do CRUD
app.UseStaticFiles(new StaticFileOptions
{ //método UseStaticFiles
    // (new StaticFileOptions) intância
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Views")),//dois parâmetros e um método
   // PhysicalFileProvider: qual diretório ele vai direcionar (pasta view)
    RequestPath = ""//caminho raiz, vazio pq é a raiz, se fosse /app seria app
});

// direcionar pro index
app.MapGet("/", () => Results.Redirect("/index.html"));

app.MapGet("/produtos",()=>{ //referenciar para qual objeto a gente vai usar, rota get, lista que ele vai retornar
    //conectar com o banco de dados através de uma string de conexão
    var produtos = new List<Produto>();// lista é parecida com vetor
    using var conn = new MySqlConnection(connStr);//control espaço???
    conn.Open();// abre conexão do BD
    var cmd = new MySqlCommand("SELECT * FROM tb_produtos", conn);// comando sql
    var reader = cmd.ExecuteReader();// executa o comando
    while(reader.Read()){//enquanto tiver dados ele vai ler
        produtos.Add(new Produto
        {
            Id = reader.GetInt32("id"),//pega o id do banco de dados, GetInt32 pq é inteiro
            Nome = reader.GetString("nome"),//pega o nome do banco de dados
            Preco = reader.GetDouble("preco"),//pega o preço do banco de dados
            Categoria = reader.GetString("categoria")//pega a categoria do banco de dados

        });//adiciona o produto na lista
    }
    return produtos;//retorna a lista de produtos, que vai ser convertida em json, pq o aspnet core faz isso automaticamente
});

app.MapDelete("/produtos/{id}", (int id) => //na hora que clicar no botão excluir, ele vai pegar o id do produto
{
    using var conn = new MySqlConnection(connStr);//instancia
    conn.Open();
    var cmd = new MySqlCommand("DELETE FROM tb_produtos WHERE id = @id", conn);// comando sql, @id é um parâmetro, pq o id é variável e não um valor fixo
    cmd.Parameters.AddWithValue("@id", id);//adiciona o valor do id no parâmetro @id
    cmd.ExecuteNonQuery();//Quando faz read retorna colunas com valor (reader) com parâmetro, neste caso não tem retorno (update, delete, create)
    conn.Close();
    return Results.Ok();
});

app.MapPost("/produtos", async (//INSERÇÃO - pega os dados do formulário, que são enviados via post
    HttpRequest req) =>//req é o objeto que representa a requisição http
{
    var form = await req.ReadFormAsync();
    string nome = form["nome"];
    decimal preco = decimal.Parse(form["preco"]);
    string categoria = form["categoria"];
    //Pega os dados do form para (depois) preparar os dados do formulário para a string de insert
    using var conn = new MySqlConnection(connStr);//instancia
    conn.Open();
    var cmd = new MySqlCommand("INSERT INTO tb_produtos (nome, preco, categoria)" +
        "values (@nome, @preco, @categoria)", conn);
    cmd.Parameters.AddWithValue("@nome", nome);
    cmd.Parameters.AddWithValue("@preco", preco);  
    cmd.Parameters.AddWithValue("@categoria", categoria);  
    cmd.ExecuteNonQuery();
    conn.Close();  
    return Results.Redirect("/index.html");//redireciona para a página inicial
});

app.MapPost("/produtos/{id}", async (int id,// ATUALIZAÇÃO - na hora que clicar no botão editar, ele vai pegar o id do produto
    HttpRequest req) =>
{
    var form = await req.ReadFormAsync();
    string nome = form["nome"];
    decimal preco = decimal.Parse(form["preco"]);
    string categoria = form["categoria"];
    //Pega os dados do form para (depois) preparar os dados do formulário para a string de insert
    using var conn = new MySqlConnection(connStr);//Instancia
    conn.Open();
    var cmd = new MySqlCommand(
        "UPDATE td_produtos SET nome=@nome, preco=@preco,"+
        "categoria=@categoria WHERE id=@id", conn);
    cmd.Parameters.AddWithValue("@nome", nome);
    cmd.Parameters.AddWithValue("@preco", preco);
    cmd.Parameters.AddWithValue("@categoria", categoria);
    cmd.Parameters.AddWithValue("@id", id);
    cmd.ExecuteNonQuery();//Quando faz read retorna colunas com valor (reader) com parâmetro, neste caso não tem retorno (update, delete, create)
    conn.Close();
    return Results.Redirect("/index.html");
});

app.Run();
