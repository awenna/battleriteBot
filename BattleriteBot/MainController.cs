using System;
using System.Collections;
using Telegram.Bot.Types;

namespace BattleriteBot
{
    public interface IMainController
    {
        void RegisterTG();
        void RegisterDiscord();
        void AddMessage(Chat chat);
        ChatState GetState(Chat chat);
        ChatState AddSigning(Chat chat, Signing signing);
        void Reset(Chat chat);
    }

    public class MainController : IMainController
    {
        private readonly Hashtable _states;

        public MainController()
        {
            _states = new Hashtable();
        }

        public void RegisterTG()
        {
            throw new NotImplementedException();
        }

        public void RegisterDiscord()
        {
            throw new NotImplementedException("No Discord support.");
        }

        public void AddMessage(Chat chat)
        {
            lock (_states)
            {
                if (_states.ContainsKey(chat.Id))
                {
                    _states[chat.Id] = ((ChatState)_states[chat.Id]).AddMessage();
                    return;
                }

                var state = new ChatState().AddMessage();
                _states.Add(chat.Id, state);
            }
        }

        public ChatState GetState(Chat chat)
        {
            lock (_states)
            {
                if (_states.ContainsKey(chat.Id))
                    return (ChatState) _states[chat.Id];

                var state = new ChatState();
                _states.Add(chat.Id, state);

                return state;
            }
        }

        public ChatState AddSigning(Chat chat, Signing signing)
        {
            lock (_states)
            {
                if (!_states.ContainsKey(chat.Id))
                {
                    var state = new ChatState();
                    _states.Add(chat.Id, state);
                }
                var current = (ChatState)_states[chat.Id];
                current = current.AddSigning(signing);
                _states[chat.Id] = current;

                return current;
            }
        }

        public void Reset(Chat chat)
        {
            lock (_states)
            {
                if (_states.ContainsKey(chat.Id))
                {
                    var state = new ChatState();
                    _states[chat.Id] = state;
                }
            }
        }
    }
}
