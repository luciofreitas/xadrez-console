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
        public Peca vulneravelEnPassant { get; private set; }

        public PartidaDeXadrez()
        {
            tabuleiro = new Tabuleiro(8, 8);
            turno = 1;
            jogadorAtual = Cor.Branco;
            terminada = false;
            xeque = false;
            vulneravelEnPassant = null;
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

            //#jogadaespecial roque pequeno 
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca Torre = tabuleiro.retirarPeca(origemTorre);
                Torre.incrementarQuantidadeMovimentos();
                tabuleiro.colocarPeca(Torre, destinoTorre);
            }
            //#jogadaespecial roque grande 
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca Torre = tabuleiro.retirarPeca(origemTorre);
                Torre.incrementarQuantidadeMovimentos();
                tabuleiro.colocarPeca(Torre, destinoTorre);
            }
            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == null)
                {
                    Posicao posP;
                    if (p.cor == Cor.Branco)
                    {
                        posP = new Posicao(destino.linha + 1, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(destino.linha - 1, destino.coluna);
                    }
                    pecaCapturada = tabuleiro.retirarPeca(posP);
                    capturadas.Add(pecaCapturada);
                }
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

            //#jogadaespecial roque pequeno 
            if (p is Rei && destino.coluna == origem.coluna + 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna + 3);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna + 1);
                Peca Torre = tabuleiro.retirarPeca(destinoTorre);
                Torre.decrementarQuantidadeMovimentos();
                tabuleiro.colocarPeca(Torre, origemTorre);
            }
            //#jogadaespecial roque grande 
            if (p is Rei && destino.coluna == origem.coluna - 2)
            {
                Posicao origemTorre = new Posicao(origem.linha, origem.coluna - 4);
                Posicao destinoTorre = new Posicao(origem.linha, origem.coluna - 1);
                Peca Torre = tabuleiro.retirarPeca(destinoTorre);
                Torre.incrementarQuantidadeMovimentos();
                tabuleiro.colocarPeca(Torre, origemTorre);
            }
            // #jogadaespecial en passant
            if (p is Peao)
            {
                if (origem.coluna != destino.coluna && pecaCapturada == vulneravelEnPassant)
                {
                    Peca peao = tabuleiro.retirarPeca(destino);
                    Posicao posP;
                    if (p.cor == Cor.Branco)
                    {
                        posP = new Posicao(3, destino.coluna);
                    }
                    else
                    {
                        posP = new Posicao(4, destino.coluna);
                    }
                    tabuleiro.colocarPeca(peao, posP);
                }
            }
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = executaMovimento(origem, destino);
            if (estaEmXeque(jogadorAtual))
            {
                desfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque");
            }
            Peca peca = tabuleiro.peca(destino);
            // #jogadaespecial promocao
            if (peca is Peao)
            {
                if ((peca.cor == Cor.Branco && destino.linha == 0) || (peca.cor == Cor.Preto && destino.linha == 7))
                {
                    peca = tabuleiro.retirarPeca(destino);
                    pecas.Remove(peca);
                    Peca dama = new Dama(peca.cor, tabuleiro);
                    tabuleiro.colocarPeca(dama, destino);
                    pecas.Add(dama);
                }
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
            // #jogadaespecial en passant
            if (peca is Peao && (destino.linha == origem.linha - 2 || destino.linha == origem.linha + 2))
            {
                vulneravelEnPassant = peca;
            }
            else
            {
                vulneravelEnPassant = null;
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
            foreach (Peca x in pecas)
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
            colocarNovaPeca('d', 8, new Dama(Cor.Preto, tabuleiro));
            colocarNovaPeca('e', 8, new Rei(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('a', 7, new Peao(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('b', 7, new Peao(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('c', 7, new Peao(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('d', 7, new Peao(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('e', 7, new Peao(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('f', 7, new Peao(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('g', 7, new Peao(Cor.Preto, tabuleiro, this));
            colocarNovaPeca('h', 7, new Peao(Cor.Preto, tabuleiro, this));

            colocarNovaPeca('a', 1, new Torre(Cor.Branco, tabuleiro));
            colocarNovaPeca('h', 1, new Torre(Cor.Branco, tabuleiro));
            colocarNovaPeca('g', 1, new Cavalo(Cor.Branco, tabuleiro));
            colocarNovaPeca('b', 1, new Cavalo(Cor.Branco, tabuleiro));
            colocarNovaPeca('c', 1, new Bispo(Cor.Branco, tabuleiro));
            colocarNovaPeca('f', 1, new Bispo(Cor.Branco, tabuleiro));
            colocarNovaPeca('d', 1, new Dama(Cor.Branco, tabuleiro));
            colocarNovaPeca('e', 1, new Rei(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('a', 2, new Peao(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('b', 2, new Peao(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('c', 2, new Peao(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('d', 2, new Peao(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('e', 2, new Peao(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('f', 2, new Peao(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('g', 2, new Peao(Cor.Branco, tabuleiro, this));
            colocarNovaPeca('h', 2, new Peao(Cor.Branco, tabuleiro, this));

        }
    }
}
