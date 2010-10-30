﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using QuantoEh.Dominio;
using QuantoEh.Worker;
using StoryQ.pt_BR;

namespace QuantoEh.Tests
{
    [TestFixture]
    public class AvaliadorDeTweetsSpec_Calcular
    {
        private AvaliadorDeTweets _avaliadorDeTweets;
        private Mock<IRepositorioDeTweetsParaProcessar> _repositorioDeTweetsParaProcessar;
        private Mock<IRespostasParaRetuitar> _respostasParaRetuitar;
        private int _calculados;

        [Test]
        public void RodarVerificadorDeTweet()
        {
            new Historia("calcular um tweet")
               .Para("saber quanto da um tweet")
               .Enquanto("usuario do quantoeh")
               .EuQuero("que o avaliador calcule meus tweets")

               .ComCenario("tweet bem formado com soma simples")
               .Dado(UmRepositorioDeTweetsParaProcessarCom2Tweets)
               .E(UmaListaDeRespostas)
               .E(UmAvaliadordorDeTweets)
               .Quando(OAvaliadorCalculaOsTweets)
               .Entao(AFileDeProcessamentoDeCalculoFoiZerada)
               .E(AFilaDeRetweetTem2TweetsParaRetuitar)
               .E(OAvaliadorConfirmouQue2TuitesForamProcessados)

               .Execute();

        }

        private void UmRepositorioDeTweetsParaProcessarCom2Tweets()
        {
            _repositorioDeTweetsParaProcessar = new Mock<IRepositorioDeTweetsParaProcessar>();
            var listaDeTweetsNovos = new List<TweetParaProcessar>
                                                          {
                                                              new TweetParaProcessar("testequantoeh: @quantoeh 2 + 3"),
                                                              new TweetParaProcessar("testequantoeh: @quantoeh 2 * 3")
                                                          };
            _repositorioDeTweetsParaProcessar.Setup(r => r.ObterTodos()).Returns(listaDeTweetsNovos);
        }

        private void UmAvaliadordorDeTweets()
        {
            _avaliadorDeTweets = new AvaliadorDeTweets(new Mock<IMenções>().Object, _repositorioDeTweetsParaProcessar.Object, _respostasParaRetuitar.Object);
        }

        private void UmaListaDeRespostas()
        {
            _respostasParaRetuitar = new Mock<IRespostasParaRetuitar>();
        }

        private void OAvaliadorCalculaOsTweets()
        {
            _calculados = _avaliadorDeTweets.CalcularTweets();
        }

        private void AFileDeProcessamentoDeCalculoFoiZerada()
        {
            _repositorioDeTweetsParaProcessar.VerifyAll();
        }

        private void AFilaDeRetweetTem2TweetsParaRetuitar()
        {
            _respostasParaRetuitar.Verify(r => r.Adicionar("5 RT testequantoeh: @quantoeh 2 + 3"));
            _respostasParaRetuitar.Verify(r => r.Adicionar("6 RT testequantoeh: @quantoeh 2 * 3"));
        }

        private void OAvaliadorConfirmouQue2TuitesForamProcessados()
        {
            Assert.AreEqual(2, _calculados);
        }
    }
}