﻿using NUnit.Framework;
using QuantoEh.Dominio;
using StoryQ.pt_BR;

namespace QuantoEh.Tests
{
    [TestFixture]
    public class CalcularTweet
    {
        private TweetParaProcessar _tweetParaProcessar;
        private Resposta _resposta;

        [Test]
        public void CalcularTweetTestes()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o quanto eh calcule meus tweets")

               .ComCenario("tweet bem formado com soma simples")
               .Dado(UmTweetBemFormadoComUsuario_ETexto_, "giovannibassi", "@quantoeh 2 + 3")
               .Quando(SolicitoUmResultadoAEsteTweet)
               .Entao(TenhoOResultaoEsperado_, "5 RT @giovannibassi: @quantoeh 2 + 3")

               .ComCenario("tweet bem formado com soma simples")
               .Dado(UmTweetBemFormadoComUsuario_ETexto_, "giovannibassi", "@quantoeh 2 + 4")
               .Quando(SolicitoUmResultadoAEsteTweet)
               .Entao(TenhoOResultaoEsperado_, "6 RT @giovannibassi: @quantoeh 2 + 4")

               .ComCenario("tweet bem formado com uma expressao complexa")
               .Dado(UmTweetBemFormadoComUsuario_ETexto_, "giovannibassi", "@quantoeh 3 * (2 ** 5) - 5")
               .Quando(SolicitoUmResultadoAEsteTweet)
               .Entao(TenhoOResultaoEsperado_, "91 RT @giovannibassi: @quantoeh 3 * (2 ** 5) - 5")

               .Execute();

        }

        private void UmTweetBemFormadoComUsuario_ETexto_(string usuario, string texto)
        {
            _tweetParaProcessar = new TweetParaProcessar(usuario, texto, 1);
        }

        private void SolicitoUmResultadoAEsteTweet()
        {
            _resposta = _tweetParaProcessar.ProcessarResposta();
        }

        private void TenhoOResultaoEsperado_(string esperado)
        {
            Assert.AreEqual(esperado, _resposta.Texto);
        }
    }
}