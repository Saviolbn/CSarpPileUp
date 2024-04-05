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


class Program
{
    static void Main(string[] args)
    {
        int numDiscos = 5;
        int[] torreA = new int[numDiscos];
        int[] torreB = new int[numDiscos];
        int[] torreC = new int[numDiscos];


        void inicializarTorre()
        {
            for (int i = 0; i < numDiscos; i++)
            {
                torreA[i] = numDiscos - i;
            }
        };

        // quanto menor o valor retornado, melhor estamos
        int avaliarSistema()
        {
            int h = 0;

            for (int i = numDiscos; i > 0; i--)
            {
                if (torreC[i - 1] != i)
                {
                    h++;
                }
            }

            return h;
        }



        // move do topo da torreX para o fundo da torre Y
        bool moverTopo(int[] torreX, int[] torreY)
        {
            int topoIndex = -1;
            for (int i = numDiscos - 1; i >= 0; i--)
            {
                if(torreX[i] != 0) 
                {
                    topoIndex = i;
                    break;
                }
            }

            if (topoIndex == -1) 
            {
                return false;
            } 

            Console.WriteLine(topoIndex);


            for (int i = 0; i < numDiscos - 1; i++)
            {
                if(i == 0) 
                {
                    if (torreY[i] == 0) 
                    {
                        torreY[i] = torreX[topoIndex];
                        torreX[topoIndex] = 0;
                    }
                } else {               
                    if (torreY[i] == 0 && torreY[i - 1] > torreX[topoIndex])
                    {
                        torreY[i] = torreX[topoIndex];
                        torreX[topoIndex] = 0;
                    }
                }
            }

            return true;
        }

        void imprimirSistema()
        {
            Console.WriteLine($"{string.Join("", torreA)} {string.Join("", torreB)} {string.Join("", torreC)}");
        }


        inicializarTorre();
        imprimirSistema();

        moverTopo(torreA, torreC);
        imprimirSistema();

        moverTopo(torreA, torreC);
        imprimirSistema();

        moverTopo(torreA, torreB);
        imprimirSistema();

        moverTopo(torreC, torreB);
        imprimirSistema();
    }
}