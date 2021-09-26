using System;
using System.Collections.Generic;
using System.Linq;

namespace FormulaParser
{
    internal class SignalCollection : ISignalNameProvider
    {
        public static IDictionary<string, Signal> Signals { get; set; }

        public IEnumerable<string> SignalNames => Signals.Keys;

        public SignalCollection()
        {
            Signals = new Dictionary<string, Signal>(StringComparer.OrdinalIgnoreCase)
            {
                { "Const50", new Signal("Const50", 50d, 20)},
                { "RandomSignal", new Signal("RandomSignal", 20)},
                { "BitSignal", new Signal("BitSignal", 20)},
                { "Sinus", new Signal("Sinus", 20)},
            };

            Random r = new Random();

            Signals.TryGetValue("RandomSignal", out Signal randSig);
            Signals.TryGetValue("BitSignal", out Signal bitSig);
            Signals.TryGetValue("Sinus", out Signal sinus);
            for (int i = 0; i < 20; i++)
            {
                randSig.Samples[i] = r.Next(-200, 200);
                bitSig.Samples[i] = r.Next(0, 2);
                sinus.Samples[i] = Math.Sin(i / 10d);
            }
        }

        public static Signal GetSignal(string name)
        {
            Signals.TryGetValue(name, out var result);
            return result;
        }

        public static Signal GetSignal(int index)
        {
            return Signals.FirstOrDefault(s => s.Value.Id == index).Value;
        }

        public static void AddRandomSignalToCollection(string name, int size)
        {
            Random r = new Random();
            var signal = new Signal(name, size);
            for (int i = 0; i < size; i++)
            {
                signal.Samples[i] = r.Next(-50, 50);
            }
            Signals.Add(name, signal);
            //Console.WriteLine($"=== Signal added to SignalCollection: ${signal.Id}  ===");
        }

        public static void AddResultSignalToCollection(Signal signal)
        {
            Signals.Add($"result_{Signals.Count}", signal);
        }
    }
}