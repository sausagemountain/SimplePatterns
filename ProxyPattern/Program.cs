using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProxyPattern
{
    class Program
    {
        static IEnumerable<(T1, T2)> Permutations<T1, T2> (IEnumerable<T1> a, IEnumerable<T2> b)
        {
            foreach (T1 a1 in a) {
                foreach (T2 b1 in b) {
                    yield return (a1, b1);
                }
            }
        }

        static void Main (string[] args)
        {
            var names = new List<string>() {
                                               "Василий Петрович Зелёный",
                                               "Евгений Сергеевич Жмых",
                                               "Афанасий Константинович Щелкунов"
                                           };

            var positions = new List<string>() {
                                                   "Генеральный директор",
                                                   "Электрик",
                                                   "Выгульщик собак", 
                                                   "Системный администратор"
                                               };


            var all = Permutations(names, positions).ToList();

            Console.WriteLine("Service");
            IService s = new Service();
            for (int count = 0; count < 2; count++) {
                foreach (var tuple in all)
                    Console.WriteLine("{0,20:#.000}\t{1}", s.CalculateIndex(tuple.Item1, tuple.Item2), tuple);
                Console.WriteLine();
            }

            Console.WriteLine("Proxy");
            s = new Proxy1(new Service());
            for (int count = 0; count < 2; count++) {
                foreach (var tuple in all)
                    Console.WriteLine("{0, 20:#.000}\t{1}", s.CalculateIndex(tuple.Item1, tuple.Item2), tuple);
                Console.WriteLine();
            }

            Console.ReadKey(true);
        }
    }
}
