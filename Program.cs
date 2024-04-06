using System.Collections;
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



public class Jogo(int[] torreA, int[] torreB, int[] torreC, int custo)
{
    public int[] TorreA { get; set; } = torreA;
    public int[] TorreB { get; set; } = torreB;
    public int[] TorreC { get; set; } = torreC;
    public int Custo { get; set; } = custo;

    public int lastMove { get; set; } = -1;

    public bool isSolucao { get; set; } = false;
}


public class TreeNode
{
    public Jogo Jogo { get; set; }
    public TreeNode[] Children { get; set; }



    public TreeNode(Jogo jogo)
    {
        Jogo = jogo;
        Children = new TreeNode[6];
    }

    public void AddChild(Jogo jogo, int i, Hashtable nodesVisitados)
    {
        if (jogo == null)
        {
            return;
        }

        if (i < 6) // Limiting to 6 children
        {
            // checa se ja ta na arvore
            //if(nãoduplicado)
            {
                int hash = 0;
                for (int j = 0; j < jogo.TorreA.Length; j++)
                {
                    hash += (jogo.TorreA[j] + j) * 1 + (jogo.TorreB[j] + j) * 10 + (jogo.TorreC[j] + j) * 100; // Example: calculate a simple hash based on TorreA values
                }

                if (nodesVisitados.ContainsKey(hash))
                {
                    return;
                }

                nodesVisitados.Add(hash, null);
                Children[i] = new TreeNode(jogo);
            }
        }
        else
        {
            throw new InvalidOperationException("Cannot add more than 6 children.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        //Console.Clear();
        int numDiscos = 5;
        Hashtable nodesVisitados = new Hashtable();

        //

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

        void calcularMovimento(TreeNode node, int profundidade, int lastMove)
        {
            Tuple<bool, int[], int[]> resposta = Tuple.Create(false, new int[0], new int[0]);

                resposta = moverTopo(node.Jogo.TorreA, node.Jogo.TorreB);
                if (resposta.Item1)
                {
                    int[] otherArray = new int[numDiscos];
                    Array.Copy(node.Jogo.TorreC, otherArray, numDiscos);

                    Jogo novoJogo = new Jogo(
                        resposta.Item2,
                        resposta.Item3,
                        otherArray,
                        profundidade
                    );
                    novoJogo.Custo = novoJogo.Custo + 1;

                    int heuristica = avaliarSistema(novoJogo);
                    novoJogo.isSolucao = heuristica == 0;
                    novoJogo.lastMove = lastMove;

                    node.AddChild(novoJogo, 0, nodesVisitados);
                }

                resposta = moverTopo(node.Jogo.TorreA, node.Jogo.TorreC);
                if (resposta.Item1)
                {
                    int[] otherArray = new int[numDiscos];
                    Array.Copy(node.Jogo.TorreB, otherArray, numDiscos);

                    Jogo novoJogo = new Jogo(
                        resposta.Item2,
                        otherArray,
                        resposta.Item3,
                        profundidade
                    );
                    novoJogo.Custo = novoJogo.Custo + 1;

                    int heuristica = avaliarSistema(novoJogo);
                    novoJogo.isSolucao = heuristica == 0;
                    novoJogo.lastMove = lastMove;

                    node.AddChild(novoJogo, 1, nodesVisitados);
                }

                resposta = moverTopo(node.Jogo.TorreB, node.Jogo.TorreA);
                if (resposta.Item1)
                {
                    int[] otherArray = new int[numDiscos];
                    Array.Copy(node.Jogo.TorreC, otherArray, numDiscos);

                    Jogo novoJogo = new Jogo(
                        resposta.Item3,
                        resposta.Item2,
                        otherArray,
                        profundidade
                    );
                    novoJogo.Custo = novoJogo.Custo + 1;

                    int heuristica = avaliarSistema(novoJogo);
                    novoJogo.isSolucao = heuristica == 0;
                    novoJogo.lastMove = lastMove;

                    node.AddChild(novoJogo, 2, nodesVisitados);
                }

                resposta = moverTopo(node.Jogo.TorreB, node.Jogo.TorreC);
                if (resposta.Item1)
                {
                    int[] otherArray = new int[numDiscos];
                    Array.Copy(node.Jogo.TorreA, otherArray, numDiscos);

                    Jogo novoJogo = new Jogo(
                        otherArray,
                        resposta.Item2,
                        resposta.Item3,
                        profundidade
                    );
                    novoJogo.Custo = novoJogo.Custo + 1;

                    int heuristica = avaliarSistema(novoJogo);
                    novoJogo.isSolucao = heuristica == 0;
                    novoJogo.lastMove = lastMove;

                    node.AddChild(novoJogo, 3, nodesVisitados);
                }

                resposta = moverTopo(node.Jogo.TorreC, node.Jogo.TorreA);
                if (resposta.Item1)
                {
                    int[] otherArray = new int[numDiscos];
                    Array.Copy(node.Jogo.TorreB, otherArray, numDiscos);

                    Jogo novoJogo = new Jogo(
                        resposta.Item3,
                        otherArray,
                        resposta.Item2,
                        profundidade
                    );
                    novoJogo.Custo = novoJogo.Custo + 1;

                    int heuristica = avaliarSistema(novoJogo);
                    novoJogo.isSolucao = heuristica == 0;
                    novoJogo.lastMove = lastMove;

                    node.AddChild(novoJogo, 4, nodesVisitados);
                }

                resposta = moverTopo(node.Jogo.TorreC, node.Jogo.TorreB);
                if (resposta.Item1)
                {
                    int[] otherArray = new int[numDiscos];
                    Array.Copy(node.Jogo.TorreA, otherArray, numDiscos);

                    Jogo novoJogo = new Jogo(
                        otherArray,
                        resposta.Item2,
                        resposta.Item3,
                        profundidade
                    );
                    novoJogo.Custo = novoJogo.Custo + 1;

                    int heuristica = avaliarSistema(novoJogo);
                    novoJogo.isSolucao = heuristica == 0;
                    novoJogo.lastMove = lastMove;

                    node.AddChild(novoJogo, 5, nodesVisitados);
                }

            //por profundidade
            for (int i = 0; i < 6; i++)
            {
                if (node.Children[i] != null)
                {
                    calcularMovimento(node.Children[i], profundidade + 1, i);
                }
            }
        }

        string lastMoveToTxt(int lastMove)
        {
            return lastMove switch
            {
                0 => "A -> B",
                1 => "A -> C",
                2 => "B -> A",
                3 => "B -> C",
                4 => "C -> A",
                5 => "C -> B",
                _ => "",
            };
        }

        void imprimirSolucao(TreeNode node, List<string> texto)
        {
            List<string> newList = texto.Select(a => a).ToList(); ;
            newList.Add($"LastMove: {lastMoveToTxt(node.Jogo.lastMove)} A:{string.Join("", node.Jogo.TorreA)} B:{string.Join("", node.Jogo.TorreB)} C:{string.Join("", node.Jogo.TorreC)} Custo:{node.Jogo.Custo}");
            
            if (node.Jogo.isSolucao)
            {
                Console.WriteLine("SOLUCAO");
                for (int j = 0; j <= texto.Count; j++)
                {
                    Console.WriteLine(newList[j]);
                }

                return;
            }

            for (int i = 0; i < node.Children.Length; i++)
            {
                if (node.Children[i] != null)
                {
                    imprimirSolucao(node.Children[i], newList);
                }
            }
        }

        Jogo jogo = new(new int[numDiscos], new int[numDiscos], new int[numDiscos], 0);
        TreeNode arvore = new TreeNode(jogo);
        inicializarTorre(jogo);
        calcularMovimento(arvore, 0, 0);
        imprimirSolucao(arvore, new List<string>());
    }
}