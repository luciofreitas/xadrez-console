using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xadrez_console.tabuleiro;

namespace xadrez_console.xadrez
{
    internal class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        private int turno;
        private Cor jogadorAtual;
        public bool terminada { get; private set; }

        public PartidaDeXadrez()
        {
            tab = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQuantidadeMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);

        }
        private void colocarPecas()
        {
            tab.colocarPeca(new Torre(Cor.Preto, tab), new PosicaoXadrez('a', 8).toPosicao());
            tab.colocarPeca(new Torre(Cor.Preto, tab), new PosicaoXadrez('h', 8).toPosicao());
            tab.colocarPeca(new Cavalo(Cor.Preto, tab), new PosicaoXadrez('g', 8).toPosicao());
            tab.colocarPeca(new Cavalo(Cor.Preto, tab), new PosicaoXadrez('b', 8).toPosicao());
            tab.colocarPeca(new Bispo(Cor.Preto, tab), new PosicaoXadrez('c', 8).toPosicao());
            tab.colocarPeca(new Bispo(Cor.Preto, tab), new PosicaoXadrez('f', 8).toPosicao());
            tab.colocarPeca(new Rei(Cor.Preto, tab), new PosicaoXadrez('d', 8).toPosicao());
            tab.colocarPeca(new Dama(Cor.Preto, tab), new PosicaoXadrez('e', 8).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('a', 7).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('b', 7).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('c', 7).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('d', 7).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('e', 7).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('f', 7).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('g', 7).toPosicao());
            tab.colocarPeca(new Peao(Cor.Preto, tab), new PosicaoXadrez('h', 7).toPosicao());

            tab.colocarPeca(new Torre(Cor.Branco, tab), new PosicaoXadrez('a', 1).toPosicao());
            tab.colocarPeca(new Torre(Cor.Branco, tab), new PosicaoXadrez('h', 1).toPosicao());
            tab.colocarPeca(new Cavalo(Cor.Branco, tab), new PosicaoXadrez('g', 1).toPosicao());
            tab.colocarPeca(new Cavalo(Cor.Branco, tab), new PosicaoXadrez('b', 1).toPosicao());
            tab.colocarPeca(new Bispo(Cor.Branco, tab), new PosicaoXadrez('c', 1).toPosicao());
            tab.colocarPeca(new Bispo(Cor.Branco, tab), new PosicaoXadrez('f', 1).toPosicao());
            tab.colocarPeca(new Rei(Cor.Branco, tab), new PosicaoXadrez('d', 1).toPosicao());
            tab.colocarPeca(new Dama(Cor.Branco, tab), new PosicaoXadrez('e', 1).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('a', 2).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('b', 2).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('c', 2).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('d', 2).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('e', 2).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('f', 2).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('g', 2).toPosicao());
            tab.colocarPeca(new Peao(Cor.Branco, tab), new PosicaoXadrez('h', 2).toPosicao());
        }
    }
}
