using System;
using Domain;
using Domain.Interfaces;
using Newtonsoft.Json;

namespace CleanArchitectureLearn
{
    partial class Program
    {
        public class PrefixedConsolePresenter : IPresenter
        {
            private string _prefix;

            public PrefixedConsolePresenter(string prefix)
            {
                _prefix = prefix;
            }

            public void Success<TResponse>(TResponse response)
            {
                Console.WriteLine($"{_prefix} {JsonConvert.SerializeObject(response)}");
            }

            public void Error(string error)
            {
                Console.WriteLine(error);
            }
        }
    }
}