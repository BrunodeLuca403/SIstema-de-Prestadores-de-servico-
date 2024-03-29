﻿using DevIO.Business.Interfaces;

namespace DevIO.Business.Notificacoes
{
    public class Notificador : INotificador
    {
        private  List<Notificacao> _notificacoes;

        public Notificador()
        {
            _notificacoes = new List<Notificacao>();  
        }

        public void handle(Notificacao notificacao)
        {
            _notificacoes.Add(notificacao);
        }

        public List<Notificacao> ObterNotificacao()
        {
            return _notificacoes;
        }

        public bool TemNotificacao()
        {
            return _notificacoes.Any();
        }
    }
}
