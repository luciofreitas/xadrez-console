using System;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;
namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Tabuleiro tab = new Tabuleiro(8, 8);

                tab.colocarPeca(new Torre(Cor.Preto, tab), new Posicao(0, 0));
                tab.colocarPeca(new Torre(Cor.Preto, tab), new Posicao(1, 3));
                tab.colocarPeca(new Rei(Cor.Preto, tab), new Posicao(0, 2));

                tab.colocarPeca(new Torre(Cor.Branco, tab), new Posicao(3, 5));
                tab.colocarPeca(new Torre(Cor.Branco, tab), new Posicao(4, 6));
                tab.colocarPeca(new Rei(Cor.Branco, tab), new Posicao(5, 7));



                Tela.imprimirTabuleiro(tab);
            }
            catch (TabuleiroException ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();

        }
    }
}