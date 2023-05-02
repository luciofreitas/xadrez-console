using System;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;
namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            Tabuleiro tab = new Tabuleiro(8, 8);
            tab.colocarPeca(new Torre(Cor.Preto,tab), new Posicao(0,0));
            tab.colocarPeca(new Torre(Cor.Preto,tab), new Posicao(1,3));
            tab.colocarPeca(new Rei(Cor.Preto,tab), new Posicao(2,4));

            Tela.imprimirTabuleiro(tab);

            Console.ReadLine();
        }
    }
}