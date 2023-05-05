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
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;
        public bool xeque { get; private set; }

        public PartidaDeXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            terminada = false;
            xeque = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public Peca executaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tabuleiro.retirarPeca(origem);
            p.incrementarQuantidadeMovimentos();
            Peca pecaCapturada = tabuleiro.retirarPeca(destino);
            tabuleiro.colocarPeca(p, destino);

            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void desfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = tabuleiro.retirarPeca(destino);
            p.decrementarQuantidadeMovimentos();
            if (pecaCapturada != null)
            {
                tabuleiro.colocarPeca(pecaCapturada, destino);
                capturadas.Remove(pecaCapturada);
            }
            tabuleiro.colocarPeca(p, origem);
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);
            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque");
            }

            if (estaEmXeque(adversaria(jogadorAtual)))
            {
                xeque = true;
            }
            else
            {
                xeque = false;
            }
            if (testeXequemate(adversaria(jogadorAtual)))
            {
                terminada = true;
            }
            else
            {
                turno++;
                mudaJogador();
            }
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
            if (!tabuleiro.peca(origem).movimentoPossivel(destino))
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

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }
        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }
        private Cor adversaria(Cor cor)
        {
            if (cor == Cor.Branco)
            {
                return Cor.Preto;
            }
            else
            {
                return Cor.Branco;
            }
        }

        private Peca rei(Cor cor)
        {
            foreach (Peca x in pecasEmJogo(cor))
            {
                if (x is Rei)
                {
                    return x;
                }
            }
            return null;
        }
        public bool estaEmXeque(Cor cor)
        {
            Peca R = rei(cor);
            if (R == null)
            {
                throw new TabuleiroException($"Não tem rei da cor {cor} no tabuleiro!");
            }
            foreach (Peca x in pecasEmJogo(adversaria(cor)))
            {
                bool[,] mat = x.movimentosPossiveis();
                if (mat[R.posicao.linha, R.posicao.coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool testeXequemate(Cor cor)
        {
            if (!estaEmXeque(cor))
            {
                return false;
            }
            foreach (Peca x in pecasEmJogo(cor))
            {
                bool[,] mat = x.movimentosPossiveis();
                for (int i = 0; i < tabuleiro.linhas; i++)
                {
                    for (int j = 0; j < tabuleiro.colunas; j++)
                    {
                        if (mat[i, j])
                        {
                            Posicao origem = x.posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = executaMovimento(origem, destino);
                            bool testeXeque = estaEmXeque(cor);
                            desfazMovimento(origem, destino, pecaCapturada);
                            if (!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tabuleiro.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }
        private void colocarPecas()
        {
            colocarNovaPeca('a', 8, new Torre(Cor.Preto, tabuleiro));
            colocarNovaPeca('h', 8, new Torre(Cor.Preto, tabuleiro));
            colocarNovaPeca('g', 8, new Cavalo(Cor.Preto, tabuleiro));
            colocarNovaPeca('b', 8, new Cavalo(Cor.Preto, tabuleiro));
            colocarNovaPeca('c', 8, new Bispo(Cor.Preto, tabuleiro));
            colocarNovaPeca('f', 8, new Bispo(Cor.Preto, tabuleiro));
            colocarNovaPeca('d', 8, new Rei(Cor.Preto, tabuleiro));
            colocarNovaPeca('e', 8, new Dama(Cor.Preto, tabuleiro));
            colocarNovaPeca('a', 7, new Peao(Cor.Preto, tabuleiro));
            colocarNovaPeca('b', 7, new Peao(Cor.Preto, tabuleiro));
            colocarNovaPeca('c', 7, new Peao(Cor.Preto, tabuleiro));
            colocarNovaPeca('d', 7, new Peao(Cor.Preto, tabuleiro));
            colocarNovaPeca('e', 7, new Peao(Cor.Preto, tabuleiro));
            colocarNovaPeca('f', 7, new Peao(Cor.Preto, tabuleiro));
            colocarNovaPeca('g', 7, new Peao(Cor.Preto, tabuleiro));
            colocarNovaPeca('h', 7, new Peao(Cor.Preto, tabuleiro));

            colocarNovaPeca('a', 1, new Torre(Cor.Branco, tabuleiro));
            colocarNovaPeca('h', 1, new Torre(Cor.Branco, tabuleiro));
            colocarNovaPeca('g', 1, new Cavalo(Cor.Branco, tabuleiro));
            colocarNovaPeca('b', 1, new Cavalo(Cor.Branco, tabuleiro));
            colocarNovaPeca('c', 1, new Bispo(Cor.Branco, tabuleiro));
            colocarNovaPeca('f', 1, new Bispo(Cor.Branco, tabuleiro));
            colocarNovaPeca('d', 1, new Rei(Cor.Branco, tabuleiro));
            colocarNovaPeca('e', 1, new Dama(Cor.Branco, tabuleiro));
            colocarNovaPeca('a', 2, new Peao(Cor.Branco, tabuleiro));
            colocarNovaPeca('b', 2, new Peao(Cor.Branco, tabuleiro));
            colocarNovaPeca('c', 2, new Peao(Cor.Branco, tabuleiro));
            colocarNovaPeca('d', 2, new Peao(Cor.Branco, tabuleiro));
            colocarNovaPeca('e', 2, new Peao(Cor.Branco, tabuleiro));
            colocarNovaPeca('f', 2, new Peao(Cor.Branco, tabuleiro));
            colocarNovaPeca('g', 2, new Peao(Cor.Branco, tabuleiro));
            colocarNovaPeca('h', 2, new Peao(Cor.Branco, tabuleiro));

        }
    }
}
