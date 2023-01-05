using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace ChatApp.Services.Global
{
    public static class InternalDelegates
    {
        #region OnUserPick
        public static event Action<PersonModel> OnUserPick;
        public static void CallOnUserPick(PersonModel model)
        {
            OnUserPick?.Invoke(model);
        }
        #endregion
        #region OnBroadcast
        public static event Action<ChatMessageModel> OnBroadcast;
        public static void CallOnBroadcast(ChatMessageModel model)
        {
            OnBroadcast?.Invoke(model);
        }
        #endregion
    }
}
