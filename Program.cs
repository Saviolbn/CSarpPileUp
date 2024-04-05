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
    public List<TreeNode<T>> Children { get; set; }

    public TreeNode(T data)
    {
        Data = data;
        Children = new List<TreeNode<T>>();
    }

    public void AddChild(T data)
    {
        if (Children.Count < 6) // Limiting to 6 children
        {
            TreeNode<T> child = new TreeNode<T>(data);
            Children.Add(child);
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
                if (torreX[i] != 0)
                {
                    topoIndex = i;
                    break;
                }
            }

            if (topoIndex == -1)
            {
                return Tuple.Create(false, torreXCopy, torreYCopy);
            }

            for (int i = 0; i < numDiscos - 1; i++)
            {
                if (i == 0)
                {
                    if (torreY[i] == 0)
                    {
                        torreY[i] = torreX[topoIndex];
                        torreX[topoIndex] = 0;
                    }
                }
                else
                {
                    if (torreY[i] == 0 && torreY[i - 1] > torreX[topoIndex])
                    {
                        torreY[i] = torreX[topoIndex];
                        torreX[topoIndex] = 0;
                    }
                }
            }

            return Tuple.Create(true, torreXCopy, torreYCopy);
        }

        void imprimirSistema(Jogo jogo)
        {
            Console.WriteLine($"A:{string.Join("", jogo.TorreA)} B:{string.Join("", jogo.TorreB)} C:{string.Join("", jogo.TorreC)} Custo:{jogo.Custo}");
        }

        void calcularMovimento(TreeNode<Jogo> node)
        {
            var resposta = moverTopo(node.Data.TorreA, node.Data.TorreB);
            if (resposta.Item1)
            {
                int[] otherArray = new int[numDiscos];
                Array.Copy(node.Data.TorreC, otherArray, numDiscos);
                
                Jogo novoJogo = new Jogo(
                    resposta.Item2,
                    resposta.Item3,
                    otherArray,
                    0
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo);
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
                    0
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo);
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
                    0
                );
                novoJogo.Custo = novoJogo.Custo + 1 + avaliarSistema(novoJogo);
                node.AddChild(novoJogo);
            }

        }


        Jogo jogo = new(new int[numDiscos], new int[numDiscos], new int[numDiscos], 0);
        inicializarTorre(jogo);

        TreeNode<Jogo> arvore = new TreeNode<Jogo>(jogo);

        calcularMovimento(arvore);

        imprimirSistema(arvore.Data);
        for (int i = 0; i < arvore.Children.Count - 1; i++)
        {
            imprimirSistema(arvore.Children[i].Data);
        }
    }
}