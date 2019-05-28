using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositePattern
{
    public static class Program
    {
        public static Random Rnd = new Random();

        static void Main(string[] args)
        {
            ITree<Pos> company;
            {
                company = new Manager(Pos.CEO, "ceo",
                    new Manager(Pos.HR, "hr0",
                        new Manager(Pos.Manager, "m0",
                            new Employee(Pos.s_emp),
                            new Employee(Pos.s_emp),
                            new Employee(Pos.s_emp),
                            new Employee(Pos.s_emp),
                            new Employee(Pos.s_emp),
                            new Manager(Pos.Manager, "m1",
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp)
                            )
                        )
                    ),
                    new Manager(Pos.Sales, "s0",
                        new Employee(Pos.s_emp),
                        new Employee(Pos.s_emp),
                        new Employee(Pos.s_emp),
                        new Employee(Pos.s_emp),
                        new Employee(Pos.s_emp),
                        new Employee(Pos.s_emp),
                        new Manager(Pos.Manager, "m2",
                            new Employee(Pos.m_emp),
                            new Employee(Pos.m_emp),
                            new Employee(Pos.m_emp),
                            new Employee(Pos.m_emp),
                            new Employee(Pos.m_emp),
                            new Employee(Pos.m_emp),
                            new Manager(Pos.Manager, "m3",
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp),
                                new Employee(Pos.emp)
                            )
                        )
                    )
                );
            }


            Console.WriteLine(company.ToString());
            Console.WriteLine();

            var result = company.PerformTask(Pos.emp, "do job");
            foreach (var s in result) {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            result = company.PerformTask(Pos.HR, "manage people");
            foreach (var s in result)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            result = company.PerformTask(Pos.Sales, "create sales strategies");
            foreach (var s in result)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            result = company.PerformTask(Pos.s_emp, "sell products");
            foreach (var s in result) {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            result = company.PerformTask(Pos.CEO, "Rule company");
            foreach (var s in result)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            result = company.PerformTask(Pos.Manager, "assign tasks");
            foreach (var s in result) {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            result = company.PerformTask(Pos.m_emp, "do stuff");
            foreach (var s in result) {
                Console.WriteLine(s);
            }
            Console.WriteLine();

            Console.ReadKey(true);
        }
    }
}
