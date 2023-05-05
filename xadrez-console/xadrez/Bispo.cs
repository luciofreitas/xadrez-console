using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xadrez_console.tabuleiro;

namespace xadrez_console.xadrez
{
    internal class Bispo : Peca
    {
        public Bispo(Cor cor, Tabuleiro tabuleiro) : base(cor, tabuleiro)
        {

        }
        public override string ToString()
        {
            return "B";
        }
        private bool podeMover(Posicao pos)
        {
            Peca peca = tabuleiro.peca(pos);
            return peca == null || peca.cor != cor;
        }
        public override bool[,] movimentosPossiveis()
        {

            bool[,] mat = new bool[tabuleiro.linhas, tabuleiro.colunas];

            Posicao pos = new Posicao(0, 0);

            //diagonalCimaEsquerda
            pos.definirValores(posicao.linha - 1, posicao.coluna - 1);

            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.linha - 1, pos.coluna - 1);
            }
            //diagonalCimaDireita
            pos.definirValores(posicao.linha - 1, posicao.coluna + 1);

            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.linha - 1, pos.coluna + 1);
            }
            //diagonalBaixoDireita
            pos.definirValores(posicao.linha + 1, posicao.coluna + 1);

            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.linha + 1, pos.coluna + 1);
            }
            //diagonalBaixoEsquerda
            pos.definirValores(posicao.linha + 1, posicao.coluna - 1);

            while (tabuleiro.posicaoValida(pos) && podeMover(pos))
            {
                mat[pos.linha, pos.coluna] = true;
                if (tabuleiro.peca(pos) != null && tabuleiro.peca(pos).cor != cor)
                {
                    break;
                }
                pos.definirValores(pos.linha + 1, pos.coluna - 1);
            }
            return mat;
        }
    }
}
