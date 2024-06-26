﻿using System;
using BoletoNetCore.Extensions;
using static System.String;

namespace BoletoNetCore
{
    [CarteiraCodigo("112")]
    public class BancoInterCarteira112 : ICarteira<BancoInter>
    {
        internal static Lazy<ICarteira<BancoInter>> Instance { get; } = new Lazy<ICarteira<BancoInter>>(() => new BancoInterCarteira112());

        private BancoInterCarteira112()
        {

        }

        public void FormataNossoNumero(Boleto boleto)
        {

            // Nosso número não pode ter mais de 11 dígitos

            if (IsNullOrWhiteSpace(boleto.NossoNumero) || boleto.NossoNumero == "00000000000")
            {
                // Banco irá gerar Nosso Número
                boleto.NossoNumero = new String('0', 11);
                boleto.NossoNumeroDV = "0";
                boleto.NossoNumeroFormatado = "000/00000000000-0";
            }
            else
            {
                // Nosso Número informado pela empresa
                if (boleto.NossoNumero.Length > 11)
                    throw new Exception($"Nosso Número ({boleto.NossoNumero}) deve conter 11 dígitos.");
                boleto.NossoNumero = boleto.NossoNumero.PadLeft(11, '0');
                boleto.NossoNumeroDV = (boleto.Carteira + boleto.NossoNumero).CalcularDVBancoInter();
                boleto.NossoNumeroFormatado = $"{boleto.Carteira.PadLeft(3, '0')}/{boleto.NossoNumero}-{boleto.NossoNumeroDV}";
            }

        }

        public string FormataCodigoBarraCampoLivre(Boleto boleto)
        {
            var contaBancaria = boleto.Banco.Beneficiario.ContaBancaria;
            return $"{contaBancaria.Agencia}{boleto.Carteira}{boleto.Banco.Beneficiario.CodigoTransmissao}{boleto.NossoNumero}";
        }
    }
}
