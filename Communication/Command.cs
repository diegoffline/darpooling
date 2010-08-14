﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace Communication
{
    public interface IDarPoolingOperations
    {
        Result Join(string username, string password);
        Result Unjoin(string username);
        Result RegisterUser(User newUser);
        Result InsertTrip(Trip trip);
        // Result SearchTrip();
    }

    public interface ICommand
    {
        Result Execute();
        Result EndExecute(IAsyncResult asyncValue);
    }

    /// <summary>
    /// The command class allows arbitrary commands to be executed.
    /// </summary>
    [DataContract]
    [KnownType(typeof(JoinCommand))]
    public abstract class Command : ICommand
    {
        protected int commandID;
        protected IDarPoolingOperations receiver;
        protected AsyncCallback callbackMethod;
        protected Result result;

        public abstract Result Execute();
        public abstract Result EndExecute(IAsyncResult asyncValue);

        public int CommandID
        {
            get { return commandID; }
            set { commandID = value; }
        }

        public IDarPoolingOperations Receiver 
        {
            get { return receiver; }
            set { receiver = value; }
        }

        public AsyncCallback Callback
        {
            set { callbackMethod = value; }
        }
    }

    [DataContract]
    public class JoinCommand : Command 
    {
        private UserNode node;
        [DataMember]
        private string username;
        [DataMember]
        private string passwordHash;
        public delegate Result Login(string x, string y);
        Login login;
        
        public JoinCommand(UserNode node, string username, string passwordHash)
        {
            this.node = node;
            this.username = username;
            this.passwordHash = passwordHash;
        }

        public override Result Execute()
        {
            login = new Login(receiver.Join);
            login.BeginInvoke(username, passwordHash, callbackMethod, this);
            return new LoginOkResult();
        }

        public override Result EndExecute(IAsyncResult asyncValue)
        {
            // Obtain the AsyncResult object
            AsyncResult asyncResult = (AsyncResult)asyncValue;
            // Obtain the delegate
            Login l = (Login) asyncResult.AsyncDelegate;
            // Obtain the return value of the invoked method
            result = l.EndInvoke(asyncValue);
            return result;
        }
    }

    public class UnjoinCommand : Command
    {
        public override Result Execute()
        {
            throw new NotImplementedException();
        }

        public override Result EndExecute(IAsyncResult asyncValue)
        {
            throw new NotImplementedException();
        }
    }
    public class RegisterUserCommand : Command
    {
        public override Result Execute()
        {
            throw new NotImplementedException();
        }

        public override Result EndExecute(IAsyncResult asyncValue)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertTripCommand : Command
    {
        public override Result Execute()
        {
            throw new NotImplementedException();
        }

        public override Result EndExecute(IAsyncResult asyncValue)
        {
            throw new NotImplementedException();
        }
    }
    public class SearchTripCommand : Command
    {
        public override Result Execute()
        {
            throw new NotImplementedException();
        }

        public override Result EndExecute(IAsyncResult asyncValue)
        {
            throw new NotImplementedException();
        }
    }
}