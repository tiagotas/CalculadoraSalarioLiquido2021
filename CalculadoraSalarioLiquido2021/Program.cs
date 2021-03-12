using System;

using System.Globalization;

namespace CalculadoraSalarioLiquido2021
{
    class Program
    {
        static void Main(string[] args)
        {
            /**
             * Regra de Negócio do cálculo do salário líquido:
             * https://www.contabeis.com.br/noticias/42234/calculo-da-nova-tabela-progressiva-do-inss-aliquotas-e-parcela-a-deduzir/
             * 
             * Calculadora de Teste:
             * https://www.todacarreira.com/calculadora-de-salario-liquido/?value=3000&dependents=&otherdiscounts=#salary-simulator
             */

            NumberFormatInfo nfi = new CultureInfo("pt-BR").NumberFormat;

            string nome;
            double salario_bruto = 0;
            int dependentes = 0;

            Console.WriteLine("Bem-vindo a calculadora de salário liquido =D");

            Console.WriteLine("Qual é seu nome? ");
            nome = Console.ReadLine();

            Console.WriteLine("Informe seu salário bruto: ");
            salario_bruto = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine("Quantidade de Dependentes: ");
            dependentes = Convert.ToInt32(Console.ReadLine());


            /**
             * Calculando o desconto do INSS:
             * 1ª Faixa = 7,5% até um salário mínimo (R$ R$ 1.045);
               2ª Faixa = 9,0% para quem ganha entre R$ 1.045,01 R$ e 2.089,60.
               3ª Faixa = 12% para quem ganha entre R$ 2.089,61 e R$ 3.134,40.
               4ª Faixa = 14% para quem ganha entre R$ 3.134,41 e R$ 6.101,06.
             * 
             * Exemplo de cálculo para um salário de R$ 3.000,00
             * 
             * 1ª Faixa: R$ 1.045 x 0,075          = R$ 78,38
             * 2ª Faixa: 2089 - 1045 = 1044 x 0,09 = R$ 93,96
             * 3ª Faixa: 3000 - 2089 = 911 x 0,12  = R$ 109,32
             * 
             * Total a pagar de INSS = 78,38 + 93,96 + 109,32 = 281,66
             */
            double primeira_faixa = 0;
            double segunda_faixa = 0;
            double terceira_faixa = 0;
            double quarta_faixa = 0;


            // Se a pessoa ganhar até um salário minimo.
            if(salario_bruto <= 1045)
            {
                primeira_faixa = salario_bruto * 0.075;
            }

            // Se a pessoa ganha mais que um salário minimo.
            if(salario_bruto > 1045)
            {
                primeira_faixa = 1045 * 0.075;


                // Calculando o valor caso a pessoa ganhe menos que o teto da segunda faixa.
                if(salario_bruto <= 2089.60)
                {
                    segunda_faixa = (salario_bruto - 1045) * 0.09;
                }

                // Calculando caso a pessoa ganhe MAIS que o teto da segunda faixa.
                if(salario_bruto >= 2089.61)
                {
                    segunda_faixa = (2089.60 - 1045) * 0.09;

                    // Se a pessoa ganha ATÉ o teto da terceira faixa.
                    if(salario_bruto <= 3134.40)
                    {
                        terceira_faixa = (salario_bruto - 2089.60) * 0.12;
                    }

                    // Caso a pessoa ganhe MAIS que o TETO da terceira faixa
                    if(salario_bruto > 3134.41)
                    {
                        terceira_faixa = (3134.40 - 2089.60) * 0.12;

                        // Caso a pessoa ganhe MENOS que o teto (R$ 6.101,06) da QUARTA faixa.
                        if(salario_bruto <= 6101.06)
                        {
                            quarta_faixa = (salario_bruto - 3134.40) * 0.14;
                        }

                        // Caso a pessoa ganhe MAIS que o teto (R$ 6.101,07) da QUARTA faixa.
                        if (salario_bruto > 6101.07)
                        {
                            quarta_faixa = (6101.06 - 3134.41) * 0.14;
                        }
                    } // Fecha o IF to teto da terceira faixa.
                } // Fecha o IF do teto da segunda faixa.
            }  // Fecha IF do salário maior que salário minimo.


            double desconto_inss = primeira_faixa + segunda_faixa + terceira_faixa + quarta_faixa;

            /**
             * Calculando o IRRF
             * 1) Descontar o valor do INSS
             * 2) Descontar de acordo com a Tabela
             * 
             * Faixas                              Aliquota      Deduzir
             * Até R$ 1.903,98                       -             - 
             * De R$ 1.903,99 até R$ 2.826,65        7,5%         142,80
             * De R$ 2.826,66 até R$ 3.751,05        15%          354,80
             * De R$ 3.751,06 até R$ 4.664,68        22,5%        636,13
             * Acima de R$ 4.664,68                  27,5%        869,36
             */

            double base_calculo = salario_bruto - desconto_inss - (dependentes * 189.59);
            double desconto_ir = 0;
            string aliquota = "";


            if(base_calculo >= 1903.99 && base_calculo <= 2826.65)
            {
                desconto_ir = (base_calculo * 0.075) - 142.8;
                aliquota = "7,5%";
            }

            if(base_calculo >= 2826.66 && base_calculo <= 3751.05)
            {
                desconto_ir = (base_calculo * 0.15) - 354.8;
                aliquota = "15%";
            }

            if(base_calculo >= 3751.06 && base_calculo <= 4664.68)
            {
                desconto_ir = (base_calculo * 0.225) - 636.13;
                aliquota = "22,5%";
            }

            if(base_calculo > 4664.68)
            {
                desconto_ir = (base_calculo * 0.275) - 869.36;
                aliquota = "27,5%";
            }



            double salario_liquido = salario_bruto - desconto_ir - desconto_inss;

            double total_descontos = desconto_ir + desconto_inss;



            Console.WriteLine("INSS da 1ª Faixa 7,5% {0} ", primeira_faixa.ToString("C", nfi) );
            Console.WriteLine("INSS da 2ª Faixa   9% {0} ", segunda_faixa.ToString("C", nfi));
            Console.WriteLine("INSS da 3ª Faixa  12% {0} ", terceira_faixa.ToString("C", nfi));
            Console.WriteLine("INSS da 4ª Faixa  14% {0} ", quarta_faixa.ToString("C", nfi));          
            Console.WriteLine("Desconto INSS {0} ", desconto_inss.ToString("C", nfi));

            Console.WriteLine("");

            Console.WriteLine("Valor dos {0} IRRF {1} ", aliquota, desconto_ir.ToString("C", nfi));

            Console.WriteLine("");

            Console.WriteLine("Total de descontos {0} ", total_descontos.ToString("C", nfi));
            Console.WriteLine("Seu salário Liquido é: {0} ", salario_liquido.ToString("C", nfi));


            Console.ReadKey();
        }
    }
}
