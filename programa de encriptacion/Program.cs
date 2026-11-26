
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

Dictionary<char, int> letras = new()
{
    {' ', 0}, {'A', 1}, {'B', 2}, {'C', 3}, {'D', 4}, {'E', 5}, {'F', 6},
    {'G', 7}, {'H', 8}, {'I', 9}, {'J', 10}, {'K', 11}, {'L', 12}, {'M', 13},
    {'N', 14}, {'O', 15}, {'P', 16}, {'Q', 17}, {'R', 18}, {'S', 19}, {'T', 20},
    {'U', 21}, {'V', 22}, {'W', 23}, {'X', 24}, {'Y', 25}, {'Z', 26}
};

Dictionary<int, char> letras_invertido = new Dictionary<int, char>();

foreach (var par in letras)
{
    letras_invertido.Add(par.Value, par.Key);
}

static int[] mensaje_a_numeros(Dictionary<char, int> letras)
{
    Console.WriteLine("Ingresa un mensaje a encriptar: ");
    List<int> mensaje_en_numeros = new List<int>();
    string mensaje = Console.ReadLine()!.ToUpper();

    while (mensaje.Length % 3 != 0)
    {
        mensaje += ' ';
    }

    foreach (char word in mensaje)
    {
        if (letras.ContainsKey(word))
        {
            int numero = letras[word];
            mensaje_en_numeros.Add(numero);
        }

    }
    return mensaje_en_numeros.ToArray();
}

static int[,] convertir_a_matriz(int[] mensaje_en_numeros)
{
    int[,] matrizinicial = new int[3, (mensaje_en_numeros.Length /  3)];
    int indice = 0;
    for(int i = 0; i < (mensaje_en_numeros.Length / 3); i++)
    {
        for (int j = 0; j < 3; j++)
        {
            matrizinicial[j, i] = mensaje_en_numeros[indice];
            indice++;
        }
    }
    return matrizinicial;
}

static int[,] crearmatrizclave(int [,] matriz_a_encriptar)
{
    Random random = new Random();
    
    Console.WriteLine("Que desea realizar?: \n");
    Console.WriteLine("1 - Ingresar una matriz llave. \n");
    Console.WriteLine("2 - Crear una matriz llave de manera automatica. \n");
    Console.Write("Ingresa tu decision (1 / 2):  ");
    int decision = int.Parse(Console.ReadLine()!);
    int[,] matrizllave = new int[matriz_a_encriptar.GetLength(0), matriz_a_encriptar.GetLength(0)];
    if (decision == 1)
    {
        int determinante;
        do
        {
            for(int i = 0; i < matriz_a_encriptar.GetLength(0); i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Console.WriteLine($"Ingresa el numero en fila {j + 1} y columna {i + 1}");
                    matrizllave[j, i] = int.Parse(Console.ReadLine()!);
                }
            }
            determinante =
                matrizllave[0, 0] * (matrizllave[1, 1] * matrizllave[2, 2] - matrizllave[1, 2] * matrizllave[2, 1]) -
                matrizllave[0, 1] * (matrizllave[1, 0] * matrizllave[2, 2] - matrizllave[1, 2] * matrizllave[2, 0]) +
                matrizllave[0, 2] * (matrizllave[1, 0] * matrizllave[2, 1] - matrizllave[1, 1] * matrizllave[2, 0]);
            if (determinante == 0)
            {
                Console.WriteLine("\nLa matriz ingresada tiene determinante 0, intente con otros numeros.");
            }
        } while (determinante == 0);
    }
    
    else
    {
        int determinante;
        do
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrizllave[j, i] = random.Next(1, 101);
                }
            }

            determinante =
                matrizllave[0, 0] * (matrizllave[1, 1] * matrizllave[2, 2] - matrizllave[1, 2] * matrizllave[2, 1]) -
                matrizllave[0, 1] * (matrizllave[1, 0] * matrizllave[2, 2] - matrizllave[1, 2] * matrizllave[2, 0]) +
                matrizllave[0, 2] * (matrizllave[1, 0] * matrizllave[2, 1] - matrizllave[1, 1] * matrizllave[2, 0]);

        } while (determinante == 0);

    }
    return matrizllave;
}

static double[,] encriptarmatriz(int [,] matriz_llave, int [,] matriz_a_encriptar)
{
    double[,] matriz_resultado = new double[matriz_llave.GetLength(0), matriz_a_encriptar.GetLength(1)];
    for (int i = 0; i < matriz_llave.GetLength(0); i++)
    {
        for (int j = 0; j < matriz_a_encriptar.GetLength(1); j++)
        {
            double suma = 0;
            for (int k = 0; k < matriz_llave.GetLength(1); k++)
            {
                suma += matriz_llave[i, k] * matriz_a_encriptar[k,j];
            }
            matriz_resultado[i, j] = suma;
        }
    }
    return matriz_resultado;
}



static (int[], int[]) datos_paradesencriptar()
{
    Console.WriteLine("Ingrese el mensaje encriptado, añada espacios despues de cada valor: ");
    string mensajeadesencriptar = Console.ReadLine()!;
    string[] mensajeseparado = mensajeadesencriptar.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    Console.WriteLine("\nIngrese la matriz clave utilizada para encriptar el mensaje, favor añada espacios despues de cada valor:");
    string matrizclave = Console.ReadLine()!;
    string[] matrizclaveseparada = matrizclave.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    int[] mensajeennumeros = new int[mensajeseparado.Length];
    int[] matrizclave_numeros = new int[matrizclaveseparada.Length];

    for (int i = 0; i < mensajeseparado.Length; i++)
    {
        mensajeennumeros[i] = int.Parse(mensajeseparado[i]);
    }

    for (int i = 0; i < matrizclaveseparada.Length; i++)
    {
        matrizclave_numeros[i] = int.Parse(matrizclaveseparada[i]);
    }

    return (mensajeennumeros, matrizclave_numeros);
}


static double[,] calcular_inversa_llave(int[] matrizclave_numeros)
{
    int dimension = (int)Math.Sqrt(matrizclave_numeros.Length);
    double[,] matriz_aumentada = new double[dimension, 2 * dimension];
    int indice = 0;

    for (int i = 0; i < dimension; i++)
    {
        for (int j = 0; j < dimension; j++)
        {
            matriz_aumentada[i, j] = matrizclave_numeros[indice];
            indice++;
        }
        matriz_aumentada[i, i + dimension] = 1.0;
    }

    for (int i = 0; i < dimension; i++)
    {
        int fila_maxima = i;
        for (int k = i + 1; k < dimension; k++)
        {
            if (Math.Abs(matriz_aumentada[k, i]) > Math.Abs(matriz_aumentada[fila_maxima, i]))
                fila_maxima = k;
        }

        if (fila_maxima != i)
        {
            for (int j = 0; j < 2 * dimension; j++)
            {
                double temporal = matriz_aumentada[i, j];
                matriz_aumentada[i, j] = matriz_aumentada[fila_maxima, j];
                matriz_aumentada[fila_maxima, j] = temporal;
            }
        }

        double pivote = matriz_aumentada[i, i];
        if (Math.Abs(pivote) < 1e-9)
            throw new Exception("La matriz llave no es invertible (determinante igual a 0).");

        for (int j = 0; j < 2 * dimension; j++)
            matriz_aumentada[i, j] /= pivote;

        for (int k = 0; k < dimension; k++)
        {
            if (k != i)
            {
                double factor = matriz_aumentada[k, i];
                for (int j = 0; j < 2 * dimension; j++)
                    matriz_aumentada[k, j] -= factor * matriz_aumentada[i, j];
            }
        }
    }

    double[,] matriz_llave_inversa = new double[dimension, dimension];
    for (int i = 0; i < dimension; i++)
    {
        for (int j = 0; j < dimension; j++)
        {
            matriz_llave_inversa[i, j] = matriz_aumentada[i, j + dimension];
        }
    }

    return matriz_llave_inversa;
}


static int[,] convertirnumeros_a_matriz(int[] numeros_a_desencriptar, double[,] matrizllave_inversa)
{
    int[,] matrizadesencriptar = new int[matrizllave_inversa.GetLength(0), 
    (numeros_a_desencriptar.Length / matrizllave_inversa.GetLength(0)) ];
    int indice = 0;

    for (int i = 0; i < (numeros_a_desencriptar.Length / matrizllave_inversa.GetLength(0)); i++)
    {
        for (int j = 0; j < matrizllave_inversa.GetLength(0); j++)
        {
            matrizadesencriptar[j, i] = numeros_a_desencriptar[indice];
            indice++;
        }
    }

    return matrizadesencriptar;
}

static int[,] desencriptarmatriz(int[,] matriz_a_desencriptar, double[,] matrizllave_inversa)
{
    int[,] matriz_resultado = new int[matrizllave_inversa.GetLength(0), matriz_a_desencriptar.GetLength(1)];
    for (int i = 0; i < matrizllave_inversa.GetLength(0); i++)
    {
        for (int j = 0; j < matriz_a_desencriptar.GetLength(1); j++)
        {
            double suma = 0;
            for (int k = 0; k < matrizllave_inversa.GetLength(1); k++)
            {
                suma += matrizllave_inversa[i, k] * matriz_a_desencriptar[k, j];
            }
            matriz_resultado[i, j] = (int)Math.Round(suma);
        }
    }
    return matriz_resultado;
}

static string presentarmensaje(int [,] matriz_desencriptada, Dictionary<int, char> letras_invertido)
{
    List<char> letrass = new List<char>();
    for (int i = 0; i < matriz_desencriptada.GetLength(1); i++)
    {
        for (int j = 0; j < matriz_desencriptada.GetLength(0); j++)
        {
            int num = matriz_desencriptada[j, i];
            if (letras_invertido.ContainsKey(num))
            {
                char word = letras_invertido[num];
                letrass.Add(word);
            }
        }
    }
    string mensaje = string.Join("", letrass);
    return mensaje;
}

static int desplegarmenu()
{
    Console.WriteLine("Que desea realizar?: \n");
    Console.WriteLine("1 - Encriptar un mensaje. \n");
    Console.WriteLine("2 - Desencriptar un mensaje. \n");
    Console.Write("Ingresa tu decision (1 / 2): ");
    int decision = int.Parse(Console.ReadLine()!);
    return decision;
}

int decisionusuario = desplegarmenu();

if (decisionusuario == 1)
{
    int[] mensaje_en_numeros = mensaje_a_numeros(letras);

    int[,] matriz_a_encriptar = convertir_a_matriz(mensaje_en_numeros);

    int[,] matriz_llave = crearmatrizclave(matriz_a_encriptar);

    double[,] matriz_encriptada = encriptarmatriz(matriz_llave, matriz_a_encriptar);

    Console.WriteLine("\nMensaje encriptado:");

    for (int i = 0; i < matriz_encriptada.GetLength(1); i++)
    {
        for (int j = 0; j < matriz_encriptada.GetLength(0); j++)
        {
            Console.Write(matriz_encriptada[j, i] + " ");
        }
    }

    Console.WriteLine("\n\nMatriz llave:");

    for (int i = 0; i < matriz_llave.GetLength(0); i++)
    {
        for (int j = 0; j < matriz_llave.GetLength(1); j++)
        {
            Console.Write(matriz_llave[i, j] + " ");
        }
    }
}
else if (decisionusuario == 2)
{
    (int[] numeros_a_desencriptar, int[] matrizclave_desencriptar) = datos_paradesencriptar();

    double[,] matrizllave_inversa = calcular_inversa_llave(matrizclave_desencriptar);

    int[,] matriz_a_desencriptar = convertirnumeros_a_matriz(numeros_a_desencriptar, matrizllave_inversa);

    int[,] matriz_desencriptada = desencriptarmatriz(matriz_a_desencriptar, matrizllave_inversa);

    string mensaje = presentarmensaje(matriz_desencriptada, letras_invertido);

    Console.WriteLine("\nMensaje desencriptado: " + mensaje);
}
else
{
    Console.WriteLine("Opcion no valida.");
}