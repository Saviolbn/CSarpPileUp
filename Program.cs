// Torre de Hanoi
// Há 3 torres A B C
// Há Y discos na Torre A de maneira ordenada (em baixo está o maior)
// Deve se mover todos os discos para a torre C mantendo a mesma ordem
// Um disco maior não pode ficar em cima de um menor, ou uma haste vazia
// Só pode mover um disco por vez

// Será usado o algoritmo de busca A*
// Regras:
// R1 mover A para B
// R2 mover A para C
// R3 mover B para A
// R4 mover B para C
// R5 mover C para A
// R6 mover C para B

public class TreeNode<T>
{
    public T Data { get; set; }
    public TreeNode<T>[] Children { get; set; }

    public TreeNode(T data)
    {
        Data = data;
        Children = new TreeNode<T>[6];
    }

    public void AddChild(T data, int i)
    {
        if (i < 6) // Limiting to 6 children
        {
            Children[i] = new TreeNode<T>(data);
        }
        else
        {
            throw new InvalidOperationException("Cannot add more than 6 children.");
        }
    }
}

class Jogo(int[] torreA, int[] torreB, int[] torreC, int custo)
{
    public int[] TorreA { get; set; } = torreA;
    public int[] TorreB { get; set; } = torreB;
    public int[] TorreC { get; set; } = torreC;
    public int Custo { get; set; } = custo;
}

class Program
{
    static void Main(string[] args)
    {
        //Console.Clear();
        int numDiscos = 5;

        void inicializarTorre(Jogo jogo)
        {
            for (int i = 0; i < numDiscos; i++)
            {
                jogo.TorreA[i] = numDiscos - i;
            }
        };

        // quanto menor o valor retornado, melhor estamos
        int avaliarSistema(Jogo jogo)
        {
            int h = 0;

            for (int i = 0; i < numDiscos; i++)
            {
                if (jogo.TorreC[i] != numDiscos - i)
                {
                    h++;
                }
            }

            return h;
        }



        // move do topo da torreX para o fundo da torre Y
        Tuple<bool, int[], int[]> moverTopo(int[] torreX, int[] torreY)
        {
            int[] torreXCopy = new int[numDiscos];
            int[] torreYCopy = new int[numDiscos];

            Array.Copy(torreX, torreXCopy, numDiscos);
            Array.Copy(torreY, torreYCopy, numDiscos);

            int topoIndex = -1;
            for (int i = numDiscos - 1; i >= 0; i--)
            {
                if (torreXCopy[i] != 0)
                {
                    topoIndex = i;
                    break;
                }
            }

            if (topoIndex == -1)
            {
                return Tuple.Create(false, torreXCopy, torreXCopy);
            }

            bool isTrocou = false;
            for (int i = 0; i < numDiscos; i++)
            {
                if (i == 0)
                {
                    if (torreYCopy[i] == 0)
                    {
                        torreYCopy[i] = torreXCopy[topoIndex];
                        torreXCopy[topoIndex] = 0;
                        isTrocou = true;
                    }
                }
                else
                {
                    if (torreYCopy[i] == 0 && torreYCopy[i - 1] > torreXCopy[topoIndex])
                    {
                        torreYCopy[i] = torreXCopy[topoIndex];
                        torreXCopy[topoIndex] = 0;
                        isTrocou = true;
                    }
                }
            }
            if (!isTrocou)
            {
                return Tuple.Create(false, torreXCopy, torreYCopy);
            }

            return Tuple.Create(true, torreXCopy, torreYCopy);
        }

        void imprimirSistema(Jogo jogo)
        {
            Console.WriteLine($"A:{string.Join("", jogo.TorreA)} B:{string.Join("", jogo.TorreB)} C:{string.Join("", jogo.TorreC)} Custo:{jogo.Custo}");
        }

        void calcularMovimento(TreeNode<Jogo> node, int profundidade)
        {
            Tuple<bool, int[], int[]> resposta = Tuple.Create(false, new int[0], new int[0]);

            resposta = moverTopo(node.Data.TorreA, node.Data.TorreB);
            if (resposta.Item1)
            {
                int[] otherArray = new int[numDiscos];
                Array.Copy(node.Data.TorreC, otherArray, numDiscos);

                Jogo novoJogo = new Jogo(
                    resposta.Item2,
                    resposta.Item3,
                    otherArray,
                    profundidade
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo, 0);
            }

            resposta = moverTopo(node.Data.TorreA, node.Data.TorreC);
            if (resposta.Item1)
            {
                int[] otherArray = new int[numDiscos];
                Array.Copy(node.Data.TorreB, otherArray, numDiscos);

                Jogo novoJogo = new Jogo(
                    resposta.Item2,
                    otherArray,
                    resposta.Item3,
                    profundidade
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo, 1);
            }

            resposta = moverTopo(node.Data.TorreB, node.Data.TorreA);
            if (resposta.Item1)
            {
                int[] otherArray = new int[numDiscos];
                Array.Copy(node.Data.TorreC, otherArray, numDiscos);

                Jogo novoJogo = new Jogo(
                    resposta.Item3,
                    resposta.Item2,
                    otherArray,
                    profundidade
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo, 2);
            }

            resposta = moverTopo(node.Data.TorreB, node.Data.TorreC);
            if (resposta.Item1)
            {
                int[] otherArray = new int[numDiscos];
                Array.Copy(node.Data.TorreA, otherArray, numDiscos);

                Jogo novoJogo = new Jogo(
                    otherArray,
                    resposta.Item2,
                    resposta.Item3,
                    profundidade
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo, 3);
            }

            resposta = moverTopo(node.Data.TorreC, node.Data.TorreA);
            if (resposta.Item1)
            {
                int[] otherArray = new int[numDiscos];
                Array.Copy(node.Data.TorreA, otherArray, numDiscos);

                Jogo novoJogo = new Jogo(
                    otherArray,
                    resposta.Item2,
                    resposta.Item3,
                    profundidade
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo, 4);
            }

            resposta = moverTopo(node.Data.TorreC, node.Data.TorreB);
            if (resposta.Item1)
            {
                int[] otherArray = new int[numDiscos];
                Array.Copy(node.Data.TorreA, otherArray, numDiscos);

                Jogo novoJogo = new Jogo(
                    otherArray,
                    resposta.Item2,
                    resposta.Item3,
                    profundidade
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo, 5);
            }

            // calcularMovimento()
        }


        Jogo jogo = new(new int[numDiscos], new int[numDiscos], new int[numDiscos], 0);
        TreeNode<Jogo> arvore = new TreeNode<Jogo>(jogo);
        inicializarTorre(jogo);

        Console.WriteLine();
        calcularMovimento(arvore, 0);
        imprimirSistema(arvore.Data);
        Console.WriteLine();
        for (int i = 0; i < arvore.Children.Length; i++)
        {
            if (arvore.Children[i] != null)
            {
                imprimirSistema(arvore.Children[i].Data);
            }
        }

        Console.WriteLine();
        calcularMovimento(arvore.Children[0], 1);
        imprimirSistema(arvore.Children[0].Data);
        Console.WriteLine();
        for (int i = 0; i < arvore.Children[0].Children.Length; i++)
        {
            if (arvore.Children[0].Children[i] != null)
            {
                imprimirSistema(arvore.Children[0].Children[i].Data);
            }
        }
    }
}