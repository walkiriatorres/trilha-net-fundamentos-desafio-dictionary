using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DesafioFundamentos.Models
{
    public class Estacionamento
    {
        private decimal PrecoInicial = 0;
        private decimal PrecoAdicional = 0;
        private int Vagas = 0;
        private Dictionary<string, DateTime> Veiculos = new Dictionary<string, DateTime>();

        public Estacionamento(decimal precoInicial, decimal precoAdicional, int vagas)
        {
            this.PrecoInicial = precoInicial;
            this.PrecoAdicional = precoAdicional;
            this.Vagas = vagas;
        }

        public void AdicionarVeiculo()
        {
            if (!EstacionamentoEstaLotado())
            {
                Console.WriteLine("Digite a placa do veículo para estacionar:");
                string placa = Console.ReadLine().ToUpper();

                if (ValidarPlaca(placa))
                {
                    if (!VeiculoEstaEstacionado(placa))
                    {
                        DateTime horaEntrada = DateTime.Now;
                        Veiculos.Add(placa, horaEntrada);
                        Console.WriteLine($"Veículo de placa {placa} entrou no estacionamento em {horaEntrada} adicionado à lista de veículos estacionados.");
                    }
                    else
                    {
                        Console.WriteLine("Veículo já está estacionado.");
                    }
                }
                else
                {
                    Console.WriteLine("Placa inválida");
                }
            }
            else
            {
                Console.WriteLine("Não há vagas disponíveis no momento.");
            }
        }

        public void RemoverVeiculo()
        {
            Console.WriteLine("Digite a placa do veículo para sair:");
            string placa = Console.ReadLine().ToUpper();
            
            if(ValidarPlaca(placa))
            {
                decimal valorAPagar = ConsultarValoraPagar(placa);

                if( valorAPagar > 0)
                {
                    Console.WriteLine("Veículo está estacionado e possui valor a ser pago");
                    Pagar(placa, valorAPagar);

                } else {

                    Console.WriteLine("Veículo NÃO está estacionado. Confira se digitou a placa corretamente");
                }
            } else {

                Console.WriteLine("Placa válida.");

            }
        }

        public void ListarVeiculos()
    {
        if (Veiculos.Any())
        {
            Console.WriteLine("Os veículos estacionados são:");

            foreach (var veiculo in Veiculos)
            {
                Console.WriteLine($"Placa: {veiculo.Key}, Hora de entrada: {veiculo.Value}");
            }
        }
        else
        {
            Console.WriteLine("Não há veículos estacionados.");
        }
    }

        public bool EstacionamentoEstaLotado()
        {
            bool EstaLotado = false;
            
            if (Veiculos.Count == Vagas)
            {
                EstaLotado = true;
            }

            return EstaLotado;
        }

        public bool ValidarPlaca(string placa)
        {
            string padraoAnterior = @"^[A-Z]{3}\d{4}$";
            string padraoMercosul = @"^[A-Z]{3}\d{2}[A-Z]{2}$";

            Regex regexAnterior = new Regex(padraoAnterior, RegexOptions.IgnoreCase);
            Regex regexMercosul = new Regex(padraoMercosul, RegexOptions.IgnoreCase);

            return regexAnterior.IsMatch(placa) || regexMercosul.IsMatch(placa);
        }

        public decimal ConsultarValoraPagar(string placa)
        {
            decimal valorTotal = 0;
            if (Veiculos.TryGetValue(placa.ToUpper(), out DateTime horaEntrada) ||
                VeiculoEstaEstacionado(placa))
            {
                TimeSpan tempoEstacionado = DateTime.Now - horaEntrada;

                if (tempoEstacionado.TotalMinutes <= 60)
                {
                   valorTotal = PrecoInicial;

                } else if (tempoEstacionado.TotalMinutes > 60){

                    int horaAdicional = ((int)Math.Ceiling(tempoEstacionado.TotalHours - 1));
                    valorTotal = PrecoInicial + (PrecoAdicional * horaAdicional);
                }

                Console.WriteLine($"O veículo placa: {placa} ficou estacionado por {tempoEstacionado}.");
                Console.WriteLine($"O valor total a ser pago é: R$ {valorTotal}");                

                return valorTotal;                
            }
            else
            {
                Console.WriteLine("Erro em CalcularPermanencia().");
                return -1;
            }
        }

        public void Pagar(string placa, decimal valorTotal)
        {
            Console.WriteLine("Informe o método de pagamento:");
            Console.WriteLine("1 - Cartão de Débito");
            Console.WriteLine("2 - Cartão de Crédito");
            Console.WriteLine("3 - Dinheiro");
            Console.WriteLine("4 - Pix");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("Insira o cartão de débito... Senha solicitada... Processando pagamento... Pagamento recebido! Retire o cartão.");
                    LiberarSaida(placa);
                    break;

                case "2":
                    Console.WriteLine("Insira o cartão de crédito... Senha solicitada... Processando pagamento... Pagamento recebido! Retire o cartão. ");
                    LiberarSaida(placa);
                    break;

                case "3":
                    Console.WriteLine("Recebendo pagamento em dinheiro... Pagamento recebido! ");
                    LiberarSaida(placa);
                    break;

                case "4":
                    Console.WriteLine("Chave Pix telefone: (81)9999-9999");
                    Console.WriteLine("Recebendo pagamento em PIX... Pagamento recebido! ");
                    LiberarSaida(placa);
                    break;

                default:
                    Console.WriteLine("Opção inválida");
                    break;
            }
        }
        public bool VeiculoEstaEstacionado(string placa)
        {
            return Veiculos.ContainsKey(placa.ToUpper());
        }

        public void LiberarSaida(string placa)
        {
            Veiculos.Remove(placa);
            Console.WriteLine($"O veículo placa: {placa} está liberado. Até mais!");        
        }
    }
}

