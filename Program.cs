// Torre de Hanoi
// Há 3 torres A B C
// Há Y discos na Torre A de maneira ordenada (em baixo está o maior)
// Deve se mover todos os discos para a torre C mantendo a mesma ordem
// Um disco maior não pode ficar em cima de um menor, ou uma haste vazia
// Só pode mover um disco por vez

// Será usado o algoritmo de busca A*

int numDiscos = 5;
int[] torreA= new int[numDiscos];
int[] torreB= new int[numDiscos];
int[] torreC= new int[numDiscos];


void inicializarTorre()
{
    for (int i = numDiscos - 1; i >= 0; i--)
    {
        torreA[i] = i;
    }
};

inicializarTorre();
Console.WriteLine($"{string.Join("",torreA)} {string.Join("",torreB)} {string.Join("",torreC)}");

Console.WriteLine("Hello, World!");