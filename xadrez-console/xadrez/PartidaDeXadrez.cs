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
        public Tabuleiro tabuleiro { get; private set; }
        public int turno { get; private set; }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }

        public PartidaDeXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            terminada = false;
            colocarPecas();
        }

        public void executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.retirarPeca(origem);
            p.incrementarQuantidadeMovimentos();
            Peca pecaCapturada = tabuleiro.retirarPeca(destino);
            tabuleiro.colocarPeca(p, destino);

        }
        public void realizaJogada(Posicao origem, Posicao destino)
        {
            executaMovimento(origem, destino);
            turno++;
            mudaJogador();

        }
        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tabuleiro.peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida! ");
            }
            if (jogadorAtual != tabuleiro.peca(pos).cor)
            {
                if (jogadorAtual == Cor.Preto)
                {
                    throw new TabuleiroException("Está na vez das pretas ");
                }
                else
                {
                    throw new TabuleiroException("Está na vez das brancas ");
                }
            }
            if (!tabuleiro.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida! ");
            }
        }
        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tabuleiro.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida");
            }
        }
        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Branco)
            {
                jogadorAtual = Cor.Preto;
            }
            else
            {
                jogadorAtual = Cor.Branco;
            }
        }
        private void colocarPecas()
        {
            tabuleiro.colocarPeca(new Torre(Cor.Preto, tabuleiro), new PosicaoXadrez('a', 8).toPosicao());
            tabuleiro.colocarPeca(new Torre(Cor.Preto, tabuleiro), new PosicaoXadrez('h', 8).toPosicao());
            tabuleiro.colocarPeca(new Cavalo(Cor.Preto, tabuleiro), new PosicaoXadrez('g', 8).toPosicao());
            tabuleiro.colocarPeca(new Cavalo(Cor.Preto, tabuleiro), new PosicaoXadrez('b', 8).toPosicao());
            tabuleiro.colocarPeca(new Bispo(Cor.Preto, tabuleiro), new PosicaoXadrez('c', 8).toPosicao());
            tabuleiro.colocarPeca(new Bispo(Cor.Preto, tabuleiro), new PosicaoXadrez('f', 8).toPosicao());
            tabuleiro.colocarPeca(new Rei(Cor.Preto, tabuleiro), new PosicaoXadrez('d', 8).toPosicao());
            tabuleiro.colocarPeca(new Dama(Cor.Preto, tabuleiro), new PosicaoXadrez('e', 8).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('a', 7).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('b', 7).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('c', 7).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('d', 7).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('e', 7).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('f', 7).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('g', 7).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Preto, tabuleiro), new PosicaoXadrez('h', 7).toPosicao());

            tabuleiro.colocarPeca(new Torre(Cor.Branco, tabuleiro), new PosicaoXadrez('a', 1).toPosicao());
            tabuleiro.colocarPeca(new Torre(Cor.Branco, tabuleiro), new PosicaoXadrez('h', 1).toPosicao());
            tabuleiro.colocarPeca(new Cavalo(Cor.Branco, tabuleiro), new PosicaoXadrez('g', 1).toPosicao());
            tabuleiro.colocarPeca(new Cavalo(Cor.Branco, tabuleiro), new PosicaoXadrez('b', 1).toPosicao());
            tabuleiro.colocarPeca(new Bispo(Cor.Branco, tabuleiro), new PosicaoXadrez('c', 1).toPosicao());
            tabuleiro.colocarPeca(new Bispo(Cor.Branco, tabuleiro), new PosicaoXadrez('f', 1).toPosicao());
            tabuleiro.colocarPeca(new Rei(Cor.Branco, tabuleiro), new PosicaoXadrez('d', 1).toPosicao());
            tabuleiro.colocarPeca(new Dama(Cor.Branco, tabuleiro), new PosicaoXadrez('e', 1).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('a', 2).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('b', 2).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('c', 2).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('d', 2).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('e', 2).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('f', 2).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('g', 2).toPosicao());
            tabuleiro.colocarPeca(new Peao(Cor.Branco, tabuleiro), new PosicaoXadrez('h', 2).toPosicao());
        }
    }
}
