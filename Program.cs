using System.Diagnostics;

// start measuring time
Stopwatch sw = Stopwatch.StartNew();

// primes in 1 to 100
int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };

// divide range of int.MaxValue between the number of threads my cpu has
int[] ranges = new int[8];
for (int i = 0; i < 8; i++)
{
    ranges[i] = (int.MaxValue / 8) * (i + 1);
}


void SearchSolution(int seed_start, int seed_end, int thread_number)
{
    int solution = seed_start;

    while (true)
    {
        // determine seed
        //int seed = solution++;
        if (solution % 1000000 == 0) Console.WriteLine($"[Thread {thread_number}] Tested past seed: {solution/1000000} million");
        
        // create local rng generator
        Random rng = new Random(solution++);

        // generate random numbers // check if its a solution/matches
        int[] rng_solution = new int[primes.Length];
        bool isSolution = true;
        for (int i = 0; i < primes.Length; i++)
        {
            rng_solution[i] = rng.Next(6);
            if (rng_solution[i] != primes[i])
            {
                isSolution = false;
            }
        }
        
        if (isSolution)
        {
            Console.WriteLine("Solution found");
            Console.WriteLine($"Seed is {solution}");
            //solution = seed;

            // generating 100 random numbers to check 
            Console.WriteLine($"[Thread {thread_number}] Generating 100 numbers with last seed: ");
            Random verify = new Random(solution);
            for (int i = 0; i < 11; i++)
            {
                Console.Write($"{verify.Next(6)},");
            }
            break;
        }
        if (solution == seed_end)
        {
            Console.WriteLine($"[Thread {thread_number}] Thread reached seed range end");
            break;
        }
    }
    

}

Thread[] threads = new Thread[8];
// make threads, idk how tf i failed to loop and make them correctly but hey this works
threads[0] = new Thread(() => SearchSolution(0, ranges[0], 1));
threads[1] = new Thread(() => SearchSolution(ranges[0], ranges[1], 2));
threads[2] = new Thread(() => SearchSolution(ranges[1], ranges[2], 3));
threads[3] = new Thread(() => SearchSolution(ranges[2], ranges[3], 4));
threads[4] = new Thread(() => SearchSolution(ranges[3], ranges[4], 5));
threads[5] = new Thread(() => SearchSolution(ranges[4], ranges[5], 6));
threads[6] = new Thread(() => SearchSolution(ranges[5], ranges[6], 7));
threads[7] = new Thread(() => SearchSolution(ranges[6], ranges[7], 8));

for (int i = 0; i < 8; i++)
{
    threads[i].Start();
}

// wait for all threads to finish 
for (int i = 0; i < 8; i++)
{
    threads[i].Join(); // waits for thread
}


// show execution time
sw.Stop();
Console.WriteLine("All processes finished! check console for results.");
Console.WriteLine($"Operation took: {sw.ElapsedMilliseconds / 1000} seconds");
Console.WriteLine("Press enter to exit.");
Console.ReadLine();
