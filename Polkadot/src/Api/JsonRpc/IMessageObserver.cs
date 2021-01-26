﻿using System;
using System.IO;

namespace Polkadot.Api
{
    public interface IMessageObserver
    {
        /**
         *  Do not call API in message handler thread
         */
        void HandleMessage(string payload);

        void OnError(Exception exception);
    }
}