namespace ProjetoExample.Model
{
    public class Produto
    {
        //Atributo, geter e setter encapsulado dentro da propriedade. 
        //Propriedade com letra maiúscula e atributo minúsculo.
        //diferenças de declaração de atributo no js e c#
        //O que é atributo e propriedade
        public int Id { get; set; }

        public string Nome { get; set; }

        public double Preco { get; set; }  

        public string Categoria { get; set; }  
    }
}
