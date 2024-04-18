using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static Random random = new Random(); //random sayı oluşturmak için referans oluşturduk

    static void Main()
    {
        int n = 8; // Satranç tahtası boyutu
        int populationSize = 10000; // Popülasyon boyutu
        double mutationRate = 0.01; // Mutasyon oranı, her bireyin mutasyona uğrama şansı
        double crossoverRate = 0.6; // Çaprazlama oranı, yeni bireylerin çaprazlama ile oluşma şansı (PERMÜTASYON)
        int desiredSolutionsCount = 92; // Bulunması gereken çözüm sayısı. (Tekrara girmemek için)
        int iterasyon = 10000; // İzin verilen maksimum nesil (iterasyon) sayısı

        List<int[]> population = InitializePopulation(n, populationSize); //10.000 adet popülasyonu yolluyor.
        List<int[]> solutions = new List<int[]>();
        int generation = 0;//iterasyon sayacı

        // Çözüm bulma süreci
        while (solutions.Count < desiredSolutionsCount && generation < iterasyon) //iterasyon sayısı 10.000 olana kadar döngüye gir
        {
            List<int[]> newPopulation = NextGeneration(population, mutationRate, crossoverRate, n); //yeni bir nesil oluşturmak için parametreleri gönder
            population = newPopulation;//dönen diziyi rastele oluşturulan diziye ata

            foreach (var individual in population)
            {
                if (IsSolution(individual, n) && !solutions.Any(sol => sol.SequenceEqual(individual)))//yeni nesili tehdit eden yoksa ve benzersizse
                {
                    solutions.Add(individual);//yeni uygun çözümü ekle
                }
            }
            generation++;//10.000'e kadar artırıyor
        }

        // Çözümlerin yazdırılması
        if (solutions.Any())//çözüm bulunuyorsa;
        {
            Console.WriteLine($"Toplam {solutions.Count} çözüm bulundu, {generation} nesillerde:");
            foreach (var solution in solutions)
            {
                Console.WriteLine(" ------------------");
                PrintBoard(solution);//düzgün şekilde yazdırmak için metoda git
                Console.WriteLine(" ------------------");
                Console.WriteLine();
            }
        }
        else//çözüm yoksa;
        {
            Console.WriteLine("Çözüm bulunamadı.");
        }
        //proje kapanışı
    }

    // Popülasyonun ilk başta rastgele oluşturulması
    static List<int[]> InitializePopulation(int n, int size)//10.000 adet popülasyondan 8x8 tahtaya dizilim yaptırma yeri
    {
        List<int[]> population = new List<int[]>();
        for (int i = 0; i < size; i++)
        {
            int[] individual = Enumerable.Range(0, n).OrderBy(x => random.Next()).ToArray();// rastgele yerleştirme
            population.Add(individual);//
        }
        return population;
    }

    // Bir nesilden sonraki nesli oluşturma
    static List<int[]> NextGeneration(List<int[]> currentPopulation, double mutationRate, double crossoverRate, int n)
    {
        List<int[]> newPopulation = new List<int[]>();
        while (newPopulation.Count < currentPopulation.Count)//önceki popülasyon kadar yap
        {
            int[] parent1 = SelectParent(currentPopulation);
            int[] parent2 = SelectParent(currentPopulation);
            int[] child = random.NextDouble() < crossoverRate ? Crossover(parent1, parent2, n) : random.NextDouble() < 0.5 ? parent1 : parent2;
            //2 ebevenden 1 yeni nesil çıkar. Çaprazlama oranı ile birlikte.
            Mutate(child, mutationRate);//mutasyona uğrat
            newPopulation.Add(child);
        }
        return newPopulation;
    }

    // Ebeveyn seçimi
    static int[] SelectParent(List<int[]> population)
    {
        return population[random.Next(population.Count)];
    }

    // İki ebeveyn arasında çaprazlama yaparak çocuk oluşturma
    static int[] Crossover(int[] parent1, int[] parent2, int n)
    {
        int[] child = new int[n];
        int crossoverPoint = random.Next(1, n);//rassal çaprazlama sayısı belirle
        HashSet<int> genes = new HashSet<int>();//bu dizi sayesinde tekrar eden eleman almaz ve eşsiz çözümler elde ederiz

        for (int i = 0; i < crossoverPoint; i++)
        {
            child[i] = parent1[i];//ebeveyn 1i yeni nesile atıyor
            genes.Add(parent1[i]);
        }

        int fillPoint = crossoverPoint;
        for (int i = 0; i < n; i++) // parent2'den eksik genleri ekliyoruz
        {
            if (!genes.Contains(parent2[i]))
            {
                if (fillPoint < n)
                {
                    child[fillPoint] = parent2[i];
                    fillPoint++;
                }
            }
        }
        return child;
    }

    // Bireyin genlerinde rastgele mutasyon uygulama
    static void Mutate(int[] individual, double mutationRate)
    {
        for (int i = 0; i < individual.Length; i++)//her bir eleman için
        {
            if (random.NextDouble() < mutationRate)//oluşturulan rassal oran mutasyon oranından küçükse mutasyona uğrat.
            {
                int j = random.Next(individual.Length);
                int temp = individual[i];
                individual[i] = individual[j];
                individual[j] = temp;
            }
        }
    }

    // Bireyin çözüm olup olmadığını kontrol etme
    static bool IsSolution(int[] individual, int n)
    {
        for (int i = 0; i < n; i++)
        {
            for (int j = i + 1; j < n; j++)
            {
                if (Math.Abs(i - j) == Math.Abs(individual[i] - individual[j]))//tehdit olup olmadığını kontrol ediyor
                {
                    return false; // Tehdit var!
                }
            }
        }
        return true; // Tehdit Yok
    }

    // Satranç tahtasını görsel olarak yazdırma
    static void PrintBoard(int[] solution)
    {
        int n = solution.Length;

        for (int i = 0; i < n; i++)
        {
            Console.Write(" |");
            for (int j = 0; j < n; j++)
            {
                Console.Write(solution[j] == i ? "Q " : ". ");//vezir varsa Q koy yoksa . koy
            }
            Console.Write("|");
            Console.WriteLine();
        }
    }
}